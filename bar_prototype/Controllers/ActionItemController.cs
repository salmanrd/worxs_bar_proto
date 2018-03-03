using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
