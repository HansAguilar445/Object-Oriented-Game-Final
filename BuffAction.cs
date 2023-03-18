using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class BuffAction : CombatAction
    {
        private StatusEffect _effect { get; }
        public BuffAction(string name, int cooldown, StatusEffect effect) : base(name, cooldown)
        {
            _effect = effect;
        }

        public override void PerformAction(Combatant combatant)
        {
            WriteLine($"{combatant.Name} used {Name}.");
            _effect.Effect(combatant);
            combatant.Effects.Add(_effect);
            base.PerformAction(combatant);
        }

        public override string GetSkillInfo()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Name);
            builder.AppendLine("Type: Buff");
            builder.AppendLine($"Effect: {_effect.Name}");
            builder.AppendLine($"Cooldown: {Cooldown}");
            return builder.ToString();
        }
    }
}
