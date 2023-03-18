using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class ClassData
    {
        private string _path { get; }
        private List<string> _statNames { get; }
        public Dictionary<string, Class> Classes { get; } = new Dictionary<string, Class>();

        public ClassData(string path, List<string> statNames)
        {
            _path = path;
            _statNames = statNames;

            GetClasses();
        }

        public void GetClasses()
        {
            StreamReader stream = new StreamReader(_path);

            do
            {
                string name = stream.ReadLine();
                string description = stream.ReadLine();
                Dictionary<string, int> stats = new Dictionary<string, int>();

                foreach (string stat in _statNames)
                {
                    string statVal = stream.ReadLine();
                    stats.Add(stat, int.Parse(statVal));
                }

                string weaponVal = stream.ReadLine();
                string armourVal = stream.ReadLine();

                Weapon.WeaponType weaponType = (Weapon.WeaponType)Enum.Parse(typeof(Weapon.WeaponType), weaponVal);
                Armour.ArmourType armourType = (Armour.ArmourType)Enum.Parse(typeof(Armour.ArmourType), armourVal);

                Class pClass = new Class(name, description, stats, armourType, weaponType);
                Classes.Add(name, pClass);

            } while (!stream.EndOfStream);
        }
    }
}
