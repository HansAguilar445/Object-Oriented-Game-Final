using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal static class EnemyData
    {
        public static Dictionary<string, Enemy> Enemies { get; } = new Dictionary<string, Enemy>();
        public static Dictionary<string, List<CombatAction>> EnemyActions { get; }

        static EnemyData()
        {
            StreamReader reader = new StreamReader("../../../Enemies.txt");

            List<string> statNames = new List<string>
            {
                "HP",
                "Attack Power",
                "Defence",
                "Agility",
                "Dexterity",
                "Critical Rate"
            };

            EnemyActions = new Dictionary<string, List<CombatAction>>
            {
                {
                    "Machine", new List<CombatAction>
                    {
                        new AttackAction("Servomotor Swipe", 0, 100),
                        new GuardAction(),
                        new AttackAction("Laser Blast", 3, 150),
                        new BuffAction("Overclock", 7, StatusEffectData.Buffs["Attack Up"])
                    }
                },
                {
                    "Fae", new List<CombatAction>
                    {
                        new AttackAction("Spirit Slash", 0, 100),
                        new GuardAction(),
                        new AttackAction("Phantom Burst", 5, 200),
                        new BuffAction("Fae Sight", 9, StatusEffectData.Buffs["Accuracy Up"])
                    }
                },
                {
                    "Dragonkin", new List<CombatAction>
                    {
                        new AttackAction("Divebomb", 0, 100),
                        new GuardAction(),
                        new AttackAction("Roaring Assault", 5, 150, StatusEffectData.Buffs["Attack Up"]),
                        new BuffAction("Fangs Bared", 7, StatusEffectData.Buffs["Crit Rate Up"])
                    }
                },
                {
                    "Infantry", new List<CombatAction>
                    {
                        new AttackAction("Frontal Assault", 0, 100),
                        new GuardAction(),
                        new AttackAction("Rushdown", 5, 250),
                        new AttackAction("Forward March", 5, 100, StatusEffectData.Buffs["Supercharge"]),
                        new BuffAction("Rallying Cry", 7, StatusEffectData.Buffs["Attack Up"])
                    }
                },
                {
                    "Ninja", new List<CombatAction>
                    {
                        new AttackAction("Shuriken Swarm", 0, 100),
                        new AttackAction("Chain Spike Spin", 5, 200),
                        new AttackAction("Kunai Impaler", 3, 150, StatusEffectData.Debuffs["Defence Down"]),
                        new BuffAction("Smokescreen", 7, StatusEffectData.Buffs["Agility Up"])
                    }
                },
                {
                    "Bard", new List<CombatAction>
                    {
                        new AttackAction("Vicious Mockery", 0, 100),
                        new GuardAction(),
                        new AttackAction("Hideous Laughter", 5, 150, StatusEffectData.Debuffs["Accuracy Down"]),
                        new AttackAction("Silvery Barbs", 7, 300, StatusEffectData.Debuffs["Attack Down"]),
                        new BuffAction("Inspiration", 7, StatusEffectData.Buffs["Attack Up"])
                    }
                },
                {
                    "L", new List<CombatAction>
                    {
                        new AttackAction("Hairpin Curve Drift", 0, 100),
                        new AttackAction("100km Merge", 5, 200),
                        new AttackAction("Spinout", 5, 200, StatusEffectData.Buffs["Agility Up"]),
                        new BuffAction("Burnout", 7, StatusEffectData.Buffs["Supercharge"])
                    }
                },
                {
                    "Minecrafter", new List<CombatAction>
                    {
                        new AttackAction("Pickaxe Swing", 0, 100),
                        new GuardAction(),
                        new AttackAction("Flame Arrow", 5, 250, StatusEffectData.Debuffs["Defence Down"]),
                        new BuffAction("Structure Construction", 7, StatusEffectData.Buffs["Defence Up"])
                    }
                },
                {
                    "Z", new List<CombatAction>
                    {
                        new AttackAction("Keyboard Punishment", 0, 100),
                        new GuardAction(),
                        new AttackAction("Detonating Laptop", 7, 440),
                        new AttackAction("Peelout", 5, 300)
                    }
                },
                {
                    "B", new List<CombatAction>
                    {
                        new AttackAction("Heavy Footstep", 0, 100),
                        new GuardAction(),
                        new BuffAction("Maximum Density", 7, StatusEffectData.Buffs["Defence Up"])
                    }
                },
                {
                    "Crab", new List<CombatAction>
                    {
                        new AttackAction("Snip Snap", 0, 100),
                        new AttackAction("Carcinization", 5, 200, StatusEffectData.Buffs["Supercharge"]),
                        new BuffAction("Click Clack", 5, StatusEffectData.Buffs["Crit Rate Up"])
                    }
                },
                {
                    "Interface", new List<CombatAction>
                    {
                        new AttackAction("Blue Ripper", 0, 100),
                        new GuardAction(),
                        new AttackAction("Code Rewrite: Data Jurisdiction", 6, 200, StatusEffectData.Debuffs["Agility Down"]),
                        new BuffAction("Code Rewrite: Radical Lord", 7, StatusEffectData.Buffs["Supercharge"])
                    }
                },
                {
                    "M", new List<CombatAction>
                    {
                        new AttackAction("Meta Multislash", 0, 100),
                        new GuardAction(),
                        new AttackAction("Mach Tornado", 3, 150),
                        new AttackAction("Shuttle Loop", 3, 150, StatusEffectData.Buffs["Accuracy Up"]),
                        new AttackAction("Condor Dive", 7, 350)
                    }
                }
            };

            do
            {

                string name = reader.ReadLine();
                string type = reader.ReadLine();
                int level = int.Parse(reader.ReadLine());
                int experience = int.Parse(reader.ReadLine());
                Dictionary<string, int> Stats = new Dictionary<string, int>();

                foreach (string statName in statNames)
                {
                    Stats.Add(statName, int.Parse(reader.ReadLine()));
                }

                Enemy enemy = new Enemy(name, level, Stats, EnemyActions[type], experience, experience);
                Enemies.Add(name, enemy);
            } while (!reader.EndOfStream);

        }
    }
}
