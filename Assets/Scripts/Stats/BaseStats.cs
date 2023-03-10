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

        [SerializeField] GameObject levelUpParticleEffect = null;

        public event Action onLevelUp;

        int currentLevel = 0;

        void Start() {
            currentLevel = CalculateLevel();

            Experience experience = GetComponent<Experience>();            
            if (experience != null)
                experience.onExperienceGained += UpdateLevel;
        }

        void UpdateLevel() {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }


        public float GetStat(Stat stat) =>
            progression.GetStat(stat, characterClass, GetLevel()) + GetAdditiveModifier(stat);            



        public int GetLevel()
        {
            if (currentLevel < 1)
                currentLevel = CalculateLevel();

            return currentLevel;
        }

        
        float GetAdditiveModifier(Stat stat)
        {
            float total = 0;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                    total += modifier;
            }

            return total;
        }


        int CalculateLevel()
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

            Debug.Log("CalculateLevel()");
            Debug.Log("CalculateLevel(), new level = " + penultimateLevel + 1);

            return penultimateLevel + 1;
        }


    }
}
