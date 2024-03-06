using Microsoft.EntityFrameworkCore;
using Invoicing.DataAccess.Entities;
namespace InvoiceApp.Models
{
	public class CustomerViewModel
	{
		public Customer ActiveCustomer { get; set; }
	}
}
