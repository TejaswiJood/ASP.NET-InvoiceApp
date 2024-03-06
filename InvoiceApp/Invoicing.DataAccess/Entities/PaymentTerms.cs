namespace Invoicing.DataAccess.Entities
{
    public class PaymentTerms
    {
        public int PaymentTermsId { get; set; }

        public string Description { get; set; } = null!;

        public int DueDays { get; set; }
        public List<Invoice>? Invoices { get; set; }
    }
}
