using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static FinalProject.Armour;
using static FinalProject.Weapon;

namespace FinalProject
{
    internal static class Shop
    {
        private static Dictionary<string, Weapon> _weapons { get; set; } = new();
        private static Dictionary<string, Armour> _armour { get; set; } = new();
        private static Dictionary<string, Consumable> _consumables { get; set; }

        static Shop()
        {
            _consumables = ConsumableData.Consumables;
            _weapons = WeaponData.Weapons;
            _armour = ArmourData.Armours;
        }

        public static void Select(Hero hero)
        {
            string dialogue = "Xord: Are you buying? Or selling?";
            Menu menu = new Menu(new List<string> { "Buying", "Selling" });
            string choice = menu.RunMenu(dialogue);

            if (choice == "Buying")
            {
                BrowseShop(hero);
            }
            else
            {
                BrowseInventory(hero);
            }
        }

        private static void BrowseShop(Hero hero)
        {
            bool isLooping = true;
            string dialogue = "Xord: What can I get you?";
            List<string> options = new List<string>
            {
                "Weapons",
                "Armour",
                "Consumables",
                "Back"
            };
            Menu menu = new Menu(options);

            do
            {
                string finances = $"";
                string category = menu.RunMenu(dialogue);
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("Xord: So, what'll it be?");
                switch (category)
                {
                    case "Weapons":
                        BrowseWeapons(hero);
                        break;
                    case "Armour":
                        BrowseArmour(hero);
                        break;
                    case "Consumables":
                        BrowseConsumables(hero);
                        break;
                    case "Back":
                        isLooping = false;
                        break;
                }
            } while (isLooping);
        }

        private static void BrowseWeapons(Hero hero)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> weaponTypeOptions = new List<string>(Enum.GetNames(typeof(Weapon.WeaponType)));
            weaponTypeOptions.Insert(0, "Back");

            Menu wares = new Menu(weaponTypeOptions);

