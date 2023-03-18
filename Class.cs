using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Class
    {
        public string Name { get; }
        public string Description { get; }
        public Dictionary<string, int> Bases { get; }
        public Armour.ArmourType ArmourType { get; }
        public Weapon.WeaponType WeaponType { get; }

        public Class(string name, string description, Dictionary<string, int> bases, Armour.ArmourType armourType, Weapon.WeaponType weaponType)
        {
            Name = name;
            Description = description;
            Bases = bases;
            ArmourType = armourType;
            WeaponType = weaponType;
        }
    }
}
