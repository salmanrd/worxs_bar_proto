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

        public int GetIdMax10()
        {
            return random.Next(10);
        }

        public int GetIdMax100()
        {
            return random.Next(100);
        }
    }
}
