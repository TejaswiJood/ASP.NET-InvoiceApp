using System.ComponentModel.DataAnnotations;

namespace Invoicing.DataAccess.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

		[Required(ErrorMessage = "Enter the Name.")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Enter the Address of the Customer.")]
		public string? Address1 { get; set; }

        public string? Address2 { get; set; }

		[Required(ErrorMessage = "Enter the City of the Customer.")]
		public string? City { get; set; } = null!;
		
        [Required(ErrorMessage = "Enter the Province of the Customer.")]
		public string? ProvinceOrState { get; set; } = null!;

		[Required(ErrorMessage = "Enter the Zipcode of the Customer.")]
		public string? ZipOrPostalCode { get; set; } = null!;
		
		[Required(ErrorMessage = "Enter the Phone Number of the Customer.")]
		public string? Phone { get; set; }

		public string? ContactLastName { get; set; }

		public string? ContactFirstName { get; set; }

		[RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Enter a a valid email address ")]
		public string? ContactEmail { get; set; }

        public bool IsDeleted { get; set; } = false;
    
        public List<Invoice>? Invoices { get; set; }
    }
}
