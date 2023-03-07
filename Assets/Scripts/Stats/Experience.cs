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
            Debug.Log("Experience triggered GainExperience");
            experiencePoints += experience;
            onExperienceGained();
        }


        public object CaptureState() => experiencePoints;

        public void RestoreState(object state) =>
            experiencePoints = (int)state;
    }
}