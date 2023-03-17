using System.Runtime.Serialization;
// using System.Runtime.Intrinsics.Arm.Arm64;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;
// using RPG.Resources;

namespace RPG.Attributes 
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        
        float healthPoints = -1f;


        Collider collider;

        bool isDead = false;


        void Awake() =>
            collider = GetComponent<Collider>();


        void Start()
        {            
            if (healthPoints < 0)
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void OnEnable() =>
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;

        void OnDisable() =>
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;


        public void TakeDamage(GameObject instigator, float damage = 0)
        {
            print(gameObject.name + " took damage: " + damage);

            healthPoints = Mathf.Max(healthPoints - damage, 0);
            
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }


        public float GetPercentage()
        {
            // var health = GetComponent<BaseStats>().GetStat(Stat.Health);
            // return (100 * healthPoints) / health;
            // return 
            // return healthPoints;
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }


        public string GetHealthPercentageAsText()
        {
            var health = GetComponent<BaseStats>().GetStat(Stat.Health);            
            var percent = 100 * (healthPoints / health);

            return percent.ToString(); // + "%";
        }


        void Die()
        {
            if (!isDead)
            {
                isDead = true;

                gameObject.GetComponent<NavMeshAgent>().enabled = false;

                if (collider != null)
                    collider.enabled = false;

                GetComponent<Animator>().SetTrigger("die");

                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }


        public void AwardExperience(GameObject instigator)
        {
            
            Experience experience = instigator.GetComponent<Experience>();

            if (experience == null)
                return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }


        void RegenerateHealth()
        {
            // float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);            
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);                                          
            regenHealthPoints *= regenerationPercentage / 100;

            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }


        public bool IsDead() => 
            isDead;


        #region Saving.

        public object CaptureState() =>
            healthPoints;

        public void RestoreState(object state) {
            healthPoints = (float)state;

            if (healthPoints == 0)
                Die();
        }

        #endregion
    }
}