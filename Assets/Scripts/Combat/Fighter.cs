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

        Transform target;

        float timeSinceLastAttack = 0;


        void Update() {
            
            timeSinceLastAttack += Time.deltaTime;

            MoveToAttackTarget();
        }


        void MoveToAttackTarget()
        {
            if (target == null)
                return;

            if (!GetIsInRange()) 
                GetComponent<Mover>().MoveTo(target.position);                

            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }


        void AttackBehavior() {

            if (timeSinceLastAttack > timeBetweenAttacks) {
                // This triggers Hit() event.
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }


        // Animation event.
        void Hit() {
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }


        public bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }


        public void AttackTarget(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }


        public void Cancel() =>
            target = null;
    }    
}