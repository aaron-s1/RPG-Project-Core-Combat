using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;

        GameObject player;
        Health health;
        Mover mover;
        Fighter fighter;

        Vector3 guardPosition;

        float timeSinceLastSawPlayer = Mathf.Infinity;


        void Start() {
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }


        void Update()
        {
            if (health.IsDead())
                return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehavior();
            }

            else if (timeSinceLastSawPlayer < suspicionTime)
                SuspicionBehaviour();

            else
                GuardBehaviour();


            timeSinceLastSawPlayer += Time.deltaTime;
        }


        void AttackBehavior() =>
            fighter.Attack(player);


        void SuspicionBehaviour() =>
            GetComponent<ActionScheduler>().CancelCurrentAction();


        void GuardBehaviour() =>
            mover.StartMoveAction(guardPosition);


        bool InAttackRangeOfPlayer() {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return (distanceToPlayer < chaseDistance);
        }


        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }    
}
