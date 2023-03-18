using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class StatusEffect
    {
        public int Duration { get; }
        public int Timer { get; set; }
        public Action<Combatant> Effect { get; }
        public Action<Combatant> Reversal { get; }
        public string Name { get; }

        public StatusEffect(string name, int duration, Action<Combatant> effect, Action<Combatant> reversal)
        {
            Duration = duration;
            Timer = 0;
            Effect = effect;
            Reversal = reversal;
            Name = name;
        }
    }
}
