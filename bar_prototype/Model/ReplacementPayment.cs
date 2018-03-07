using System.Linq;

namespace bar_prototype.Model
{
    public class ReplacementPayment : ActionItem
    {
        public override string ActionType { get { return "ReplacementPayment"; } }


        public override double CalculateUnallocatedPayment()
        {
            var suspenseDeficiency = AssociatedActions.Find(x => x.ActionType == "SuspenseDeficiency");

            var failedPaymentAmount = (suspenseDeficiency as SuspenseDeficiency).FailedPayment.Amount;

            var totalPayment = PaymentGroup.CalculateTotalPayment();

            return totalPayment - failedPaymentAmount;
        }

    }



}
