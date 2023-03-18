using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal static class ArmourData
    {

        public static Dictionary<string, Armour> Armours { get; set; } = new Dictionary<string, Armour>();

        static ArmourData()
        {
            HashSet<string> armourTypes = new HashSet<string>(Enum.GetNames(typeof(Armour.ArmourType)));
            StreamReader reader = new StreamReader("../../../Equipment/Armour.txt");
            string armourType = "";

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

                if (armourTypes.Contains(value))
                {
                    armourType = value;
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
                        string statVal = reader.ReadLine();
                        modifiers.Add(stat, int.Parse(statVal));
                    }

                    Armour.ArmourType type = (Armour.ArmourType)Enum.Parse(typeof(Armour.ArmourType), armourType);

                    Armour armour = new Armour(name, description, level, buyPrice, sellPrice, modifiers, type);
                    Armours.Add(name, armour);
                }
            } while (!reader.EndOfStream);

        }

        public static Dictionary<string, Armour> GetArmoursOfType(Armour.ArmourType type)
        {
            Dictionary<string, Armour> armours = new Dictionary<string, Armour>();

            foreach ((string name, Armour armour) in Armours)
            {
                if (armour.Type == type)
                {
                    armours.Add(name, armour);
                }
            }

            return armours;
        }
    }

}
