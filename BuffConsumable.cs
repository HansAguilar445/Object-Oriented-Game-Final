using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class BuffConsumable : Consumable
    {
        public StatusEffect Effect { get; }

        public BuffConsumable(string name, string description, int buyPrice, int sellPrice, StatusEffect effect)
            : base(name, description, buyPrice, sellPrice)
        {
            Effect = effect;
        }

        public override void ActivateEffect(Hero hero)
        {
            hero.Effects.Add(Effect);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Name);
            builder.AppendLine(Description);
            builder.AppendLine($"Applies {Effect.Name} on use.");

            return builder.ToString();
        }
    }
}
