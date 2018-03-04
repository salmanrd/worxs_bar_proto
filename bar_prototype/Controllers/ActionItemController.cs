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
    public class ActionItemController : Controller
    {
       
        public ActionItemRepository actionItemRepository;
        public IdGenerator idGenerator;
        PaymentInstructionRepository paymentInstructionRepository;
        public ActionItemController()
        {
            actionItemRepository = new ActionItemRepository();
            idGenerator = new IdGenerator();
            paymentInstructionRepository = new PaymentInstructionRepository();
        }


        public IActionResult Index()
        {
            return View(actionItemRepository.GetAll());
        }

        public IActionResult Create()
        {
            var actionItem = new Draft
            {
                Id = idGenerator.GetId(),
                Status = StatusType.Pending
            };
           
            actionItemRepository.Add(actionItem);

            return View("ActionItem", actionItem);
        }

        public IActionResult Details(int id)
        {
            var item = actionItemRepository.Get(id);
            return View("ActionItem", item);
        }

        public IActionResult AddFee(int id, int feeAmount)
        {
            
            var actionItem = actionItemRepository.Get(id);

            actionItem.Fees.Add(
                new Fee
            {
                Code = string.Format("X{0}", idGenerator.GetIdMax100()),
                Version = idGenerator.GetIdMax10(),
                Description = "Claim fees for claims < 3000",
                CaseReference = "xxxx-xxxx-xxxx-xxxx",
                CalculatedAmount = feeAmount

            });

            return RedirectToAction("Details", new {id = id});
        }

        public IActionResult AddPayment(int id, int paymentInstructionId)
        {

            var paymentInstruction = paymentInstructionRepository.Get(paymentInstructionId, StatusType.Submitted);

            if (paymentInstruction == null)
            {
                ModelState.AddModelError("Error", "Payment instruction not found");
                return RedirectToAction("Details", new { id = id });
            }

            var actionItem = actionItemRepository.Get(id);

            actionItemRepository.Remove(actionItem);
            actionItem.PaymentGroup.PaymentInstructions.Add(paymentInstruction);
            actionItemRepository.Add(actionItem);

            return RedirectToAction("Details", new { id = id });
        }


        public IActionResult Process(int id)
        {
            var actionItem = actionItemRepository.Get(id);
            if (actionItem.CalculateUnallocatedPayment() != 0)
            {
                ModelState.AddModelError("Error", "Cannot process");
                return RedirectToAction("Details", new { id = id });
            }


            var processed = new Processed
            {
                Id = actionItem.Id,
                Status = StatusType.PendingApproval
            };
            processed.Fees = actionItem.Fees;
            processed.PaymentGroup = actionItem.PaymentGroup;
            processed.Remissions = actionItem.Remissions;

            actionItemRepository.Remove(actionItem);
            actionItemRepository.Add(processed);

            return RedirectToAction("Index");
        }

        public IActionResult TransferToBar(int id)
        {
            var item = actionItemRepository.Get(id);
            item.Status = StatusType.TransferredToBar;




            return RedirectToAction("Index");
        }
    }
}
