using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.ViewModels;

namespace DDD.Application.Interfaces
{
   public interface IJoinRequestService : IDisposable
    {
        bool AddJoinRequest(JoinRequestVM joinRequestVM);
        IEnumerable<JoinRequestVM> GetAll(JoinRequestPostVM joinRequestPostVM);
        JoinRequestVM GetById(string requestId);
        bool UpdateJoinRequest(JoinRequestVM joinRequestVM);
        JoinRequestVM GetJoinRequestByEmail(string email);
        bool DeleteCompetitionBook(string requestId, string bookId);
        bool AddCompetitionBook(string requestId, string bookId);
        bool GivewayBook(string requestId, string userId);
    }
}
