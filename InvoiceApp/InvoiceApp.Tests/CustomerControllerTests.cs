using Invoicing.DataAccess.Entities;

using Microsoft.AspNetCore.Http;

namespace InvoiceApp.Test
{
	public class CustomerControllerTests
	{
		private readonly Mock<IInvoicingService> _mockInvoicingService = new Mock<IInvoicingService>();
		private readonly Mock<ITempDataDictionary> _tempData = new Mock<ITempDataDictionary>();
		[Fact]
		public void Add_GET_ReturnsViewResult()
		{
			var controller = new CustomerController(_mockInvoicingService.Object);
			var result = controller.Add();
			Assert.IsType<ViewResult>(result);
		}



		[Fact]
		public void List_GET_ReturnCustomerViewModel()
		{
			// Arrange
			var customer = new List<Customer>()
			{
				new Customer {CustomerId = 1, Name="Alice", IsDeleted=false},
				new Customer {CustomerId = 2, Name="Bob", IsDeleted=false},
			};

			_mockInvoicingService
				.Setup(invoiceManagerService => invoiceManagerService.GetCustomersFromTo(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(customer);

			var controller = new CustomerController(_mockInvoicingService.Object);

			//Act

			var result = controller.List("A", "T") as ViewResult;
			var viewModel = result?.Model as CustomersViewModel;

			//Assert
			Assert.NotNull(viewModel);
			Assert.Equal(customer, viewModel.Customers);

		}

		[Fact]
		public void Edit_GET_ReturnsNotFoundResult_WhenCustomerDoesNotExists()
		{
			//Arrange
			_mockInvoicingService
				.Setup(invoiceManagerService => invoiceManagerService.GetCustomerById(It.IsAny<int>()))
				.Returns((Customer)null);

			var controller = new CustomerController(_mockInvoicingService.Object);
			//Act
			var result = controller.Edit(2);
			//Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Delete_GET_ReturnsRedirectToActionResult()
		{
			var customer = new Customer { CustomerId = 1, Name = "Eren", IsDeleted = false };
			_mockInvoicingService
				.Setup(invoiceManagerService => invoiceManagerService.GetCustomerById(1))
				.Returns(customer);
			_mockInvoicingService
				.Setup(invoiceManagerService => invoiceManagerService.UpdateIsDeleted(1))
				.Verifiable();

			var controller = new CustomerController(_mockInvoicingService.Object) { TempData = _tempData.Object };

			var result = controller.Delete(1);

			Assert.IsType<RedirectToActionResult>(result);

		}

		[Fact]
		public void UndoDelete_GET_ReturnsRedirectToActionResult()
		{
			var customer = new Customer { CustomerId = 1, Name = "Eren", IsDeleted = false };
			_mockInvoicingService
				.Setup(invoiceManagerService => invoiceManagerService.GetCustomerById(1))
				.Returns(customer);
			_mockInvoicingService
				.Setup(invoiceManagerService => invoiceManagerService.UpdateIsDeleted(1))
				.Verifiable();

			var controller = new CustomerController(_mockInvoicingService.Object) { TempData = _tempData.Object };

			var result = controller.UndoDelete(1);

			Assert.IsType<RedirectToActionResult>(result);

		}
	}
}