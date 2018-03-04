namespace bar_prototype.Model
{
    public abstract class PaymentInstruction
    {
        public abstract string PaymentMethod { get; }
        public double Amount { get; set; }
        public int Id { get; set; }
        public StatusType Status { get; set; }


        public bool TransferredtoBar { get; set; }
        public bool SuspenseDeficiency { get; set; }

    }



}
