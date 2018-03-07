using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bar_prototype.Model;
using bar_prototype.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bar_prototype.Controllers
{
    public class PaymentInstructionController : Controller
    {
        PaymentInstructionRepository repository;
        ActionItemRepository actionItemRepository;
        IdGenerator idGenerator;
        public PaymentInstructionController()
        {
            repository = new PaymentInstructionRepository();
            idGenerator = new IdGenerator();
            actionItemRepository = new ActionItemRepository();
        }

        public IActionResult Index()
        {
            return View(repository.GetAll());
        }

        public IActionResult Create()
        {
            return View("PaymentInstructionAdd", new PaymentInstructionViewModel());
        }

        public IActionResult Details(int id)
        {
            var item = repository.Get(id);

            var viewModel = new PaymentInstructionViewModel
            {
                Id = item.Id,
                Amount = item.Amount,
                SelectedPaymentMethod = item.PaymentMethod
            };
            return View("PaymentInstructionAdd", viewModel);
        }

        [HttpPost]
        public IActionResult Create(PaymentInstructionViewModel viewModel)
        {
            PaymentInstruction paymentInstruction = null;
            switch (viewModel.SelectedPaymentMethod)
            {
                case "Cash":
                    paymentInstruction = new Cash();
                    break;
                case "Cheque":
                    paymentInstruction = new Cheque();
                    break;
            }

            paymentInstruction.Id = idGenerator.GetId();
            paymentInstruction.Amount = viewModel.Amount;
            paymentInstruction.Status = StatusType.Draft;
            repository.Add(paymentInstruction);

            return View("Index", repository.GetAll());
        }

        public IActionResult Submit(int id)
        {
            var item = repository.Get(id);
            item.Status = StatusType.Pending;


           

            return RedirectToAction("Index");
        }

    }
}
