using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal interface IEquipment
    {
        public string Name { get; }
        public string Description { get; }
        public int LevelRequirement { get; }
        public int BuyPrice { get; }
        public int SellPrice { get; }
        public Dictionary<string, int> Modifiers { get; }
    }
}
