using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Enemy : Combatant
    {
        public List<string> Drops { get; private set; }
        public int ExperienceDropped { get; }
        public int GoldDropped { get; }
        public Hero Target { get; set; }
        private EnemyID _id { get; set; }

        public Enemy(string name, int level, Dictionary<string, int> stats, List<CombatAction> actions,
            int experience, int gold) : base(name, level, stats)
        {
            //Drops = drops;
            ExperienceDropped = experience;
            GoldDropped = gold;

            _id = new EnemyID(level);

            foreach (CombatAction action in actions)
            {
                Actions.Add(action.Name, action);
            }
        }

        public Enemy(string name, int level, Dictionary<string, int> stats, Dictionary<string, CombatAction> actions,
            int experience, int gold) : base(name, level, stats, actions)
        {
            //Drops = drops;
            ExperienceDropped = experience;
            GoldDropped = gold;

            _id = new EnemyID(level);
        }

        public Enemy Clone()
        {
            Enemy clone = new Enemy(Name, Level, Stats, Actions, ExperienceDropped, GoldDropped);
            return clone;
        }

        public override void TakeAction()
        {
            Random random = new Random();
            int act = random.Next(0, Actions.Count);
            bool validAction = true;

            do
            {
                CombatAction action = Actions.ElementAt(act).Value;

                if (action.CooldownTimer == 0)
                {
                    if (action is AttackAction)
                    {
                        AttackAction attack = (AttackAction)action;
                        attack.PerformAction(this, Target);
                    }
                    else
                    {
                        action.PerformAction(this);
                    }
                    validAction = true;
                }
                else
                {
                    act = random.Next(0, Actions.Count);
                    validAction = false;
                }
            } while (!validAction);

        }

        public string GetAllInfo()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Name);
            builder.AppendLine($"Level: {Level}");
            builder.AppendLine($"HP: {CurrentHealth}/{Stats["HP"]}");
            builder.AppendLine($"Attack Power: {AttackPower}");
            builder.AppendLine($"Defence: {Defence}");
            builder.AppendLine($"Agility: {Agility}");
            builder.AppendLine($"Dexterity: {Dexterity}");
            builder.AppendLine($"Critical Hit Rate: {Critical}%");

            foreach (StatusEffect effect in Effects)
            {
                builder.AppendLine($"{effect.Name}");
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Name);
            builder.AppendLine($"Level: {Level}");
            builder.AppendLine($"HP: {CurrentHealth}/{Stats["HP"]}");

            if (Effects.Count > 0)
            {
                builder.AppendLine("Active effects:");
                foreach (StatusEffect effect in Effects)
                {
                    builder.AppendLine($"{effect.Name}");
                }
            }

            builder.AppendLine();

            return builder.ToString();
        }
    }

    public class EnemyID
    {
        public int ID { get; }

        public EnemyID(int iD)
        {
            ID = iD;
        }
    }
}
