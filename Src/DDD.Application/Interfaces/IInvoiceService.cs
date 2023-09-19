using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.ViewModels;

namespace DDD.Application.Interfaces
{
    public interface IInvoiceService : IDisposable
    {
        IEnumerable<InvoiceVM> GetByEditorId(int skip, int take, string userID);
        bool AddInvoice(InvoiceVM _invoiceVM);

    }
}
