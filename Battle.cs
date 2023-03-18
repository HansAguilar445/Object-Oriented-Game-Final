using System.Text;

namespace FinalProject
{
    internal class Battle
    {
        public Hero Hero { get; set; }
        public int RecommendedLevel { get; }
        public string Name { get; }
        private List<Enemy> _enemies { get; set; } = new List<Enemy>();
        private List<List<Enemy>> _enemyWave { get; set; } = new List<List<Enemy>>();
        private List<Enemy> _killedEnemies { get; set; } = new List<Enemy>();
        private List<int> _waveEnemyCount { get; set; } = new List<int>();
        private int _wave = 0;
        private int _goldEarned = 0;
        private int _experienceEarned = 0;

        public Battle(List<Enemy> enemies, List<int> _enemyCount, int recommendedLevel, string name)
        {
            _enemies = enemies;
            _waveEnemyCount = _enemyCount;
            RecommendedLevel = recommendedLevel;
            Name = name;
        }

        private void GenerateWave()
        {
            int waveMarker = 0;

            for (int i = 0; i < _waveEnemyCount.Count; i++)
            {
                List<Enemy> enemies = new List<Enemy>();

                for (int j = 0; j < _waveEnemyCount[i]; j++)
                {
                    Enemy enemy = _enemies[waveMarker];
                    enemy.Assign();
                    enemies.Add(enemy);
                    waveMarker++;
                }

                _enemyWave.Add(enemies);
            }

            NumberEnemies();
        }

        private void NumberEnemies()
        {
            Dictionary<string, int> enemyTypeCounter = new Dictionary<string, int>();
            foreach (Enemy enemy in _enemies)
            {
                if (!char.IsDigit(enemy.Name[enemy.Name.Length - 1]))
                {
                    enemyTypeCounter.TryAdd(enemy.Name, 0);
                    enemyTypeCounter[enemy.Name]++;
                    enemy.Name += $" {enemyTypeCounter[enemy.Name]}";
                }
            }
        }

        public void Fight()
        {
            StartFight();

            bool isLooping = true;

            do
            {
                foreach (Enemy enemy in _enemyWave[_wave])
                {
                    enemy.Target = Hero;
                }

                Hero.Targets = _enemyWave[_wave];

                Hero.TakeAction(GetBattleInfo());

                foreach (Enemy enemy in _enemyWave[_wave])
                {
                    if (enemy.Alive)
                    {
                        enemy.TakeAction();
                    }
                }

                IncrementTimer(Hero);

                foreach (Enemy enemy in _enemyWave[_wave])
                {
                    IncrementTimer(enemy);
                    enemy.Target = Hero;
                }

                if (CheckIfWaveCleared(_wave))
                {
                    _wave++;
                }

                if (!Hero.Alive || Hero.Retreated)
                {
                    WriteLine("You failed to achieve victory.");
                    Hero.Losses++;
                    Hero.Retreated = false;
                    Hero.Assign();
                    isLooping = false;
                }


                if (_wave >= _waveEnemyCount.Count)
                {
                    Hero.Wins++;

                    if ((Hero.Gold + _goldEarned) > int.MaxValue)
                    {
                        Hero.Gold = int.MaxValue;
                    }
                    else
                    {
                        Hero.Gold += _goldEarned;
                    }

                    WriteLine("You won!");
                    WriteLine($"You earned {_goldEarned}G and {_experienceEarned} XP.");

                    Hero.AwardExperience(_experienceEarned);
                    Hero.Assign();
                    Hero.Effects.Clear();

                    isLooping = false;
                }

                WriteLine("Press any key to advance.");
                ReadKey();
            } while (isLooping);
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Name: {Name}");
            builder.AppendLine($"Waves: {_waveEnemyCount.Count}");
            builder.AppendLine($"Recommended Level: {RecommendedLevel}");

            return builder.ToString();
        }

        private void StartFight()
        {
            _wave = 0;
            _goldEarned = 0;
            _experienceEarned = 0;
            _killedEnemies = new List<Enemy>();
            GenerateWave();
        }

        private void IncrementTimer(Combatant combatant)
        {
            if (combatant.Effects.Count > 0)
            {
                List<StatusEffect> expiredEffects = new();

                foreach (StatusEffect effect in combatant.Effects)
                {
                    effect.Timer++;

                    if (effect.Timer == effect.Duration)
                    {
                        effect.Timer = 0;
                        expiredEffects.Add(effect);
                    }
                }

                foreach (StatusEffect expiredEffect in expiredEffects)
                {
                    expiredEffect.Reversal(combatant);
                    combatant.Effects.Remove(expiredEffect);
                }
            }

            foreach ((_, CombatAction action) in combatant.Actions)
            {
                if (action.CooldownTimer > 0)
                {
                    action.CooldownTimer--;
                }
            }
        }

        private string GetBattleInfo()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{Name} - Wave {_wave + 1}\n");

            for (int i = 0; i < _enemyWave[_wave].Count; i++)
            {
                if (_enemyWave[_wave][i].Alive)
                {
                    builder.Append($"{_enemyWave[_wave][i]}");
                }
            }

            builder.AppendLine($"{Hero.Name} - {Hero.CurrentHealth}/{Hero.FinalizedStats["HP"]}");

            if (Hero.Effects.Count > 0)
            {
                builder.AppendLine("Active effects:");
                foreach (StatusEffect effect in Hero.Effects)
                {
                    builder.AppendLine($"{effect.Name}");
                }
            }

            return builder.ToString();
        }

        private bool CheckIfWaveCleared(int wave)
        {
            bool isCleared = true;
            List<Enemy> livingEnemies = new List<Enemy>();

            foreach (Enemy enemy in _enemyWave[wave])
            {
                if (enemy.Alive)
                {
                    isCleared = false;
                }
                else if (!enemy.Alive && !_killedEnemies.Contains(enemy))
                {
                    _killedEnemies.Add(enemy);
                    _goldEarned += enemy.GoldDropped;
                    _experienceEarned += enemy.ExperienceDropped;
                }
            }

            return isCleared;
        }

    }
}
