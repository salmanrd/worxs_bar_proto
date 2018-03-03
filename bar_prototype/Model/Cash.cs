namespace bar_prototype.Model
{
    public class Cash : PaymentInstruction
    {
        public override string PaymentMethod { get { return "Cash"; } }
    }



}
