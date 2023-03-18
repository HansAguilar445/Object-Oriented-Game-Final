using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal static class WeaponData
    {
        public static Dictionary<string, Weapon> Weapons { get; private set; } = new Dictionary<string, Weapon>();
        public static AttackAction Attack = new AttackAction("Attack", 0, 100);
        public static GuardAction Guard = new GuardAction();

        public static Dictionary<Weapon.WeaponType, List<CombatAction>> Actions { get; } = new()
        {
            {
                Weapon.WeaponType.Sword, new List<CombatAction>
                {
                    new AttackAction("Cutting Edge", 3, 150),
                    new AttackAction("Gale Blade", 5, 200),
                    new BuffAction("Offensive Stance", 10, StatusEffectData.Buffs["Attack Up"]),
                    new AttackAction("Shatterpoint", 5, 175, StatusEffectData.Debuffs["Defence Down"]),
                    new BuffAction("Defensive Stance", 10, StatusEffectData.Buffs["Defence Up"]),
                    new AttackAction("Swallow Reversal", 7, 440)
                }
            },
            {
                Weapon.WeaponType.Axe, new List<CombatAction>
                {
                    new AttackAction("Smashdown", 5, 300),
                    new AttackAction("Strong Swing", 7, 400, StatusEffectData.Buffs["Attack Up"]),
                    new BuffAction("Riastrad", 10, StatusEffectData.Buffs["Supercharge"]),
                    new AttackAction("Wild Abandon", 5, 300),
                    new AttackAction("Brutalize", 7, 450),
                    new AttackAction("Triumphant Axe", 12, 600)
                }
            },
            {
                Weapon.WeaponType.Polearm, new List<CombatAction>
                {
                    new AttackAction("Rapid Twirl", 3, 170),
                    new AttackAction("Battering Thrust", 5, 200),
                    new BuffAction("Focus", 9, StatusEffectData.Buffs["Accuracy Up"]),
                    new AttackAction("Breach Strike", 5, 175),
                    new BuffAction("Grandstand", 9, StatusEffectData.Buffs["Defence Up"]),
                    new AttackAction("Wolf's Head", 7, 400)
                }
            },
            {
                Weapon.WeaponType.Drone, new List<CombatAction>
                {
                    new AttackAction("Spinning Blades", 3, 150),
                    new AttackAction("Ramming Speed", 4, 175),
                    new BuffAction("Recalibrate", 6, StatusEffectData.Buffs["Accuracy Up"]),
                    new AttackAction("Diversionary Tactic", 5, 175, StatusEffectData.Buffs["Agility Up"]),
                    new BuffAction("Precision Scan", 7, StatusEffectData.Buffs["Crit Rate Up"]),
                    new AttackAction("Sacrifice", 8, 440)
                }
            },
            {
                Weapon.WeaponType.Knife, new List<CombatAction>
                {
                    new AttackAction("Trickster's Dance", 3, 170, StatusEffectData.Buffs["Agility Up"]),
                    new AttackAction("Killing Doll", 3, 200),
                    new BuffAction("Presence Concealment", 9, StatusEffectData.Buffs["Agility Up"]),
                    new AttackAction("Blood Marker", 3, 175, StatusEffectData.Debuffs["Agility Down"]),
                    new BuffAction("Death Perception", 9, StatusEffectData.Buffs["Crit Rate Up"]),
                    new AttackAction("From Hell", 15, 900)
                }
            },
            {
                Weapon.WeaponType.Rifle, new List<CombatAction>
                {
                    new AttackAction("Bayonet Charge", 3, 120),
                    new AttackAction("Blinding Bullet", 5, 150, StatusEffectData.Debuffs["Accuracy Down"]),
                    new BuffAction("Red Dot", 5, StatusEffectData.Buffs["Accuracy Up"]),
                    new AttackAction("Lock and Load", 5, 200, StatusEffectData.Buffs["Supercharge"]),
                    new AttackAction("Bullet Barrage", 7, 300),
                    new AttackAction("Head Shot", 7, 440)
                }
            }
        };

        static WeaponData()
        {
            HashSet<string> weaponTypes = new HashSet<string>(Enum.GetNames(typeof(Weapon.WeaponType)));
            StreamReader reader = new StreamReader("../../../Equipment/Weapons.txt");
            string weaponType = "";

            List<string> statNames = new List<string>
            {
                "HP",
                "Attack Power",
                "Defence",
                "Agility",
                "Dexterity",
                "Critical Rate"
            };

            do
            {
                string value = reader.ReadLine();
                if (weaponTypes.Contains(value))
                {
                    weaponType = value;
                }
                else
                {
                    string name = value;
                    string description = reader.ReadLine();
                    int level = int.Parse(reader.ReadLine());
                    int buyPrice = int.Parse(reader.ReadLine());
                    int sellPrice = int.Parse(reader.ReadLine());
                    Dictionary<string, int> modifiers = new Dictionary<string, int>();

                    foreach (string stat in statNames)
                    {
                        modifiers.Add(stat, int.Parse(reader.ReadLine()));
                    }

                    Weapon.WeaponType type = (Weapon.WeaponType)Enum.Parse(typeof(Weapon.WeaponType), weaponType);

                    Weapon weapon = new Weapon(name, description, level, buyPrice, sellPrice, modifiers, type);

                    Weapons.Add(name, weapon);
                }

            } while (!reader.EndOfStream);

        }

        public static Dictionary<string, Weapon> GetWeaponsOfType(Weapon.WeaponType type)
        {
            Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();

            foreach ((string name, Weapon weapon) in Weapons)
            {
                if (weapon.Type == type)
                {
                    weapons.Add(name, weapon);
                }
            }

            return weapons;
        }
    }
}
