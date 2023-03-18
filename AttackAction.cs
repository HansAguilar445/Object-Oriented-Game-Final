using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class AttackAction : CombatAction
    {
        private int _modifier { get; }
        private StatusEffect _effect { get; }

        public AttackAction(string name, int cooldown, int modifier) : base(name, cooldown)
        {
            _modifier = modifier;
        }

        public AttackAction(string name, int cooldown, int modifier, StatusEffect effect) : base(name, cooldown)
        {
            _modifier = modifier;
            _effect = effect;
        }

        public void PerformAction(Combatant user, Combatant target)
        {
            WriteLine($"{user.Name} attacked {target.Name} with {Name}.");
            Random random = new Random();
            int critBonus = 0;
            int crits = random.Next(0, 100);
            int hitChance = 100 + (user.Level - target.Level) + (user.Dexterity - target.Agility);
            int hit = random.Next(0, 100);

            if (hit < 5)
            {
                hit = 5;
            }

            if (crits <= user.Critical)
            {
                critBonus = (int)Math.Round((decimal)(user.AttackPower * 7 / 10));
            }

            if (hit <= hitChance)
            {
                decimal damage = user.AttackPower;
                damage += critBonus;
                damage *= _modifier / 100;
                damage -= target.Defence;

                if (damage > 0)
                {
                    WriteLine($"{user.Name} dealt {(int)damage} damage to {target.Name}.");
                    target.CurrentHealth -= (int)damage;
                }
                else
                {
                    WriteLine($"{user.Name} dealt no damage to {target.Name}");
                }

                if (_effect != null)
                {
                    if (StatusEffectData.Debuffs.ContainsKey(_effect.Name) && (random.Next(0, 10) == 1))
                    {
                        target.Effects.Add(_effect);
                        _effect.Effect(target);
                    }
                    else if (StatusEffectData.Buffs.ContainsKey(_effect.Name))
                    {
                        user.Effects.Add(_effect);
                        _effect.Effect(user);
                    }
                }
            }
            else
            {
                WriteLine($"{target.Name} dodged.");
            }

            base.PerformAction(user);
        }

        public override string GetSkillInfo()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Name);
            builder.AppendLine("Type: Attack");
            builder.AppendLine($"Damage modifier: {_modifier}%");

            if (_effect != null)
            {
                builder.AppendLine($"Additional effect: {_effect.Name}");
            }
            builder.AppendLine($"Cooldown: {Cooldown}");

            return builder.ToString();
        }
    }
}
