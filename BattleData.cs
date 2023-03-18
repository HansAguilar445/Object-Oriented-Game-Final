using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal static class BattleData
    {
        public static Dictionary<string, Battle> Battles { get; } = new Dictionary<string, Battle>();

        static BattleData()
        {
            List<string> battleNames = new List<string>
            {
                "A Basic Battle",
                "We've Got Wyverns",
                "A Dance With The Fairies",
                "Deployment",
                "Conductor and Adjutant",

                "A Funny Guy",
                "Initial L",
                "Back In The Mine",
                "Wild Hunt",
                "The One Who All Electronics Fear",

                "Attack of the 'Evil' Alien Robots",
                "The Bread King",
                "Giant Enemy Crab",
                "Radical Lord",
                "Lone Swordsman"
            };

            List<int> recommendedLevels = new List<int>
            { 1, 3, 3, 5, 7, 7, 9, 9, 11,
                13, 13, 15, 15, 20, 20 };

            List<List<Enemy>> enemies = new List<List<Enemy>>
            {
                new List<Enemy>
                {
                    EnemyData.Enemies["Automaton"].Clone(),
                    EnemyData.Enemies["Automaton"].Clone(),
                    EnemyData.Enemies["Automaton"].Clone(),
                    EnemyData.Enemies["Automaton"].Clone(),
                    EnemyData.Enemies["Automaton"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Wyvern"].Clone(),
                    EnemyData.Enemies["Wyvern"].Clone(),
                    EnemyData.Enemies["Wyvern"].Clone(),
                    EnemyData.Enemies["Wyvern"].Clone(),
                    EnemyData.Enemies["Wyvern"].Clone(),
                    EnemyData.Enemies["Wyvern"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Fairy"].Clone(),
                    EnemyData.Enemies["Fairy"].Clone(),
                    EnemyData.Enemies["Fairy"].Clone(),
                    EnemyData.Enemies["Fairy"].Clone(),
                    EnemyData.Enemies["Fairy"].Clone(),
                    EnemyData.Enemies["Fairy"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Rookie Mercenary"].Clone(),
                    EnemyData.Enemies["Trainee Ninja"].Clone(),
                    EnemyData.Enemies["Rookie Mercenary"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Rookie Mercenary"].Clone(),
                    EnemyData.Enemies["Adjutant"].Clone()
                },

                new List<Enemy>
                {
                    EnemyData.Enemies["Dragonborn Bard"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Wild Driver"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Block Person"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Barghest"].Clone(),
                    EnemyData.Enemies["Dormarch"].Clone(),
                    EnemyData.Enemies["Barghest"].Clone(),
                    EnemyData.Enemies["Dormarch"].Clone(),
                    EnemyData.Enemies["Barghest"].Clone(),
                    EnemyData.Enemies["Dormarch"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Walking Techbane"].Clone()
                },

                new List<Enemy>
                {
                    EnemyData.Enemies["'Evil' Alien Robot"].Clone(),
                    EnemyData.Enemies["'Evil' Alien Robot"].Clone(),
                    EnemyData.Enemies["'Evil' Alien Robot"].Clone(),
                    EnemyData.Enemies["'Evil' Alien Robot"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Gravity Distortion"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Giant Enemy Crab"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Rogue Interface"].Clone()
                },
                new List<Enemy>
                {
                    EnemyData.Enemies["Star Warrior"].Clone()
                }
            };

            List<List<int>> enemyCounts = new List<List<int>>
            {
                new List<int> { 2, 2, 1 },
                new List<int> { 2, 2, 2 },
                new List<int> { 3, 3 },
                new List<int> { 3 },
                new List<int> { 2 },

                new List<int> { 1 },
                new List<int> { 1 },
                new List<int> { 1 },
                new List<int> { 2, 2, 2 },
                new List<int> { 1 },

                new List<int> { 2, 2 },
                new List<int> { 1 },
                new List<int> { 1 },
                new List<int> { 1 },
                new List<int> { 1 }
            };


            for (int i = 0; i < battleNames.Count; i++)
            {
                Battle battle = new Battle(enemies[i], enemyCounts[i], recommendedLevels[i], battleNames[i]);
                Battles.Add(battleNames[i], battle);
            }
        }
    }
}
