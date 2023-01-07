using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;

        void Update()
        {
            MoveToAttackTarget();
        }


        void MoveToAttackTarget()
        {
            if (target == null)
                return;

            if (!GetIsInRange())
                GetComponent<Mover>().MoveTo(target.position);

            else
                GetComponent<Mover>().StopMovement();
        }


        bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }


        public void AttackTarget(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }


        public void CancelAttack() =>
            target = null;
    }    
}