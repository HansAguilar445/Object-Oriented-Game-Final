using System.Text;

namespace FinalProject
{
    internal class Hero : Combatant
    {
        public int Experience { get; private set; }
        public int Gold { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public bool Retreated = false;

        public Class Class { get; private set; }
        private Dictionary<string, int> _baseStats { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> FinalizedStats { get; private set; } = new Dictionary<string, int>();
        public Weapon Weapon { get; private set; }
        public Armour Armour { get; private set; }
        private AttackAction _attackAction { get; }
        private GuardAction _guardAction { get; }

        public List<Weapon> WeaponInventory { get; private set; } = new();
        public List<Armour> ArmourInventory { get; private set; } = new();
        public List<Consumable> ConsumableInventory { get; private set; } = new();
        public List<Enemy> Targets { get; set; } = new();

        public Hero(string name, int level, Class pClass) : base(name, level)
        {
            Class = pClass;
            _attackAction = WeaponData.Attack;
            _guardAction = WeaponData.Guard;
            GenerateStats();


            foreach (CombatAction skill in WeaponData.Actions[Class.WeaponType])
            {
                Actions.Add(skill.Name, skill);
            }
        }

        public bool EquipWeapon(Weapon weapon)
        {
            if ((Class.WeaponType == weapon.Type) && (Level >= weapon.LevelRequirement))
            {
                Weapon = weapon;
                GenerateStats();
                Assign();
                return true;
            }
            return false;
        }

        public bool EquipArmour(Armour armour)
        {
            if (((int)Class.ArmourType >= (int)armour.Type) && (Level >= armour.LevelRequirement))
            {
                Armour = armour;
                GenerateStats();
                Assign();
                return true;
            }

            return false;
        }

        public void ObtainItem(Armour armour)
        {
            if (AddToInventory(armour))
            {
                WriteLine($"{armour.Name} obtained.");
            }
            else
            {
                WriteLine($"{armour.Name} sold for {armour.SellPrice} gold.");
            }
        }

        public void ObtainItem(Weapon weapon)
        {
            if (AddToInventory(weapon))
            {
                WriteLine($"{weapon.Name} obtained.");
            }
            else
            {
                WriteLine($"{weapon.Name} sold for {weapon.SellPrice} gold.");
            }
        }

        public void ObtainItem(Consumable consumable)
        {
            if (AddToInventory(consumable))
            {
                WriteLine($"{consumable.Name} obtained.");
            }
            else
            {
                WriteLine($"{consumable.Name} sold for {consumable.SellPrice} gold.");
            }
        }

        public List<Weapon> GetWeaponsOfType(Weapon.WeaponType type)
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (Weapon weapon in WeaponInventory)
            {
                if (weapon.Type == type)
                {
                    weapons.Add(weapon);
                }
            }

            return weapons;
        }

        public List<Armour> GetArmourOfType(Armour.ArmourType type)
        {
            List<Armour> armours = new List<Armour>();

            foreach (Armour armour in ArmourInventory)
            {
                if (armour.Type == type)
                {
                    armours.Add(armour);
                }
            }

            return armours;
        }

        public bool AddToInventory(Weapon weapon)
        {
            if (WeaponInventory.Count < 99)
            {
                WeaponInventory.Add(weapon);
                return true;
            }

            WriteLine("Inventory is full.");

            if ((Gold + weapon.SellPrice) > int.MaxValue)
            {
                Gold = int.MaxValue;
            }
            else
            {
                Gold += weapon.SellPrice;
            }

            return false;
        }

        public bool AddToInventory(Armour armour)
        {
            if (ArmourInventory.Count < 99)
            {
                ArmourInventory.Add(armour);
                return true;
            }

            WriteLine("Inventory is full.");

            if ((Gold + armour.SellPrice) > int.MaxValue)
            {
                Gold = int.MaxValue;
            }
            else
            {
                Gold += armour.SellPrice;
            }

            return false;
        }

        public bool AddToInventory(Consumable consumable)
        {
            if (WeaponInventory.Count < 99)
            {
                ConsumableInventory.Add(consumable);
                return true;
            }

            WriteLine("Inventory is full.");

            if ((Gold + consumable.SellPrice) > int.MaxValue)
            {
                Gold = int.MaxValue;
            }
            else
            {
                Gold += consumable.SellPrice;
            }

            return false;
        }

        public void Sell(Weapon weapon)
        {
            WeaponInventory.Remove(weapon);

            if ((Gold + weapon.SellPrice) > int.MaxValue)
            {
                Gold = int.MaxValue;
            }
            else
            {
                Gold += weapon.SellPrice;
            }
        }

        public void Sell(Armour armour)
        {
            ArmourInventory.Remove(armour);

            if ((Gold + armour.SellPrice) > int.MaxValue)
            {
                Gold = int.MaxValue;
            }
            else
            {
                Gold += armour.SellPrice;
            }
        }

        public void Sell(Consumable consumable)
        {
            ConsumableInventory.Remove(consumable);

            if ((Gold + consumable.SellPrice) > int.MaxValue)
            {
                Gold = int.MaxValue;
            }
            else
            {
                Gold += consumable.SellPrice;
            }
        }

        public void SetExperience(int xp)
        {
            int experienceThreshold = ExperienceThreshold();
            if (Level < 20)
            {
                Experience += xp;

                if (Experience >= experienceThreshold)
                {
                    do
                    {
                        Level++;
                        Experience -= experienceThreshold;
                        GenerateStats();
                    } while (Experience >= experienceThreshold);

                    if (Level == 20)
                    {
                        Experience = 0;
                    }
                }
            }
            else
            {
                Level = 20;
            }
        }

        public void AwardExperience(int xp)
        {
            int experienceThreshold = ExperienceThreshold();
            if (Level < 20)
            {
                Experience += xp;

                if (Experience >= experienceThreshold)
                {
                    WriteLine($"{Name} leveled up!");
                    do
                    {
                        Level++;
                        Experience -= experienceThreshold;
                        GenerateStats();
                    } while (Experience >= experienceThreshold);

                    if (Level == 20)
                    {
                        Experience = 0;
                    }
                }
            }
        }

        public void TakeAction(string backlog)
        {
            bool isLooping = true;
            StringBuilder info = new StringBuilder();
            info.Append(backlog);

            do
            {
                List<string> options = new List<string>
                {
                    "Attack",
                    "Guard",
                    "Skill",
                    "Items",
                    "Analyze",
                    "Escape"
                };
                Menu menu = new Menu(options);
                string action = menu.RunMenu(info.ToString());

                Clear();

                switch (action)
                {
                    case "Attack":
                        _attackAction.PerformAction(this, GetTarget());
                        isLooping = false;
                        break;
                    case "Guard":
                        _guardAction.PerformAction(this);
                        isLooping = false;
                        break;
                    case "Skill":
                        info.AppendLine("\nSelect an option:");

                        List<string> ops = new List<string>();

                        foreach ((_, CombatAction act) in Actions)
                        {
                            ops.Add(act.ToString());
                        }

                        ops.Add("Back");
                        Menu skillMenu = new Menu(ops);
                        string selection = skillMenu.RunMenu(info.ToString());

                        if (selection != "Back")
                        {
                            string actionName = selection.Split(" - ")[0];
                            if (Actions[actionName].CooldownTimer == 0)
                            {
                                Clear();
                                if (Actions[actionName] is AttackAction)
                                {
                                    AttackAction attack = (AttackAction)Actions[actionName];
                                    attack.PerformAction(this, GetTarget());
                                }
                                else
                                {
                                    Actions[actionName].PerformAction(this);
                                }

                                isLooping = false;
                            }
                            else
                            {
                                info.Clear();
                                info.AppendLine(backlog);
                                info.AppendLine("Skill is unavailable.");
                            }
                        }
                        break;
                    case "Items":
                        info.AppendLine("\nSelect an option:");

                        List<string> consumables = new List<string>();

                        foreach (Consumable consumable in ConsumableInventory)
                        {
                            consumables.Add(consumable.Name);
                        }

                        consumables.Add("Back");
                        Menu consumableMenu = new Menu(consumables);
                        string choice = consumableMenu.RunMenu(info.ToString());

                        if (choice != "Back")
                        {
                            Clear();
                            Consumable consumable = GetConsumable(choice);
                            WriteLine($"{Name} used a {consumable.Name}.");
                            consumable.ActivateEffect(this);
                            ConsumableInventory.Remove(consumable);
                            isLooping = false;
                        }
                        break;
                    case "Analyze":
                        WriteLine(GetTarget().GetAllInfo());
                        WriteLine("Press any key to return to the battle menu.");
                        ReadKey();
                        break;
                    case "Escape":
                        Retreated = true;
                        isLooping = false;
                        break;
                }

            } while (isLooping);

        }

        private void SelectSkill()
        {

        }

        public void BrowseSelf()
        {
            bool isLooping = true;
            string dialogue = $"{Name}: What should I check?";
            List<string> options = new List<string>
            {
                "Self",
                "Weapons",
                "Armour",
                "Consumables",
                "Back"
            };
            Menu menu = new Menu(options);

            do
            {

                string category = menu.RunMenu(dialogue);
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(dialogue);

                switch (category)
                {
                    case "Self":
                        View();
                        break;
                    case "Weapons":
                        BrowseOwnWeapons(builder);
                        break;
                    case "Armour":
                        BrowseOwnArmour(builder);
                        break;
                    case "Consumables":
                        if (ConsumableInventory.Count > 0)
                        {
                            BrowseOwnConsumables(builder);
                        }
                        break;
                    case "Back":
                        isLooping = false;
                        break;
                }
            } while (isLooping);
        }

        private void BrowseOwnWeapons(StringBuilder builder)
        {
            bool isRepeating = true;

            List<string> weaponTypeOptions = new List<string>(Enum.GetNames(typeof(Weapon.WeaponType)));
            weaponTypeOptions.Insert(0, "Back");

            Menu wares = new Menu(weaponTypeOptions);

            do
            {
                string choice = wares.RunMenu(builder.ToString());

                if (weaponTypeOptions.IndexOf(choice) != 0)
                {
                    Weapon.WeaponType type = (Weapon.WeaponType)Enum.Parse(typeof(Weapon.WeaponType), choice);
                    BrowseOwnWeaponsOfType(builder, type);
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private void BrowseOwnWeaponsOfType(StringBuilder builder, Weapon.WeaponType type)
        {
            bool isRepeating = true;

            string initialMessage = builder.ToString();

            List<string> weaponOptions = new List<string>();

            weaponOptions.Add("Back");

            foreach (Weapon weapon in GetWeaponsOfType(type))
            {
                weaponOptions.Add(weapon.Name);
            }

            Menu wares = new Menu(weaponOptions);

            do
            {
                string choice = wares.RunMenu(builder.ToString());

                int index = weaponOptions.IndexOf(choice);
                if (index != 0)
                {
                    Weapon weapon = GetWeaponsOfType(type)[index - 1];
                    List<string> options = new List<string> { "View", "Back" };

                    if ((type == Class.WeaponType) &&
                        (weapon.LevelRequirement <= Level))
                    {
                        options.Insert(1, "Equip");
                    }

                    Menu menu = new Menu(options);

                    string selection = menu.RunMenu(weapon.Name);

                    if (selection == "View")
                    {
                        View(weapon);
                    }
                    else if (selection == "Equip")
                    {
                        TryEquip(weapon);
                    }

                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private void BrowseOwnArmour(StringBuilder builder)
        {
            bool isRepeating = true;

            string initialMessage = builder.ToString();
            List<string> weaponTypeOptions = new List<string>(Enum.GetNames(typeof(Armour.ArmourType)));
            weaponTypeOptions.Insert(0, "Back");

            Menu wares = new Menu(weaponTypeOptions);

            do
            {
                string choice = wares.RunMenu(builder.ToString());

                if (weaponTypeOptions.IndexOf(choice) != 0)
                {
                    Armour.ArmourType type = (Armour.ArmourType)Enum.Parse(typeof(Armour.ArmourType), choice);
                    BrowseOwnArmourOfType(builder, type);
                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private void BrowseOwnArmourOfType(StringBuilder builder, Armour.ArmourType type)
        {
            bool isRepeating = true;

            List<string> armourOptions = new List<string>();

            armourOptions.Add("Back");

            foreach (Armour armour in GetArmourOfType(type))
            {
                armourOptions.Add(armour.Name);
            }

            Menu wares = new Menu(armourOptions);

            do
            {
                string choice = wares.RunMenu(builder.ToString());

                int index = armourOptions.IndexOf(choice);
                if (index != 0)
                {
                    Armour armour = GetArmourOfType(type)[index - 1];
                    List<string> options = new List<string> { "View", "Back" };

                    if ((type <= Class.ArmourType) &&
                        (armour.LevelRequirement <= Level))
                    {
                        options.Insert(1, "Equip");
                    }

                    Menu menu = new Menu(options);

                    string selection = menu.RunMenu(armour.Name);

                    if (selection == "View")
                    {
                        View(armour);
                    }
                    else if (selection == "Equip")
                    {
                        TryEquip(armour);
                    }

                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private void BrowseOwnConsumables(StringBuilder builder)
        {
            bool isRepeating = true;

            List<string> consumableOptions = new List<string>();

            consumableOptions.Add("Back");

            foreach (Consumable consumable in ConsumableInventory)
            {
                consumableOptions.Add(consumable.Name);
            }

            Menu wares = new Menu(consumableOptions);

            do
            {
                string choice = wares.RunMenu(builder.ToString());

                int index = consumableOptions.IndexOf(choice);
                if (index != 0)
                {
                    Consumable consumable = ConsumableInventory[index - 1];
                    List<string> options = new List<string> { "View", "Back" };

                    Menu menu = new Menu(options);

                    string selection = menu.RunMenu(consumable.Name);

                    if (selection == "View")
                    {
                        View(consumable);
                    }

                }
                else
                {
                    isRepeating = false;
                }

            } while (isRepeating);
        }

        private void TryEquip(Weapon weapon)
        {
            Dictionary<string, int> simulatedStats = SimulateStats(weapon);
            string comparison = CreateComparison(simulatedStats, weapon);
            Menu confirmMenu = new Menu(new List<string> { "Yes", "No" });

            string choice = confirmMenu.RunMenu(comparison);

            if (choice == "Yes")
            {
                EquipWeapon(weapon);
            }
        }

        private void TryEquip(Armour armour)
        {
            Dictionary<string, int> simulatedStats = SimulateStats(armour);
            string comparison = CreateComparison(simulatedStats, armour);
            Menu confirmMenu = new Menu(new List<string> { "Yes", "No" });

            string choice = confirmMenu.RunMenu(comparison);

            if (choice == "Yes")
            {
                EquipArmour(armour);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Name);
            builder.AppendLine($"Level: {Level}");
            builder.AppendLine($"Class: {Class.Name}");
            builder.AppendLine($"Experience: {Experience}");
            builder.AppendLine($"Wins: {Wins}");
            builder.AppendLine($"Losses: {Losses}");
            builder.AppendLine($"HP: {CurrentHealth}/{FinalizedStats["HP"]}");
            builder.AppendLine($"Attack Power: {AttackPower}");
            builder.AppendLine($"Defence: {Defence}");
            builder.AppendLine($"Agility: {Agility}");
            builder.AppendLine($"Dexterity: {Dexterity}");
            builder.AppendLine($"Critical Hit Rate: {Critical}%");
            builder.AppendLine($"\nCurrent weapon: {Weapon.Name}");
            builder.AppendLine($"Current armour set: {Armour.Name}");
            builder.AppendLine("\nSkills:");

            foreach ((_, CombatAction action) in Actions)
            {
                builder.AppendLine($"\t{action.GetSkillInfo()}");
            }

            return builder.ToString();
        }

        private Enemy GetTarget()
        {
            List<string> targets = new List<string>();

            foreach (Enemy enemy in Targets)
            {
                if (enemy.Alive)
                {
                    targets.Add(enemy.Name);
                }
            }

            Menu menu = new Menu(targets);

            string selection = menu.RunMenu("Select a target:");

            foreach (Enemy enemy1 in Targets)
            {
                if (enemy1.Name == selection)
                {
                    return enemy1;
                }
            }

            return null;
        }

        private Consumable GetConsumable(string name)
        {
            foreach (Consumable consumable in ConsumableInventory)
            {
                if (consumable.Name == name)
                {
                    return consumable;
                }
            }

            return null;
        }

        private void GenerateStats()
        {
            GenerateBaseStats();

            foreach ((string statName, int baseStat) in _baseStats)
            {
                FinalizedStats.TryAdd(statName, 0);
                FinalizedStats[statName] = baseStat + GetGearModifier(statName);
            }

            Stats = FinalizedStats;
            Assign();
        }

        private void GenerateBaseStats()
        {
            foreach ((string statName, int baseStat) in Class.Bases)
            {
                _baseStats.TryAdd(statName, 0);

                if (statName != "Critical Rate")
                {
                    _baseStats[statName] = baseStat * Level;
                }

            }

            _baseStats["Critical Rate"] = GenerateCriticalMod();
        }

        private int GetGearModifier(string statName)
        {
            int mod = 0;

            if (Weapon != null)
            {
                mod += Weapon.Modifiers[statName];
            }

            if (Armour != null)
            {
                mod += Armour.Modifiers[statName];
            }

            return mod;
        }

        private int GetGearModifier(Weapon weapon, string statName)
        {
            int mod = 0;

            if (Weapon != null)
            {
                mod += weapon.Modifiers[statName];
            }

            if (Armour != null)
            {
                mod += Armour.Modifiers[statName];
            }

            return mod;
        }

        private int GetGearModifier(Armour armour, string statName)
        {
            int mod = 0;

            if (Weapon != null)
            {
                mod += Weapon.Modifiers[statName];
            }

            if (Armour != null)
            {
                mod += armour.Modifiers[statName];
            }

            return mod;
        }

        private int GenerateCriticalMod()
        {
            int criticalMod = Level / 5;

            if (Level % 5 == 0)
            {
                criticalMod *= 5;
            }

            return Class.Bases["Critical Rate"] + criticalMod;
        }

        private int ExperienceThreshold()
        {
            if (Level < 99)
            {
                return 500 * Level;
            }
            else
            {
                return 0;
            }
        }

        private Dictionary<string, int> SimulateStats(Weapon weapon)
        {
            Dictionary<string, int> simulatedStats = new Dictionary<string, int>();

            foreach ((string statName, int value) in _baseStats)
            {
                simulatedStats.Add(statName, value);
                simulatedStats[statName] += GetGearModifier(weapon, statName);
            }

            return simulatedStats;
        }

        private Dictionary<string, int> SimulateStats(Armour armour)
        {
            Dictionary<string, int> simulatedStats = new Dictionary<string, int>();

            foreach ((string statName, int value) in _baseStats)
            {
                simulatedStats.Add(statName, value);
                simulatedStats[statName] += GetGearModifier(armour, statName);
            }

            return simulatedStats;
        }

        private string CreateComparison(Dictionary<string, int> simulatedStats, Weapon weapon)
        {
            StringBuilder builder = new StringBuilder();
            string simHp = $" => {simulatedStats["HP"]}/{simulatedStats["HP"]} ({simulatedStats["HP"] - FinalizedStats["HP"]})\n";
            string simAtk = $" => {simulatedStats["Attack Power"]} ({simulatedStats["Attack Power"] - AttackPower})\n";
            string simDef = $" => {simulatedStats["Defence"]} ({simulatedStats["Defence"] - Defence})\n";
            string simAgi = $" => {simulatedStats["Agility"]} ({simulatedStats["Agility"] - Agility})\n";
            string simDex = $" => {simulatedStats["Dexterity"]} ({simulatedStats["Dexterity"] - Dexterity})\n";
            string simCrit = $" => {simulatedStats["Critical Rate"]}% ({simulatedStats["Critical Rate"] - Critical})\n";


            builder.Append($"HP: {CurrentHealth}/{FinalizedStats["HP"]}");
            builder.Append(simulatedStats["HP"] != FinalizedStats["HP"] ? simHp : "\n");
            builder.Append($"Attack Power: {AttackPower}");
            builder.Append(simulatedStats["Attack Power"] != AttackPower ? simAtk : "\n");
            builder.Append($"Defence: {Defence}");
            builder.Append(simulatedStats["Defence"] != Defence ? simDef : "\n");
            builder.Append($"Agility: {Agility}");
            builder.Append(simulatedStats["Agility"] != Agility ? simAgi : "\n");
            builder.Append($"Dexterity: {Dexterity}");
            builder.Append(simulatedStats["Dexterity"] != Dexterity ? simDex : "\n");
            builder.Append($"Critical Hit Rate: {Critical}%");
            builder.Append(simulatedStats["Critical Rate"] != Agility ? simCrit : "\n");
            builder.AppendLine($"{Weapon.Name} => {weapon.Name}");

            builder.AppendLine($"\nEquip {weapon.Name}?");

            return builder.ToString();
        }

        private string CreateComparison(Dictionary<string, int> simulatedStats, Armour armour)
        {
            StringBuilder builder = new StringBuilder();
            string simHp = $" => {simulatedStats["HP"]}/{simulatedStats["HP"]} ({simulatedStats["HP"] - FinalizedStats["HP"]})\n";
            string simAtk = $" => {simulatedStats["Attack Power"]} ({simulatedStats["Attack Power"] - AttackPower})\n";
            string simDef = $" => {simulatedStats["Defence"]} ({simulatedStats["Defence"] - Defence})\n";
            string simAgi = $" => {simulatedStats["Agility"]} ({simulatedStats["Agility"] - Agility})\n";
            string simDex = $" => {simulatedStats["Dexterity"]} ({simulatedStats["Dexterity"] - Dexterity})\n";
            string simCrit = $" => {simulatedStats["Critical Rate"]}% ({simulatedStats["Critical Rate"] - Critical})\n";


            builder.Append($"HP: {CurrentHealth}/{FinalizedStats["HP"]}");
            builder.Append(simulatedStats["HP"] != FinalizedStats["HP"] ? simHp : "\n");
            builder.Append($"Attack Power: {AttackPower}");
            builder.Append(simulatedStats["Attack Power"] != AttackPower ? simAtk : "\n");
            builder.Append($"Defence: {Defence}");
            builder.Append(simulatedStats["Defence"] != Defence ? simDef : "\n");
            builder.Append($"Agility: {Agility}");
            builder.Append(simulatedStats["Agility"] != Agility ? simAgi : "\n");
            builder.Append($"Dexterity: {Dexterity}");
            builder.Append(simulatedStats["Dexterity"] != Dexterity ? simDex : "\n");
            builder.Append($"Critical Hit Rate: {Critical}%");
            builder.Append(simulatedStats["Critical Rate"] != Agility ? simCrit : "\n");
            builder.AppendLine($"{Armour.Name} => {armour.Name}");

            builder.AppendLine($"\nEquip {armour.Name}?");

            return builder.ToString();
        }

        private void View()
        {
            Clear();
            WriteLine(ToString());
            WriteLine("\nPress any key to return to the previous menu.");
            ReadKey();
        }

        private void View(Weapon weapon)
        {
            Clear();
            WriteLine(weapon);
            WriteLine("Press any key to return to the previous screen.");
            ReadKey();
        }

        private void View(Armour armour)
        {
            Clear();
            WriteLine(armour);
            WriteLine("Press any key to return to the previous screen.");
            ReadKey();
        }

        private void View(Consumable consumable)
        {
            Clear();
            WriteLine(consumable);
            WriteLine("Press any key to return to the previous screen.");
            ReadKey();
        }
    }
}
