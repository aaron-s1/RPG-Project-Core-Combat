using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] CharacterClass characterClass;
        [Range(1, 99)] [SerializeField] int startingLevel = 1;

        [SerializeField] Progression progression = null;


        public int GetStat(Stat stat) =>
            progression.GetStat(stat, characterClass, GetLevel());


        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null)
                return startingLevel;

            float currentXP = experience.GetPoints();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);


            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);

                if (XPToLevelUp > currentXP)
                    return level;
            }

            return penultimateLevel + 1;
        }
    }
}
