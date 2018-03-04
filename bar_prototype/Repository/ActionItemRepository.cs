using System;
using System.Collections.Generic;
using bar_prototype.Model;

namespace bar_prototype.Repository
{
    public class ActionItemRepository
    {
        static readonly List<ActionItem> items = new List<ActionItem>();

        public ActionItemRepository()
        {
        }

        public List<ActionItem> GetAll()
        {
            return items;
        }

        public void Add(ActionItem item)
        {
            items.Add(item);
        }

        public void Remove(ActionItem item)
        {
            items.Remove(item);
        }

        public ActionItem Get(int id)
        {
            return items.Find(x => x.Id == id);
        }
    }
}
