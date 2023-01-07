using UnityEngine;
using RPG.Movement;

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
            if (target == null) {
                print("no target.");
                return;
            }

            bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;

            if (!isInRange) {
                GetComponent<Mover>().MoveTo(target.position);
                return;
            }

            GetComponent<Mover>().StopMovement();
        }


        public void AssignAttackTarget(CombatTarget combatTarget) =>
            target = combatTarget.transform;
    }    
}