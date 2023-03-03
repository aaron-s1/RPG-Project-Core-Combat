using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] int experiencePoints = 0;

        public int GetPoints() =>
            experiencePoints;

        public void GainExperience (int experience)
        {
            experiencePoints += experience; 
        }


        public object CaptureState() => experiencePoints;

        public void RestoreState(object state) =>
            experiencePoints = (int)state;
    }
}