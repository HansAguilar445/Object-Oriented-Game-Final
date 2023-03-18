using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal static class ConsumableData
    {
        public static Dictionary<string, Consumable> Consumables { get; set; }

        static ConsumableData()
        {
            Consumables = new Dictionary<string, Consumable>
            {
                {
                    "Vulnerary",
                    new HealingConsumable("Vulnerary", "A simple healing potion to keep you alive.", 500, 250, 100)
                },
                {
                    "Berserker Button",
                    new BuffConsumable("Berserker Button", "A drink that fills you with power. Fueled by whatever you " +
                    "hate most.", 1500, 750, StatusEffectData.Buffs["Supercharge"])
                },
                {
                    "Dr. Armstrong",
                    new BuffConsumable("Dr. Armstrong", "A can of soda that allows you to take more of a beating. May or may not " +
                    "contain nanomachines.", 1500, 750, StatusEffectData.Buffs["Defence Up"])
                },
                {
                    "Spinach",
                    new BuffConsumable("Spinach", "A can of spinach that grants you a boost in strength. Sailors love it.",
                    1500, 750, StatusEffectData.Buffs["Attack Up"])
                },
                {
                    "NRG",
                    new BuffConsumable("NRG", "A mysterious liquid medicine that seems to slow time around you. Do not take more than the " +
                    "recommended dosage.", 1500, 750, StatusEffectData.Buffs["Agility Up"])
                }
            };
        }
    }
}
