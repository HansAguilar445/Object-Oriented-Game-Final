using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class GuardAction : CombatAction
    {
        public GuardAction(string name = "Guard", int cooldown = 0) : base(name, cooldown)
        {

        }

        public override void PerformAction(Combatant combatant)
        {
            WriteLine($"{combatant.Name} guarded.");
            Action<Combatant> action = combatant => combatant.Defence += combatant.Stats["Defence"] / 4;
            Action<Combatant> reversal = combatant => combatant.Assign("Defence");
            StatusEffect effect = new StatusEffect("Guard", 1, action, reversal);
            combatant.Effects.Add(effect);
            effect.Effect(combatant);
        }
    }
}
