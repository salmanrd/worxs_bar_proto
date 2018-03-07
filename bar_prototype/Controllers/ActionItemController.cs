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

        public IActionResult CreateProcessed()
        {
            var actionItem = new Processed
            {
                Id = idGenerator.GetId(),
                Status = StatusType.InProgress
            };
           
            actionItemRepository.Add(actionItem);

            return View("ActionItem", actionItem);
        }

        public IActionResult Details(int id)
        {
            var item = actionItemRepository.Get(id);

            if (item.ActionType == "ReplacementPayment")
            {
                return View("ReplacementPayment", item);
            }

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

            var paymentInstruction = paymentInstructionRepository.Get(paymentInstructionId, StatusType.Pending);

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

        public IActionResult LinkSuspenseDeficiency(int id, int suspenseDeficiencyId)
        {

            var actionItem = actionItemRepository.Get(id);

            var suspenseDeficiency = actionItemRepository.Get(suspenseDeficiencyId);

            actionItem.AssociatedActions.Add(suspenseDeficiency);

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

            foreach (var payment in item.PaymentGroup.PaymentInstructions)
            {
                payment.TransferredtoBar = true;
            }


            return RedirectToAction("Index");
        }

        public IActionResult CreateSuspenseDeficiency(int id, int paymentId)
        {
            var item = actionItemRepository.Get(id);

            var paymentInstruction = paymentInstructionRepository.Get(paymentId);
            paymentInstruction.SuspenseDeficiency = true;

            var suspenseDeficiency = new SuspenseDeficiency
            {
                Id = idGenerator.GetId(),
                FailedPayment = paymentInstruction,
                Status = StatusType.InProgress
            };
            suspenseDeficiency.PaymentGroup.PaymentInstructions.Add(paymentInstruction);
            suspenseDeficiency.AssociatedActions.Add(item);

            actionItemRepository.Add(suspenseDeficiency);

            return RedirectToAction("Index");
        }

        public IActionResult ViewSuspenseDeficiency(int id)
        {
            var item = actionItemRepository.Get(id);

            return RedirectToAction("SuspenseDeficiency" , item);
        }

        public IActionResult CreateReplacementPaymentWithSuspenseDef(int id)
        {
            var item = actionItemRepository.Get(id);

            var replacementPayment = new ReplacementPayment
            {
                Id = idGenerator.GetId(),
                Status = StatusType.PendingApproval
            };
     
            replacementPayment.AssociatedActions.Add(item);
            actionItemRepository.Add(replacementPayment);

            return RedirectToAction("Index");
        }

        public IActionResult CreateReplacementPayment()
        {
            var actionItem = new ReplacementPayment
            {
                Id = idGenerator.GetId(),
                Status = StatusType.InProgress
            };

            actionItemRepository.Add(actionItem);

            return View("ReplacementPayment", actionItem);
        }

       /* public IActionResult ViewReplacementPayment(int id)
        {
            var item = actionItemRepository.Get(id);

            return RedirectToAction("ReplacementPayment", item);
        }*/
    }
}
