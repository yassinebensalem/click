using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Events;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Specifications;
using DDD.Infra.Data.Context;
using DDD.Infra.Data.Repository;
using DDD.Infra.Data.Repository.EventSourcing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace DDD.Application.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly IMapper _mapper;
        private readonly ICommunityRepository _communityRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMediatorHandler Bus;

        public CommunityService(IMapper mapper, ICommunityRepository communityRepository,
            UserManager<ApplicationUser> userManager,
            IEventStoreRepository eventStoreRepository,
            ApplicationDbContext dbContext,
            IMediatorHandler bus)
        {
            _mapper = mapper;
            _communityRepository = communityRepository;
            _userManager = userManager;
            _eventStoreRepository = eventStoreRepository;
            _dbContext = dbContext;
            Bus = bus;
        }

        //start implementing needed services
        public IEnumerable<CommunityViewModel> GetAll()
        {
            return _communityRepository.GetAll().ProjectTo<CommunityViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<CommunityViewModel> GetAll(int skip, int take)
        {
            return _communityRepository.GetAll(new CommunityFilterPaginatedSpecification(skip, take))
                .ProjectTo<CommunityViewModel>(_mapper.ConfigurationProvider);
        }

        public CommunityViewModel GetCommunityById(Guid Id)
        {
            return _mapper.Map<CommunityViewModel>(_communityRepository.GetById(Id));
        }

        public async Task<bool> AddCommunity(CommunityEditViewModel communityViewModel)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == communityViewModel.AdminEmail);
                if(user == null)
                {
                    return false;
                }

                communityViewModel.AdminId = user.Id;
                var registerCommand = _mapper.Map<RegisterNewCommunityCommand>(communityViewModel);
                await Bus.SendCommand(registerCommand);

                communityViewModel.Id = registerCommand.Id;

                await _userManager.AddToRoleAsync(user, "CommunityAdmin");

                //_bookRepository.Add(bookViewModel);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCommunity(CommunityEditViewModel communityViewModel)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(communityViewModel.AdminEmail);
                if (user == null)
                {
                    return false;
                }
                communityViewModel.AdminId = user.Id;

                var oldCommunityAdminUserRoles = from community in _dbContext.Communities
                                                 join communityMember in _dbContext.CommunityMembers on community.Id equals communityMember.CommunityId
                                              join u in _dbContext.Users on communityMember.MemberId equals u.Id
                                              join ur in _dbContext.UserRoles on u.Id equals ur.UserId
                                              join r in _dbContext.Roles on ur.RoleId equals r.Id
                                              where community.Id == communityViewModel.Id
                                              && communityMember.IsCommunityAdmin
                                              && u.Id != communityViewModel.AdminId && r.Name == "CommunityAdmin"
                                              select ur;
                if (oldCommunityAdminUserRoles != null)
                {
                    _dbContext.UserRoles.RemoveRange(oldCommunityAdminUserRoles);
                    await _userManager.AddToRoleAsync(user, "CommunityAdmin");
                }

                var updateCommand = _mapper.Map<UpdateCommunityCommand>(communityViewModel);
                await Bus.SendCommand(updateCommand);
                //_bookRepository.Update(b);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool DeleteCommunity(Guid id)
        {
            try
            {
                var removeCommand = new RemoveCommunityCommand(id);
                Bus.SendCommand(removeCommand);
                var userRolesCommunityAdmin = from community in _dbContext.Communities
                                              join communityMember in _dbContext.CommunityMembers on community.Id equals communityMember.CommunityId
                                              join u in _dbContext.Users on communityMember.MemberId equals u.Id
                                              join ur in _dbContext.UserRoles on u.Id equals ur.UserId
                                               join r in _dbContext.Roles on ur.RoleId equals r.Id
                                               where community.Id == id
                                               && communityMember.IsCommunityAdmin
                                               && r.Name == "CommunityAdmin"
                                               select ur;
                if(userRolesCommunityAdmin != null)
                {
                    _dbContext.UserRoles.RemoveRange(userRolesCommunityAdmin);
                }
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public IList<CommunityHistoryData> GetAllHistory(Guid id)
        {
            return CommunityHistory.ToJavaScriptCommunityHistory(_eventStoreRepository.All(id));
        }


        public List<CommunityViewModel> ListCommunity(string communityName)
        {
            return _communityRepository.GetAll(x => x.CommunityName == communityName).
                ProjectTo<CommunityViewModel>(_mapper.ConfigurationProvider).ToList();
        }

        public List<ApplicationUserViewModel> GetUsers(string keywords, int skip, int take)
        {
            string[] keywordGroups = new string[15];
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                keywordGroups = keywords.Split(' ');
                keywords = keywords.ToLower();
            }

            IQueryable<ApplicationUser> query = _userManager.Users;
            if (keywordGroups.Any())
            {
                var queryExample = from user in _dbContext.Users
                        join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                        join role in _dbContext.Roles on userRole.RoleId equals role.Id
                        join communityMember in _dbContext.CommunityMembers.Where(x=>!x.IsCommunityAdmin) on user.Id equals communityMember.MemberId into communityAdmin
                        //join community in _dbContext.Communities on user.Id equals community.AdminId into communityAdmin
                        from subQuery in communityAdmin.DefaultIfEmpty()
                        where role.Name == "Subscriber"
                        select user;

                query = queryExample.Where(user => (user.FirstName + user.LastName + user.Email + user.PhoneNumber).Contains(keywordGroups[0]));

                if (keywordGroups.Length > 1)
                {
                    foreach (string keyword in keywordGroups.Skip(1))
                    {
                        query = query.Union(queryExample.Where(user => (user.FirstName + user.LastName + user.Email + user.PhoneNumber).Contains(keyword)));
                    }
                }
            }

            if (take > 0 && skip >= 0)
            {
                query = query.Skip(skip).Take(take);
            }

            return query.ProjectTo<ApplicationUserViewModel>(_mapper.ConfigurationProvider).ToList();
        }

        public List<ApplicationUserViewModel> GetMembers(Guid communityId, string keywords, int skip, int take)
        {
            string[] keywordGroups = new string[15];
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                keywordGroups = keywords.Split(' ');
                keywords = keywords.ToLower();
            }

            var community = _communityRepository.GetAllWithMembers(x => x.Id == communityId).FirstOrDefault();
            if(community == null) {
                return null;
            }
            IQueryable<ApplicationUser> allMembersQuery = community?.Members.AsQueryable().Select(x => x.Member);
            IQueryable<ApplicationUser> query = community.Members.AsQueryable().Select(x => x.Member);
            if (keywordGroups.Any())
            {
                query = allMembersQuery.Where(x => (x.FirstName + x.LastName + x.Email + x.PhoneNumber).Contains(keywordGroups[0]));
                foreach (string keyword in keywordGroups.Skip(1))
                {
                    query = query.Union(allMembersQuery.Where(x =>
                        (x.FirstName + x.LastName + x.Email + x.PhoneNumber).Contains(keyword)
                        ));
                }
            }

            
            if (take > 0 && skip >= 0)
            {
                query = query.Skip(skip).Take(take);
            }

            return query.ProjectTo<ApplicationUserViewModel>(_mapper.ConfigurationProvider).ToList();
        }

        public bool AssociateMember(CommunityMemberViewModel communityMemberViewModel)
        {
            try
            {
                var associateCommunityCommand = _mapper.Map<AssociateCommunityMemberCommand>(communityMemberViewModel);
                Bus.SendCommand(associateCommunityCommand);
                //_bookRepository.Update(b);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool DissociateMember(CommunityMemberViewModel communityMemberViewModel)
        {
            try
            {
                var dissociateCommunityCommand = _mapper.Map<DissociateCommunityMemberCommand>(communityMemberViewModel);
                Bus.SendCommand(dissociateCommunityCommand);
                //_bookRepository.Update(b);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public async Task<bool> InviteWithMembership(CommunityInvitationalViewModel viewModel)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == viewModel.Email);
                IdentityResult createUserResult = null;
                if (user is null)
                {
                    user = new ApplicationUser();
                    user.Email = viewModel.Email;
                    createUserResult = await _userManager.CreateAsync(user);
                    user = _dbContext.Users.FirstOrDefault(x => x.Email == viewModel.Email);
                    if (createUserResult is null || !createUserResult.Succeeded)
                    {
                        return false;
                    }
                }

                viewModel.MemberId = Guid.Parse(user.Id);

                var inviteWithMembershipCommand = _mapper.Map<InviteToCommunityWithMembershipCommand>(viewModel);
                await Bus.SendCommand(inviteWithMembershipCommand);
                //_bookRepository.Update(b);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool IsMember(Guid? userId, Guid? communityId)
        {
            return _communityRepository.IsMember(userId, communityId);
        }

        public bool CheckMembers(ICollection<Guid> userIds, Guid? communityId)
        {
            return _communityRepository.CheckMembers(userIds.Select(x=>x.ToString()), communityId);
        }
    }
}
