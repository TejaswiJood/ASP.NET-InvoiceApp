using Invoicing.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InvoiceApp.Tests
{
	public class InvoicingServiceTests
	{
		[Theory]
		[InlineData(1)]

		public void UpdateIsDeleted_SetIsDeletedTrue(int customerId)
		{
			var options = new DbContextOptionsBuilder<InvoicingDbContext>().UseInMemoryDatabase(databaseName: "InvoiceDb").Options;
			var invoicingDbContext = new InvoicingDbContext(options);
			var invoicingService = new InvoicingService(invoicingDbContext);

			var customer = new Customer()
			{
				CustomerId = 1,
				Name = "Blanchard & Johnson Associates",
				Address1 = "27371 Valderas",
				City = "Mission Viejo",
				ProvinceOrState = "CA",
				ZipOrPostalCode = "92691",
				Phone = "214-555-3647",
				ContactEmail = "kgonz@bja.com",
				ContactFirstName = "Keeton",
				ContactLastName = "Gonzalo"
			};

			invoicingDbContext.Customer.Add(customer);
			invoicingDbContext.SaveChanges();

			invoicingService.UpdateIsDeleted(customerId);
			

			Assert.True(customer.IsDeleted);

		}
	}
}
