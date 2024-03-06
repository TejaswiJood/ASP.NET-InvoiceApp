using Invoicing.DataAccess.Entities;
namespace InvoiceApp.Models
{
    public class InvoiceViewModel : CustomerViewModel
    {
        public Invoice NewInvoice { get; set; }

        public List<PaymentTerms> PaymentTermsList { get; set; }


        public Invoice SelectedInvoice { get; set; }
        public List<Invoice> Invoices { get; set; }
        public InvoiceLineItem SelectedPaymentTerms { get; set; }
    }
}
