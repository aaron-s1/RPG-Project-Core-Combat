// using System.Runtime.Intrinsics.Arm.Arm64;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using RPG.Resources;

namespace RPG.Attributes 
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        Collider collider;

        bool isDead = false;


        void Awake() =>
            collider = GetComponent<Collider>();

        void Start() =>
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);


        public void TakeDamage(GameObject instigator, float damage = 0) {
            healthPoints = Mathf.Max(healthPoints -= damage, 0);
            
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }


        public float GetPercentage()
        {
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
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