using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Commands;
using DDD.Domain.Core.Bus;
using DDD.Domain.Interfaces;
using DDD.Domain.Specifications;

namespace DDD.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceReposistory _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler Bus;

        public InvoiceService(IInvoiceReposistory invoiceRepository, IMapper mapper, IMediatorHandler bus)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            Bus = bus;

        }

        public IEnumerable<InvoiceVM> GetByEditorId(int skip, int take, string userID)
        {
            return _invoiceRepository.GetAll(new InvoiceByEditorFilterPaginatedSpecification(skip, take, userID))
                .ProjectTo<InvoiceVM>(_mapper.ConfigurationProvider);
        }

        public bool AddInvoice(InvoiceVM _invoiceVM)
        {
            try
            {
                var registerCommand = _mapper.Map<AddNewInvoiceCommand>(_invoiceVM);
                Bus.SendCommand(registerCommand);
                //_dbContext.SaveChanges();
                //_bookRepository.Add(bookViewModel);
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
    }
}
