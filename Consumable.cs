using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Consumable
    {
        public string Name { get; }
        public string Description { get; }
        public int BuyPrice { get; }
        public int SellPrice { get; }

        public Consumable(string name, string description, int buyPrice, int sellPrice)
        {
            Name = name;
            Description = description;
            BuyPrice = buyPrice;
            SellPrice = sellPrice;
        }

        public virtual void ActivateEffect(Hero hero)
        {

        }
    }
}
