using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class HealingConsumable : Consumable
    {
        private int _strength { get; }

        public HealingConsumable(string name, string description, int buyPrice, int sellPrice, int strength) : base(name, description, buyPrice, sellPrice)
        {
            _strength = strength;
        }

        public override void ActivateEffect(Hero hero)
        {
            HealUser(hero);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Name);
            builder.AppendLine(Description);
            builder.AppendLine($"Recovers {_strength} on use.");

            return builder.ToString();
        }

        private void HealUser(Hero hero)
        {
            hero.RecoverHealth(_strength);
        }

    }
}
