using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/ New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [NonReorderable] [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, int[]>> lookupTable = null;

        public int GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            // if (stat == Stat.exp)

            int[] levels = lookupTable[characterClass][stat];

            if (levels.Length < level)
            {
                Debug.Log("failed level check");
                return 0;
            }

            // Debug.Log("hit level check");
            // Debug.Log("level = " + levels[level - 1]);
            // Debug.Log(level);
            return levels[level - 1];
            
            // [level] without adjustment makes Health work.
            // 
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();

            int[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, int[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, int[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }


        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            [NonReorderable] public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            [NonReorderable] public int[] levels;
        }
    }
}