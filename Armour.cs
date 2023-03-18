using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Armour : IEquipment
    {
        public enum ArmourType
        {
            Light,
            Medium,
            Heavy
        }

        public string Name { get; }
        public string Description { get; }
        public int LevelRequirement { get; }
        public int BuyPrice { get; }
        public int SellPrice { get; }
        public Dictionary<string, int> Modifiers { get; }
        public ArmourType Type { get; }

        public Armour(string name, string description, int levelRequirement,
            int buyPrice, int sellPrice, Dictionary<string, int> modifiers, ArmourType type)
        {
            Name = name;
            Description = description;
            LevelRequirement = levelRequirement;
            Modifiers = modifiers;
            Type = type;
            BuyPrice = buyPrice;
            SellPrice = sellPrice;
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
