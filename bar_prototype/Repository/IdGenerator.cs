using System;
namespace bar_prototype.Repository
{
    public class IdGenerator
    {
        static Random random = new Random();

        public int GetId()
        {
            return random.Next(9999);
        }
    }
}
