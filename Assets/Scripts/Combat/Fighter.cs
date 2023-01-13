using UnityEngine;
using RPG.Movement;
using RPG.Core;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;

        Animator animator;

        Health target;

        float timeSinceLastAttack = 0;

        void Awake() =>
            animator = GetComponent<Animator>();

        void Update() {
                        
            timeSinceLastAttack += Time.deltaTime;

            MoveToAttackTarget();
        }


        void MoveToAttackTarget()
        {
            if (target == null)
                return;
            if (target.IsDead())
                return;

            if (!GetIsInRange()) 
                GetComponent<Mover>().MoveTo(target.transform.position);                

            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }


        void AttackBehavior() {

            if (timeSinceLastAttack > timeBetweenAttacks) {
                // This triggers Hit() event.
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }


        // Animation event.
        void Hit() {
            // Health healthComponent = target.GetComponent<Health>();
            target.TakeDamage(weaponDamage);
        }


        public bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }


        public void AttackTarget(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }


        public void Cancel() {
            animator.SetTrigger("stopAttack");  
            target = null;
        }
    }    
}