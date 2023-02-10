using System;
using UnityEngine;
using RPG.Movement;
using RPG.Core;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] float timeBetweenAttacks = 1f;

        Weapon currentWeapon = null;

        Animator animator;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;


        void Start() {
            animator = GetComponent<Animator>();

            if (defaultWeapon != null)
                EquipWeapon(defaultWeapon);
        }

        void Update()
        {                        
            timeSinceLastAttack += Time.deltaTime;
            MoveToAttackTarget();
        }

        public void EquipWeapon(Weapon weapon) {
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        void MoveToAttackTarget()
        {
            if (target == null || target.IsDead())
                return;

            if (!GetIsInRange()) 
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);                

            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }


        void AttackBehavior()
        {
            transform.LookAt(target.transform.position);

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This triggers Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;

            Health targetHealth = combatTarget.GetComponent<Health>();

            return (targetHealth != null && !targetHealth.IsDead());
        }


        #region Attacking: Toggle Animator triggers.

        void TriggerAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }
        
        void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        public void Cancel()
        {            
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        #endregion

        

        // Played via Animation event.
        public void Hit(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }


        // Played via Animation event.
        public void Hit()
        {
            // target = GetComponent<Health>();
            // target = combatTarget.GetComponent<Health>();

            if (target == null)
                return;
            

            if (currentWeapon.HasProjectile())
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            else
                target.TakeDamage(currentWeapon.GetDamage());
        }

        void Shoot() => 
            Hit();


        public bool GetIsInRange() =>
            Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
    }
}