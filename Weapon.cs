using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Weapon : IEquipment
    {
        public enum WeaponType
        {
            Sword,
            Axe,
            Polearm,
            Drone,
            Knife,
            Rifle
        }

        public string Name { get; }
        public string Description { get; }
        public int LevelRequirement { get; }
        public int BuyPrice { get; }
        public int SellPrice { get; }

        public Dictionary<string, int> Modifiers { get; }
        public WeaponType Type { get; }

        public Weapon(string name, string description, int levelRequirement, int buyPrice,
            int sellPrice, Dictionary<string, int> modifiers, WeaponType type)
        {
            Name = name;
            Description = description;
            LevelRequirement = levelRequirement;
            BuyPrice = buyPrice;
            SellPrice = sellPrice;
            Modifiers = modifiers;
            Type = type;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Name);
            builder.AppendLine(Type.ToString());
            builder.AppendLine(Description);
            builder.AppendLine($"Level requirement: {LevelRequirement}");

            foreach ((string stat, int mod) in Modifiers)
            {
                if (mod != 0)
                {
                    builder.AppendLine($"{stat}: {mod}");
                }
            }

            return builder.ToString();
        }
    }
}
