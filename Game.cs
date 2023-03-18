using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Game
    {
        private string _basePath = "../../../";
        private string _dialoguePath { get; }
        private string _userListPath { get; }
        private string _userPath { get; }
        private string _logo = @"
            .___________. __    __   _______         ___      .______       _______ .__   __.      ___      
            |           ||  |  |  | |   ____|       /   \     |   _  \     |   ____||  \ |  |     /   \     
            `---|  |----`|  |__|  | |  |__         /  ^  \    |  |_)  |    |  |__   |   \|  |    /  ^  \    
                |  |     |   __   | |   __|       /  /_\  \   |      /     |   __|  |  . `  |   /  /_\  \   
                |  |     |  |  |  | |  |____     /  _____  \  |  |\  \----.|  |____ |  |\   |  /  _____  \  
                |__|     |__|  |__| |_______|   /__/     \__\ | _| `._____||_______||__| \__| /__/     \__\ 
        ";

        private StringBuilder _backlog { get; set; } = new StringBuilder();
        private Hero _hero { get; set; }

        private Menu _classMenu { get; }
        private Menu _confirmMenu { get; }
        private ClassData _classData { get; }
        private Dictionary<string, Consumable> _consumables { get; set; } = new Dictionary<string, Consumable>();

        public Game()
        {
            string equipmentPath = $"{_basePath}Equipment/";

            List<string> statNames = new List<string>
            {
                "HP",
                "Attack Power",
                "Defence",
                "Agility",
                "Dexterity",
                "Critical Rate"
            };

            List<string> confirm = new List<string> { "Yes", "No" };

            _dialoguePath = $"{_basePath}LoginAnnouncement.txt";
            _userPath = $"{_basePath}Users/";
            _userListPath = $"{_userPath}Users.txt";
            _classData = new ClassData($"{_basePath}ClassInfo.txt", statNames);
            List<string> classes = new List<string>(_classData.Classes.Keys);
            _classMenu = new Menu(classes);
            _confirmMenu = new Menu(confirm);
        }

        public void Play()
        {
            StartUp();
            PlayGame();
        }

        private void StartUp()
        {
            StreamReader streamReader = new StreamReader(_dialoguePath);
            string name = "";

            WriteLine(_logo);
            WriteLine(streamReader.ReadLine());
            name = GetUserInput(streamReader.ReadLine());
            _backlog.AppendLine(_logo);

            if (!IsExistingPlayer(name))
            {

                using (StreamWriter streamWriter = new StreamWriter(_userListPath, true))
                {
                    streamWriter.WriteLine(name);
                }

                _backlog.AppendLine("Xord: So you are new! Well, welcome aboard! What's your class?");

                GenerateHero(name, 1, _classMenu.RunMenu(_backlog.ToString()));

                WriteLine("Xord: Got it. Here's your weapon and your armour. Just go right ahead. I " +
                    "recommend taking a look at your skills before going at it.");
                EquipWithBasicInventory();
                SaveToUserFile();
            }
            else
            {
                WriteLine("Xord: Ah, you're here! In the list! Go right ahead.");
                LoadUser(name);
            }

            WriteLine("Press any key to continue to the game.");
            ReadKey();

            _backlog.Clear();
            _backlog.AppendLine(_logo);
            _backlog.AppendLine(streamReader.ReadLine());
            _backlog.AppendLine(streamReader.ReadLine());

            WriteLine(_backlog.ToString());
            streamReader.Close();
        }

        private void PlayGame()
        {
            bool isLooping = true;

            List<string> options = new List<string>
            {
                "View player",
                "Use the shop",
                "Enter a fight",
                "Exit game"
            };

            Menu menu = new Menu(options);

            do
            {
                string choice = menu.RunMenu(_backlog.ToString());
                switch (choice)
                {
                    case "View player":
                        _hero.BrowseSelf();
                        break;
                    case "Use the shop":
                        Shop.Select(_hero);
                        break;
                    case "Enter a fight":
                        List<string> fights = new List<string>(BattleData.Battles.Keys);
                        fights.Add("Back");
                        Menu fightMenu = new Menu(fights);
                        string selectedFight = fightMenu.RunMenu("Select a fight");
                        if (BattleData.Battles.ContainsKey(selectedFight))
                        {
                            string info = BattleData.Battles[selectedFight].ToString();
                            string continueToFight = _confirmMenu.RunMenu($"{info}\nAre you sure?");
                            if (continueToFight == "Yes")
                            {
                                BattleData.Battles[selectedFight].Hero = _hero;
                                BattleData.Battles[selectedFight].Fight();
                            }
                        }
                        break;
                    case "Exit game":
                        isLooping = false;
                        WriteLine("Xord: Good work today!");
                        break;
                }
                SaveToUserFile();
            } while (isLooping);
        }

        private void GenerateHero(string name, int level, string playerClass)
        {
            Class pClass = _classData.Classes[playerClass];
            Hero hero = new Hero(name, level, pClass);
            _hero = hero;
        }

        private void EquipWithBasicInventory()
        {
            (_, Weapon weapon) = WeaponData.GetWeaponsOfType(_hero.Class.WeaponType).ElementAt(0);
            (_, Armour armour) = ArmourData.GetArmoursOfType(_hero.Class.ArmourType).ElementAt(0);

            _hero.ObtainItem(weapon);
            _hero.ObtainItem(armour);
            _hero.EquipWeapon(weapon);
            _hero.EquipArmour(armour);
        }

        private void SaveToUserFile()
        {
            Hero hero = _hero;
            StreamWriter userFileWriter = File.CreateText($"{_userPath}{hero.Name}.txt");

            userFileWriter.WriteLine(hero.Name);
            userFileWriter.WriteLine(hero.Level);
            userFileWriter.WriteLine(hero.Class.Name);
            userFileWriter.WriteLine(hero.Experience);
            userFileWriter.WriteLine(hero.Gold);
            userFileWriter.WriteLine(hero.Wins);
            userFileWriter.WriteLine(hero.Losses);

            if (hero.Weapon != null)
            {
                userFileWriter.WriteLine(hero.Weapon.Name);
            }

            if (hero.Armour != null)
            {
                userFileWriter.WriteLine(hero.Armour.Name);
            }

            userFileWriter.Close();

            string inventoryPath = $"{_userPath}{hero.Name}Inventory.txt";

            SaveInventory(inventoryPath, hero.WeaponInventory);
            SaveInventory(inventoryPath, hero.ArmourInventory);
            SaveInventory(inventoryPath, hero.ConsumableInventory);
        }

        private void SaveInventory(string path, List<Weapon> weaponList)
        {
            StreamWriter inventoryFileWriter = new StreamWriter(path);

            inventoryFileWriter.WriteLine("Weapons");

            foreach (Weapon weapon in weaponList)
            {
                inventoryFileWriter.WriteLine(weapon.Name);
            }

            inventoryFileWriter.Close();
        }

        private void SaveInventory(string path, List<Armour> armourList)
        {
            StreamWriter inventoryFileWriter = new StreamWriter(path, true);

            inventoryFileWriter.WriteLine("Armour");

            foreach (Armour armour in armourList)
            {
                inventoryFileWriter.WriteLine(armour.Name);
            }

            inventoryFileWriter.Close();
        }

        private void SaveInventory(string path, List<Consumable> consumableList)
        {
            StreamWriter inventoryFileWriter = new StreamWriter(path, true);

            inventoryFileWriter.WriteLine("Consumables");

            foreach (Consumable consumable in consumableList)
            {
                inventoryFileWriter.WriteLine(consumable.Name);
            }

            inventoryFileWriter.Close();
        }

        private void LoadUser(string username)
        {
            string userFile = $"{_userPath}{username}.txt";
            StreamReader reader = new StreamReader(userFile);

            reader.ReadLine();
            int level = int.Parse(reader.ReadLine());
            string pClass = reader.ReadLine();

            GenerateHero(username, level, pClass);

            _hero.SetExperience(int.Parse(reader.ReadLine()));
            _hero.Gold = int.Parse(reader.ReadLine());
            _hero.Wins = int.Parse(reader.ReadLine());
            _hero.Losses = int.Parse(reader.ReadLine());

            LoadInventory(username);

            _hero.EquipWeapon(WeaponData.Weapons[reader.ReadLine()]);
            _hero.EquipArmour(ArmourData.Armours[reader.ReadLine()]);

            reader.Close();
        }

        private void LoadInventory(string username)
        {
            string inventoryFile = $"{_userPath}{username}Inventory.txt";
            StreamReader reader = new StreamReader(inventoryFile);
            string category = "";

            do
            {
                string value = reader.ReadLine();

                if (value == "Weapons" || value == "Armour" || value == "Consumables")
                {
                    category = value;
                }
                else
                {
                    switch (category)
                    {
                        case "Weapons":
                            Weapon weapon = WeaponData.Weapons[value];
                            _hero.AddToInventory(weapon);
                            break;
                        case "Armour":
                            Armour armour = ArmourData.Armours[value];
                            _hero.AddToInventory(armour);
                            break;
                        case "Consumables":
                            Consumable consumable = ConsumableData.Consumables[value];
                            _hero.AddToInventory(consumable);
                            break;
                    }
                }
            } while (!reader.EndOfStream);

            reader.Close();
        }

        private bool IsExistingPlayer(string name)
        {
            StreamReader stream = new StreamReader(_userListPath);
            string users = stream.ReadToEnd();
            stream.Close();
            return users.Contains(name);
        }

        private string GetUserInput(string prompt)
        {
            string input = "";

            WriteLine(prompt);

            do
            {
                input = ReadLine();
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

    }
}
