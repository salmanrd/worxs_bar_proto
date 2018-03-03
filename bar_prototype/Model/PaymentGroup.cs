using System.Collections.Generic;

namespace bar_prototype.Model
{
    public class PaymentGroup
    {
        public PaymentGroup()
        {
            PaymentInstructions = new List<PaymentInstruction>();
        }

        public List<PaymentInstruction> PaymentInstructions { get; set; }

        public double CalculateTotalPayment()
        {
            var total = 0d;

            foreach (var item in PaymentInstructions)
            {
                total += item.Amount;
            }

            return total;
        }
    }



}
