using System;
using System.Collections.Generic;
using bar_prototype.Model;

namespace bar_prototype.Repository
{
    public class PaymentInstructionRepository
    {
        static readonly List<PaymentInstruction> items = new List<PaymentInstruction>();
        public PaymentInstructionRepository()
        {
           
        }

        public List<PaymentInstruction> GetAll()
        {
            return items;
        }

        public void Add (PaymentInstruction item)
        {
            items.Add(item);
        }

        public PaymentInstruction Get(int id)
        {
            return items.Find(x => x.Id == id);
        }

        public PaymentInstruction Get(int id , StatusType status)
        {
            return items.Find(x => x.Id == id && x.Status == status);
        }
    }
}
