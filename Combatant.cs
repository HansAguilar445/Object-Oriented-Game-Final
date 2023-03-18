using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal abstract class Combatant
    {
        public HashSet<StatusEffect> Effects { get; set; } = new HashSet<StatusEffect>();
        public Dictionary<string, CombatAction> Actions { get; set; } = new Dictionary<string, CombatAction>();
        public Dictionary<string, int> Stats { get; set; } = new Dictionary<string, int>();
        private Dictionary<string, int> _alteredStats { get; set; } = new Dictionary<string, int>();
        public bool Alive { get { return CurrentHealth > 0; } }

        public string Name { get; set; }
        public int Level { get; set; }
        public int CurrentHealth 
        { 
            get { return _alteredStats["HP"]; } 
            set { _alteredStats["HP"] = value; }
        }
        public int AttackPower
        {
            get { return _alteredStats["Attack Power"]; }
            set { _alteredStats["Attack Power"] = value; }
        }
        public int Defence
        {
            get { return _alteredStats["Defence"]; }
            set { _alteredStats["Defence"] = value; }
        }
        public int Agility
        {
            get { return _alteredStats["Agility"]; }
            set { _alteredStats["Agility"] = value; }
        }
        public int Dexterity
        {
            get { return _alteredStats["Dexterity"]; }
            set { _alteredStats["Dexterity"] = value; }
        }
        public int Critical
        {
            get { return _alteredStats["Critical Rate"]; }
            set { _alteredStats["Critical Rate"] = value; }
        }

        public Combatant(string name, int level)
        {
            Name = name;
            Level = level;
        }

        public Combatant(string name, int level, Dictionary<string, int> stats)
        {
            Name = name;
            Level = level;
            Stats = stats;

            Assign();
        }

        public Combatant(string name, int level, Dictionary<string, int> stats, 
            Dictionary<string, CombatAction> actions)
        {
            Name = name;
            Level = level;
            Stats = stats;
            Actions = actions;

            Assign();
        }

        public void Assign()
        {
            foreach ((string statName, int value) in Stats)
            {
                _alteredStats.TryAdd(statName, 0);
                _alteredStats[statName] = value;
            }

            foreach((_, CombatAction action) in Actions)
            {
                action.CooldownTimer = 0;
            }
        }

        public void Assign(string name)
        {
            _alteredStats[name] = Stats[name];
        }

        public void RecoverHealth(int value)
        {
            int cap = Stats["HP"];
            CurrentHealth += value;

            if (CurrentHealth > cap)
            {
                CurrentHealth = cap;
            }
        }

        public virtual void TakeAction()
        {

        }

    }
}
