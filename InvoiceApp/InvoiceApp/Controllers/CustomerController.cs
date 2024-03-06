using InvoiceApp.Models;
using Invoicing.DataAccess.Entities;
using Invoicing.DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
	public class CustomerController : Controller
	{
		private readonly IInvoicingService _invoicingService;
		public CustomerController(IInvoicingService invoicingService)
		{
			_invoicingService = invoicingService;
		}
		[HttpGet("/customers/{filterFrom}-{filterTo}")]
		public IActionResult List(string filterFrom, string filterTo)
		{
			var customersViewModel = new CustomersViewModel()
			{
				Customers = _invoicingService.GetCustomersFromTo(filterFrom, filterTo),
				ActivePage = $"{filterFrom}-{filterTo}",
			};
			return View(customersViewModel);
		}

		[HttpGet("/customers/add")]
		public IActionResult Add() {
			var customerViewModel = new CustomerViewModel()
			{
				ActiveCustomer = new Invoicing.DataAccess.Entities.Customer()
			};
			return View(customerViewModel);

		}

		[HttpPost("/customers/add")]
		public IActionResult Add(CustomerViewModel customerViewModel)
		{
			if (!ModelState.IsValid) {
				return View(customerViewModel); }

			_invoicingService.AddCustomer(customerViewModel.ActiveCustomer);
			TempData["Message"] = $"Customer {customerViewModel.ActiveCustomer.Name} was added successfully!";
			TempData["className"] = "success";
			return RedirectToAction("List", new { filterFrom = "A", filterTo = "E" });

		}

		[HttpGet("/customers/edit/{customerId:int}")]
		public IActionResult Edit(int customerId)
		{
			var customer = _invoicingService.GetCustomerById(customerId);
			if (customer == null) { return NotFound(); }
			var customerViewModel = new CustomerViewModel()
			{
				ActiveCustomer = _invoicingService.GetCustomerById(customerId)
			};
			return View(customerViewModel);
		}

		[HttpPost("/customers/edit/{customerId:int}")]
		public IActionResult Edit(int customerId, CustomerViewModel customerViewModel)
		{
			if (!ModelState.IsValid) return View(customerViewModel);

			_invoicingService.EditCustomer(customerViewModel.ActiveCustomer);

			TempData["Message"] = $"Customer {customerViewModel.ActiveCustomer.Name} was updated successfully!";
			TempData["className"] = "info";

			return RedirectToAction("List", new { filterFrom = "A", filterTo = "E" });
		}

		[HttpGet("/customers/delete/{customerId:int}")]
		public IActionResult Delete(int customerId)
		{
			var customer = _invoicingService.GetCustomerById(customerId);

			if (customer == null) { return NotFound(); }


			_invoicingService.UpdateIsDeleted(customerId);
			TempData["Message"] = $"The customer \"{customer.Name}\"was deleted";
			TempData["ClassName"] = "danger";
			TempData["CustomerId"] = customerId;
			return RedirectToAction("List", new { filterFrom = "A", filterTo = "E" });
		}

		[HttpGet("/customers/{customerId:int}/undo-delete")]
		public IActionResult UndoDelete(int customerId)
		{
			var customer = _invoicingService.GetCustomerById(customerId);

			if (customer == null) { return NotFound(); }


			_invoicingService.UpdateIsDeleted(customerId);
			TempData["Message"] = $"The customer \"{customer.Name}\" is added again";
			TempData["ClassName"] = "success";

			return RedirectToAction("List", new { filterFrom = "A", filterTo = "E" });
		}

		[HttpGet("/customers/invoice/{customerId:int}")]
		public IActionResult Invoice(int customerId)
		{
			var customer = _invoicingService.GetCustomerById(customerId);
			if (customer == null) { return NotFound(); }
			var model = new InvoiceViewModel()
			{
				Invoices= _invoicingService.GetInvoicesByCustomerId(customerId),
				ActiveCustomer= customer,
				PaymentTermsList = _invoicingService.GetPaymentTerms(),
				
				
			};
			if(model.Invoices.Count > 0)
			{
                model.SelectedInvoice = (_invoicingService.GetCustomerById(customerId)).Invoices[0];

                model.SelectedInvoice.InvoiceLineItems = _invoicingService.GetInvoiceLinestemsByInvoiceId(model.SelectedInvoice.InvoiceId);

            }
            return View(model);

		}

        [HttpGet("/customer/GetInvoiceLineItem")]
        public IActionResult GetInvoiceLineItems(int customerId, int invoiceId)
        {
            InvoiceViewModel invoice = new InvoiceViewModel()
            {
                Invoices = _invoicingService.GetInvoicesByCustomerId(customerId),
                ActiveCustomer = _invoicingService.GetCustomerById(customerId),
                PaymentTermsList = _invoicingService.GetPaymentTerms(),
                NewInvoice = new Invoice(),
                SelectedInvoice = _invoicingService.GetInvoiceById(invoiceId),
				SelectedPaymentTerms = new InvoiceLineItem()
            };

            invoice.SelectedInvoice.InvoiceLineItems = _invoicingService.GetInvoiceLinestemsByInvoiceId(invoice.SelectedInvoice.InvoiceId);

            return View("Invoice", invoice);
        }
        


        [HttpPost("/customers/{customerId:int}/invoices")]
        public IActionResult AddInvoice(int customerId, InvoiceViewModel invoice)
        {
            invoice.NewInvoice.CustomerId = customerId;

            _invoicingService.AddNewInvoice(invoice.NewInvoice);


            TempData["Message"] = $"The invoice was added.";
            TempData["ClassName"] = "success";


            return RedirectToAction("Invoice", "Customer", new { customerId = customerId });


        }     

				

		[HttpPost("/customers/{customerId:int}/invoices/{invoiceId:int}")]
		public IActionResult AddLineItem(int customerId, int invoiceId, InvoiceViewModel invoice)
		{	
			_invoicingService.AddNewInvoiceLineItem(invoice.SelectedPaymentTerms);


			
            TempData["Message"] = $"The line item was added.";
            TempData["ClassName"] = "success";


            return RedirectToAction("Invoice", "Customer", new { customerId = customerId, invoiceId=invoiceId });
        }

	}
}
