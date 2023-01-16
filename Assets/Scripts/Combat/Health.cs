using UnityEngine;
using UnityEngine.AI;


namespace RPG.Combat 
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        Collider collider;

        bool isDead = false;


        void Awake() {
            collider = GetComponent<Collider>();
        }


        public bool IsDead() 
        {
            return isDead;
        }

        public void TakeDamage(float damage = 0)
        {
            healthPoints = Mathf.Max(healthPoints -= damage, 0);

            if (healthPoints == 0)
                Die();
        }

        private void Die()
        {
            if (!isDead)
            {
                if (collider != null)
                    collider.enabled = false;
                    
                isDead = true;                
                gameObject.GetComponent<NavMeshAgent>().enabled = false;                
                GetComponent<Animator>().SetTrigger("die");

            }
        }
    }
}