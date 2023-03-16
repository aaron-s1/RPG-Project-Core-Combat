using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        // public delegate void ExperienceGainedDelegate();
        public event Action onExperienceGained;


        public float GetPoints() =>
            experiencePoints;

        public void GainExperience (float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        void Tomato(){
            GainExperience(1f);
        }


        public object CaptureState() => experiencePoints;

        public void RestoreState(object state) =>
            experiencePoints = (int)state;
    }
}