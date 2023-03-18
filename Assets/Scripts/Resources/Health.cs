using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Attributes 
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        
        LazyValue<float> healthPoints;


        Collider collider;

        bool isDead = false;


        void Awake() {
            collider = GetComponent<Collider>();

            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth() =>
            GetComponent<BaseStats>().GetStat(Stat.Health);


        void Start() =>
            healthPoints.ForceInit();


        void OnEnable() =>
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;

        void OnDisable() =>
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;



        public void TakeDamage(GameObject instigator, float damage = 0)
        {
            print(gameObject.name + " took damage: " + damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            
            if (healthPoints.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints() =>
            healthPoints.value;
            

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
            return 100 * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }


        public string GetHealthPercentageAsText()
        {
            var health = GetComponent<BaseStats>().GetStat(Stat.Health);            
            var percent = 100 * (healthPoints.value / health);

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

            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }


        public bool IsDead() => 
            isDead;


        #region Saving.

        public object CaptureState() =>
            healthPoints.value;

        public void RestoreState(object state) {
            healthPoints.value = (float)state;

            if (healthPoints.value == 0)
                Die();
        }

        #endregion
    }
}