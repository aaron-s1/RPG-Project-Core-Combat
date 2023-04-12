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
        //
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] WeaponConfig defaultWeapon = null;

        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        Animator animator;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;


        void Awake() {
            animator = GetComponent<Animator>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }


        void Start() {
            currentWeapon.ForceInit();
        }

        void Update()
        {                        
            timeSinceLastAttack += Time.deltaTime;
            MoveToAttackTarget();
        }


        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        Weapon AttachWeapon(WeaponConfig weapon)
        {
            animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);            
        }

        public Health GetTarget() =>
            target;



        void MoveToAttackTarget()
        {
            if (target == null || target.IsDead())
                return;

            if (!GetIsInRange(target.transform)) 
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
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
                !GetIsInRange(combatTarget.transform))
            {
                return false;
            }

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
                yield return currentWeaponConfig.GetDamage();
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return currentWeaponConfig.GetPercentageBonus();
        }


        // Played via Animation event.
        // THIS IS WHAT SWORD CALLS?
        public void Hit(GameObject combatTarget) {
            if (combatTarget == null)
                return;

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();

            currentWeapon.value.OnHit();
        }


        // Sword and Unarmed weapons aren't calling this... 
        // Played via Animation event.
        public void Hit()
        {
            if (target == null)
                return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
                currentWeapon.value.OnHit();

            if (currentWeaponConfig.HasProjectile())
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);

            else
                // target.TakeDamage(gameObject, currentWeapon.GetDamage());
                target.TakeDamage(gameObject, damage);
                
            currentWeapon.value.OnHit();
        }
        

        void Shoot() => 
            Hit();

        
        public bool GetIsInRange(Transform targetTransform)
        {
            if (target == null)
                return false;

            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }


        #region ~ SAVING ~

        public object CaptureState()
        {
            // if (currentWeapon.name == null)
            //     return null;

            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {   
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }



        #endregion
    }
}