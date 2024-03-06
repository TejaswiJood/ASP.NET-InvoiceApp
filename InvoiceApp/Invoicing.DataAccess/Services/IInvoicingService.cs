using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoicing.DataAccess.Entities;

namespace Invoicing.DataAccess.Services
{
    public interface IInvoicingService
    {
        public List<Customer> GetCustomersFromTo(string filterFrom = "A", string filterTo = "Z");

        public Customer? GetCustomerById(int customerId);

        public void UpdateIsDeleted(int customerId);
		public int AddCustomer(Customer customer);
		public void EditCustomer(Customer customer);

        public List<Invoice> GetInvoicesByCustomerId(int customerId);

        public Invoice? GetInvoiceById(int invoiceId);

        public List<InvoiceLineItem> GetInvoiceLinestemsByInvoiceId(int invoiceId);

        public int AddNewInvoice(Invoice invoice);

        public InvoiceLineItem AddNewInvoiceLineItem(InvoiceLineItem invoiceLineItem);

        public List<PaymentTerms> GetPaymentTerms();
    }
}
