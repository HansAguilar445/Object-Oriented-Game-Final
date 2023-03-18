using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal abstract class CombatAction
    {
        public string Name { get; }
        public int Cooldown { get; }
        public int CooldownTimer { get; set; }

        public CombatAction(string name, int cooldown)
        {
            Name = name;
            Cooldown = cooldown;
            CooldownTimer = 0;
        }

        public virtual void PerformAction(Combatant combatant)
        {
            CooldownTimer = Cooldown;
        }

        public override string ToString()
        {
            string timeToUse = CooldownTimer == 0 ? "" : $" - {CooldownTimer} turns remaining";
            return $"{Name}{timeToUse}";
        }

        public virtual string GetSkillInfo()
        {
            return ToString();
        }
    }
}
