namespace bar_prototype.Model
{
    public class SuspenseDeficiency : ActionItem
    {
        public override string ActionType { get { return "SuspenseDeficiency"; } }

        public PaymentInstruction FailedPayment { get; set; }
    }



}
