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
       
        public ActionItemRepository repository;
        public IdGenerator idGenerator;
        public ActionItemController()
        {
            repository = new ActionItemRepository();
            idGenerator = new IdGenerator();
        }


        public IActionResult Index()
        {
            return View(repository.GetAll());
        }

        public IActionResult Details(int id)
        {
            var item = repository.Get(id);
            return View("ActionItem", item);
        }

        public IActionResult AddFee(int id, int feeAmount)
        {
            
            var actionItem = repository.Get(id);

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
    }
}
