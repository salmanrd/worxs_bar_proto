namespace bar_prototype.Model
{
    public class Cheque : PaymentInstruction
    {
        public override string PaymentMethod { get { return "Cheque"; } }
    }



}