            do
            {
                initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                string choice = wares.RunMenu(initialMessage);

                if (weaponTypeOptions.IndexOf(choice) != 0)
                {
                    Weapon.WeaponType type = (Weapon.WeaponType)Enum.Parse(typeof(Weapon.WeaponType), choice);
                    BrowseWeaponsOfType(hero, type);
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private static void BrowseWeaponsOfType(Hero hero, Weapon.WeaponType type)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> weaponOptions = new List<string>(WeaponData.GetWeaponsOfType(type).Keys);
            weaponOptions.Add("Back");

            StringBuilder builder = new StringBuilder();
            Menu wares = new Menu(weaponOptions);
            builder.AppendLine(initialMessage);

            do
            {
                initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                string choice = wares.RunMenu(builder.ToString());

                if (_weapons.ContainsKey(choice))
                {
                    if (_weapons[choice].BuyPrice > hero.Gold)
                    {
                        builder.Clear();
                        builder.AppendLine(initialMessage);
                        builder.AppendLine("Xord: Too rich for your blood. Come back for it another time.");
                    }
                    else
                    {
                        builder.Clear();
                        Purchase(hero, _weapons[choice]);

                        initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                        builder.AppendLine(initialMessage);
                    }
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private static void BrowseArmour(Hero hero)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> armourOptions = new List<string>(Enum.GetNames(typeof(Armour.ArmourType)));
            armourOptions.Insert(0, "Back");

            Menu wares = new Menu(armourOptions);

            do
            {
                initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                string choice = wares.RunMenu(initialMessage);

                if (armourOptions.IndexOf(choice) != 0)
                {
                    Armour.ArmourType type = (Armour.ArmourType)Enum.Parse(typeof(Armour.ArmourType), choice);
                    BrowseArmourOfType(hero, type);
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private static void BrowseArmourOfType(Hero hero, Armour.ArmourType type)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> armourOptions = new List<string>(ArmourData.GetArmoursOfType(type).Keys);
            armourOptions.Insert(0, "Back");

            StringBuilder builder = new StringBuilder();
            builder.AppendLine(initialMessage);

            Menu wares = new Menu(armourOptions);

            do
            {
                string choice = wares.RunMenu(builder.ToString());

                if (_armour.ContainsKey(choice))
                {
                    if (_armour[choice].BuyPrice > hero.Gold)
                    {
                        builder.Clear();
                        builder.AppendLine(initialMessage);
                        builder.AppendLine("Xord: Too rich for your blood. Come back for it another time.");
                    }
                    else
                    {
                        builder.Clear();
                        Purchase(hero, _armour[choice]);

                        initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                        builder.AppendLine(initialMessage);
                    }
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }


        private static void BrowseConsumables(Hero hero)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> consumableOptions = new List<string>(_consumables.Keys);
            consumableOptions.Insert(0, "Back");

            StringBuilder builder = new StringBuilder();
            builder.AppendLine(initialMessage);

            Menu wares = new Menu(consumableOptions);

            do
            {
                initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                string choice = wares.RunMenu(builder.ToString());

                if (_consumables.ContainsKey(choice))
                {
                    if (_consumables[choice].BuyPrice > hero.Gold)
                    {
                        builder.Clear();
                        builder.AppendLine(initialMessage);
                        builder.AppendLine("Xord: Too rich for your blood. Come back for it another time.");
                    }
                    else
                    {
                        builder.Clear();
                        Purchase(hero, _consumables[choice]);

                        initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                        builder.AppendLine(initialMessage);
                    }
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private static bool Purchase(Hero hero, Weapon weapon)
        {
            Menu confirm = new Menu(new List<string> { "Yes", "No" });
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(weapon.ToString());
            builder.AppendLine($"Price: {weapon.BuyPrice}G");
            builder.AppendLine("Will you buy it?");

            string choice = confirm.RunMenu(builder.ToString());

            if (choice == "Yes")
            {
                hero.ObtainItem(weapon);
                hero.Gold -= weapon.BuyPrice;
                return true;
            }

            return false;
        }

        private static bool Purchase(Hero hero, Armour armour)
        {
            Menu confirm = new Menu(new List<string> { "Yes", "No" });
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(armour.ToString());
            builder.AppendLine($"Price: {armour.BuyPrice}G");
            builder.AppendLine("Will you buy it?");

            string choice = confirm.RunMenu(builder.ToString());

            if (choice == "Yes")
            {
                hero.ObtainItem(armour);
                hero.Gold -= armour.BuyPrice;
                return true;
            }

            return false;
        }

        private static bool Purchase(Hero hero, Consumable consumable)
        {
            Menu confirm = new Menu(new List<string> { "Yes", "No" });
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(consumable.ToString());
            builder.AppendLine($"Price: {consumable.BuyPrice}G");
            builder.AppendLine("Will you buy it?");

            string choice = confirm.RunMenu(builder.ToString());

            if (choice == "Yes")
            {
                hero.ObtainItem(consumable);
                hero.Gold -= consumable.BuyPrice;
                return true;
            }

            return false;
        }

        public static void BrowseInventory(Hero hero)
        {
            bool isLooping = true;
            string dialogue = "Xord: What are you selling?";
            List<string> options = new List<string>
            {
                "Weapons",
                "Armour",
                "Consumables",
                "Back"
            };
            Menu menu = new Menu(options);

            do
            {
                string finances = $"{hero.Name}'s Funds: {hero.Gold}G";
                string category = menu.RunMenu(dialogue);
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("Xord: So, what'll it be?");
                builder.AppendLine(finances);

                switch (category)
                {
                    case "Weapons":
                        BrowseOwnWeapons(hero);
                        break;
                    case "Armour":
                        BrowseOwnArmour(hero);
                        break;
                    case "Consumables":
                        if (hero.ConsumableInventory.Count > 0)
                        {
                            BrowseOwnConsumables(hero);
                        }
                        break;
                    case "Back":
                        isLooping = false;
                        break;
                }
            } while (isLooping);
        }

        private static void BrowseOwnWeapons(Hero hero)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> weaponTypeOptions = new List<string>(Enum.GetNames(typeof(Weapon.WeaponType)));
            weaponTypeOptions.Insert(0, "Back");

            Menu wares = new Menu(weaponTypeOptions);

            do
            {
                string choice = wares.RunMenu(initialMessage);

                if (weaponTypeOptions.IndexOf(choice) != 0)
                {
                    Weapon.WeaponType type = (Weapon.WeaponType)Enum.Parse(typeof(Weapon.WeaponType), choice);
                    BrowseOwnWeaponsOfType(hero, type);
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private static void BrowseOwnWeaponsOfType(Hero hero, Weapon.WeaponType type)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> weaponOptions = new List<string>();
            weaponOptions.Add("Back");

            foreach (Weapon weapon in hero.GetWeaponsOfType(type))
            {
                weaponOptions.Add(weapon.Name);
            }

            StringBuilder builder = new StringBuilder();
            Menu wares = new Menu(weaponOptions);
            builder.AppendLine(initialMessage);

            do
            {
                initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                string choice = wares.RunMenu(builder.ToString());

                if (weaponOptions.IndexOf(choice) != 0)
                {
                    if ((hero.Weapon == _weapons[choice]) &&
                        (hero.WeaponInventory.IndexOf(_weapons[choice]) ==
                        hero.WeaponInventory.LastIndexOf(_weapons[choice])))
                    {
                        builder.Clear();
                        builder.AppendLine(initialMessage);
                        builder.AppendLine("Xord: Don't sell that weapon. You need it.");
                    }
                    else
                    {
                        if (Sell(hero, _weapons[choice]))
                        {
                            weaponOptions.Remove(choice);
                        }

                        builder.Clear();
                        initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                        builder.AppendLine(initialMessage);
                    }
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private static void BrowseOwnArmour(Hero hero)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> armourOptions = new List<string>(Enum.GetNames(typeof(Armour.ArmourType)));
            armourOptions.Insert(0, "Back");

            Menu wares = new Menu(armourOptions);

            do
            {
                initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                string choice = wares.RunMenu(initialMessage);

                if (armourOptions.IndexOf(choice) != 0)
                {
                    Armour.ArmourType type = (Armour.ArmourType)Enum.Parse(typeof(Armour.ArmourType), choice);
                    BrowseOwnArmourOfType(hero, type);
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private static void BrowseOwnArmourOfType(Hero hero, Armour.ArmourType type)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> armourOptions = new List<string>();
            armourOptions.Add("Back");

            foreach (Armour weapon in hero.GetArmourOfType(type))
            {
                armourOptions.Add(weapon.Name);
            }

            StringBuilder builder = new StringBuilder();
            Menu wares = new Menu(armourOptions);

            do
            {
                initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                string choice = wares.RunMenu(builder.ToString());

                if (armourOptions.IndexOf(choice) != 0)
                {
                    if ((hero.Armour == _armour[choice]) &&
                        (hero.ArmourInventory.IndexOf(_armour[choice]) ==
                        hero.ArmourInventory.LastIndexOf(_armour[choice])))
                    {
                        builder.Clear();
                        builder.AppendLine(initialMessage);
                        builder.AppendLine("Xord: Don't sell the clothes off your back. You need those.");
                    }
                    else
                    {
                        if (Sell(hero, _armour[choice]))
                        {
                            armourOptions.Remove(choice);
                        }

                        builder.Clear();
                        initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                        builder.AppendLine(initialMessage);
                    }
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private static void BrowseOwnConsumables(Hero hero)
        {
            bool isRepeating = true;

            string initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
            List<string> consumableOptions = new List<string>();
            consumableOptions.Add("Back");

            foreach (Consumable consumable in hero.ConsumableInventory)
            {
                consumableOptions.Add(consumable.Name);
            }

            StringBuilder builder = new StringBuilder();
            Menu wares = new Menu(consumableOptions);

            builder.AppendLine(initialMessage);

            do
            {
                initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                string choice = wares.RunMenu(builder.ToString());

                if (consumableOptions.IndexOf(choice) != 0)
                {
                    if (Sell(hero, _consumables[choice]))
                    {
                        consumableOptions.Remove(choice);
                    }

                    builder.Clear();
                    initialMessage = $"Xord: So, what'll it be?\n{hero.Name}'s Funds: {hero.Gold}G";
                    builder.AppendLine(initialMessage);
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private static bool Sell(Hero hero, Weapon weapon)
        {
            Menu confirm = new Menu(new List<string> { "Yes", "No" });
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(weapon.ToString());
            builder.AppendLine($"Price: {weapon.SellPrice}G");
            builder.AppendLine("Will you sell it?");

            string choice = confirm.RunMenu(builder.ToString());

            if (choice == "Yes")
            {
                hero.Sell(weapon);
                return true;
            }

            return false;
        }

        private static bool Sell(Hero hero, Armour armour)
        {
            Menu confirm = new Menu(new List<string> { "Yes", "No" });
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(armour.ToString());
            builder.AppendLine($"Price: {armour.SellPrice}G");
            builder.AppendLine("Will you sell it?");

            string choice = confirm.RunMenu(builder.ToString());

            if (choice == "Yes")
            {
                hero.Sell(armour);
                return true;
            }

            return false;
        }

        private static bool Sell(Hero hero, Consumable consumable)
        {
            Menu confirm = new Menu(new List<string> { "Yes", "No" });
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(consumable.ToString());
            builder.AppendLine($"Price: {consumable.SellPrice}G");
            builder.AppendLine("Will you sell it?");

            string choice = confirm.RunMenu(builder.ToString());

            if (choice == "Yes")
            {
                hero.Sell(consumable);
                return true;
            }

            return false;
        }

    }
}
