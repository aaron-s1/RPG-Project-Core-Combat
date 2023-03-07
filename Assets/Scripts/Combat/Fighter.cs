using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Weapon defaultWeapon = null;

        Weapon currentWeapon = null;

        Animator animator;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;


        void Start() {
            animator = GetComponent<Animator>();

            if (defaultWeapon != null)
                EquipWeapon(defaultWeapon);

            // if (defaultWeapon == null)
            //     EquipWeapon(defaultWeapon);
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


        public Health GetTarget() => target;


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

            if (currentWeapon.HasProjectile())
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);

            else
                target.TakeDamage(gameObject, currentWeapon.GetDamage());            
        }

        void Shoot() => 
            Hit();

        
        public bool GetIsInRange()
        {
            if (target == null)
                return false;

            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }


        public object CaptureState()
        {
            // if (currentWeapon.name == null)
            //     return null;

            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {   
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}