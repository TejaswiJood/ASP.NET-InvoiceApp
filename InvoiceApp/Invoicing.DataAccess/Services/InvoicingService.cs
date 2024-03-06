using Invoicing.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Invoicing.DataAccess.Services
{
    public class InvoicingService : IInvoicingService
    {
		private readonly InvoicingDbContext _invoicingDbContext;
		public InvoicingService(InvoicingDbContext invoicingDbContext)
		{
			_invoicingDbContext = invoicingDbContext;
		}
		public List<Customer> GetCustomersFromTo(string filterFrom = "A", string filterTo = "Z")
		{
			return _invoicingDbContext.Customer
				.Where(c => string.Compare(c.Name, filterFrom) >= 0 &&
				string.Compare(c.Name, filterTo) <= 0 &&
				c.IsDeleted == false
				)
				.ToList();
		}
		public Customer? GetCustomerById(int customerId)
		{
			return _invoicingDbContext.Customer.FirstOrDefault(c => c.CustomerId == customerId);
		}
		public void UpdateIsDeleted(int customerId)
		{

			var customer = GetCustomerById(customerId);
			if (customer != null)
			{
				customer.IsDeleted = !customer.IsDeleted;
				_invoicingDbContext.Customer.Update(customer);
				_invoicingDbContext.SaveChanges();
			}


		}
		public int AddCustomer(Customer customer)
		{
			_invoicingDbContext.Customer.Add(customer);
			_invoicingDbContext.SaveChanges();
			return customer.CustomerId;
		}
		public void EditCustomer(Customer customer)
		{
			_invoicingDbContext.Customer.Update(customer);
			_invoicingDbContext.SaveChanges();

		}

        public List<Invoice> GetInvoicesByCustomerId(int customerId)
        {
            var invoices = _invoicingDbContext.Invoices
                    .Where(i => i.CustomerId == customerId)
                    .OrderBy(i => i.InvoiceDate)
                    .ToList();

            return invoices;
        }

        public Invoice? GetInvoiceById(int invoiceId)
        {
            return GetBaseQuery()
                    .Where(i => i.InvoiceId == invoiceId)
                    .FirstOrDefault();
        }

        public List<InvoiceLineItem> GetInvoiceLinestemsByInvoiceId(int invoiceId)
        {
            return _invoicingDbContext.InvoiceLineItem
                    .Include(i => i.Invoice)
                    .Where(i => i.InvoiceId == invoiceId)
                    .ToList();
        }

        public int AddNewInvoice(Invoice invoice)
        {
            _invoicingDbContext.Invoices.Add(invoice);
            _invoicingDbContext.SaveChanges();
            return invoice.InvoiceId;
        }

        public InvoiceLineItem AddNewInvoiceLineItem(InvoiceLineItem invoiceLineItem)
        {
            _invoicingDbContext.InvoiceLineItem.Add(invoiceLineItem);
            _invoicingDbContext.SaveChanges();
            return invoiceLineItem;
        }

        public List<PaymentTerms> GetPaymentTerms()
        {
            return _invoicingDbContext.PaymentTerms
                .OrderBy(p => p.PaymentTermsId)
                .ToList();
        }

        private IQueryable<Invoice> GetBaseQuery()
        {
            return _invoicingDbContext.Invoices
                .Include(i => i.Customer)
                .Include(i => i.InvoiceLineItems)
                .Include(i => i.PaymentTerms);
        }
    }
}