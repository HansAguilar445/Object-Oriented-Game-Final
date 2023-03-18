using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal static class StatusEffectData
    {
        public static Dictionary<string, StatusEffect> Buffs { get; }
        public static Dictionary<string, StatusEffect> Debuffs { get; }

        static StatusEffectData()
        {
            int defaultDuration = 4;
            Action<Combatant> attackUp = combatant => combatant.AttackPower += combatant.AttackPower / 2;
            Action<Combatant> attackDown = combatant => combatant.AttackPower /= 2;
            Action<Combatant> superCharge = combatant => combatant.AttackPower *= 3;
            Action<Combatant> revertAttack = combatant => combatant.Assign("Attack Power");
            Action<Combatant> defenceUp = combatant => combatant.Defence += combatant.Defence / 2;
            Action<Combatant> defenceDown = combatant => combatant.Defence /= 2;
            Action<Combatant> revertDefence = combatant => combatant.Assign("Defence");
            Action<Combatant> agilityUp = combatant => combatant.Agility += combatant.Agility / 2;
            Action<Combatant> agilityDown = combatant => combatant.Agility /= 2;
            Action<Combatant> revertAgility = combatant => combatant.Assign("Agility");
            Action<Combatant> dexterityUp = combatant => combatant.Dexterity += combatant.Dexterity / 2;
            Action<Combatant> dexterityDown = combatant => combatant.Dexterity /= 2;
            Action<Combatant> revertDexterity = combatant => combatant.Assign("Dexterity");
            Action<Combatant> criticalUp = combatant => combatant.Critical += combatant.Critical / 2;
            Action<Combatant> criticalDown = combatant => combatant.Critical /= 2;
            Action<Combatant> revertCrit = combatant => combatant.Assign("Critical Rate");

            Buffs = new Dictionary<string, StatusEffect>
            {
                { "Attack Up", new StatusEffect("Attack Up", defaultDuration, attackUp, revertAttack) },
                { "Supercharge", new StatusEffect("Supercharge", 2, superCharge, revertAttack) },
                { "Defence Up", new StatusEffect("Defence Up", defaultDuration, defenceUp, revertDefence) },
                { "Agility Up", new StatusEffect("Agility Up", defaultDuration, agilityUp, revertAgility) },
                { "Accuracy Up", new StatusEffect("Accuracy Up", defaultDuration, dexterityUp, revertDexterity ) },
                { "Crit Rate Up", new StatusEffect("Crit Rate Up", defaultDuration, criticalUp, revertCrit) }
            };

            Debuffs = new Dictionary<string, StatusEffect>
            {
                { "Attack Down", new StatusEffect("Attack Down", defaultDuration, attackDown, revertAttack) },
                { "Defence Down", new StatusEffect("Defence Down", defaultDuration, defenceDown, revertDefence) },
                { "Agility Down", new StatusEffect("Agility Down", defaultDuration, agilityDown, revertAgility) },
                { "Accuracy Down", new StatusEffect("Accuracy Down", defaultDuration, dexterityDown, revertDexterity ) },
                { "Crit Rate Down", new StatusEffect("Crit Rate Down", defaultDuration, criticalDown, revertCrit) }
            };
        }

    }
}
