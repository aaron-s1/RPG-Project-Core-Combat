using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Weapon defaultWeapon = null;

        LazyValue<Weapon> currentWeapon;

        Animator animator;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;


        void Awake() {
            animator = GetComponent<Animator>();
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        Weapon SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }


        void Start() =>
            currentWeapon.ForceInit();

        void Update()
        {                        
            timeSinceLastAttack += Time.deltaTime;
            MoveToAttackTarget();
        }


        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget() =>
            target;



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


        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return currentWeapon.value.GetDamage();
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return currentWeapon.value.GetPercentageBonus();
        }


        // Played via Animation event.
        public void Hit(GameObject combatTarget) {
            if (combatTarget == null)
                return;

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }


        // Sword and Unarmed weapons aren't calling this... 
        // Played via Animation event.
        public void Hit()
        {
            if (target == null)
                return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value.HasProjectile())
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);

            else
                // target.TakeDamage(gameObject, currentWeapon.GetDamage());
                target.TakeDamage(gameObject, damage);
        }
        

        void Shoot() => 
            Hit();

        
        public bool GetIsInRange()
        {
            if (target == null)
                return false;

            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
        }


        #region ~ SAVING ~

        public object CaptureState()
        {
            // if (currentWeapon.name == null)
            //     return null;

            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {   
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }



        #endregion
    }
}