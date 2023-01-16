using UnityEngine;
using UnityEngine.AI;


namespace RPG.Core 
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        Collider collider;

        bool isDead = false;


        void Awake() =>
            collider = GetComponent<Collider>();


        public void TakeDamage(float damage = 0)
        {
            healthPoints = Mathf.Max(healthPoints -= damage, 0);

            if (healthPoints == 0)
                Die();
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


        public bool IsDead() => 
            isDead;
    }
}