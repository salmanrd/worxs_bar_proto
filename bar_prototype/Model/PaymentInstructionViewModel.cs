using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bar_prototype.Model
{
    public class PaymentInstructionViewModel
    {
        public PaymentInstructionViewModel()
        {
            PaymentMethods = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cash", Value = "Cash" },
                new SelectListItem { Text = "Cheque", Value = "Cheque" }
            };
        }

        public List<SelectListItem> PaymentMethods { get; set; }
        public string SelectedPaymentMethod { get; set; }
        public double Amount { get; set; }
        public int Id { get; set; }
    }



}
