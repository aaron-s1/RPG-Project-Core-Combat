using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public float GetPoints() =>
            experiencePoints;

        public void GainExperience (float experience) {
            experiencePoints += experience; 
            Debug.Log("new experience = " + experiencePoints);
        }


        public object CaptureState() => experiencePoints;

        public void RestoreState(object state) =>
            experiencePoints = (float)state;
    }
}