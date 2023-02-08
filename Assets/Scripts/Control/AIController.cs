using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;

        [SerializeField] PatrolPath patrolPath;

        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        GameObject player;
        Health health;
        Mover mover;
        Fighter fighter;

        Vector3 guardPosition;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;


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
                AttackBehavior();

            else if (timeSinceLastSawPlayer < suspicionTime)
                SuspicionBehavior();

            else
                PatrolBehavior();


            UpdateTimers();
        }


        void UpdateTimers() {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }


        void AttackBehavior() {
            timeSinceLastSawPlayer = 0;
            fighter.Hit(player);
        }


        void SuspicionBehavior() =>
            GetComponent<ActionScheduler>().CancelCurrentAction();


        #region ~ PATROL BEHAVIOR ~

        void PatrolBehavior() {

            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                    CycleWaypoint();

                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }


        bool AtWaypoint() {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }


        void CycleWaypoint() {
            timeSinceArrivedAtWaypoint = 0;
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }


        Vector3 GetCurrentWaypoint() =>
            patrolPath.GetWaypoint(currentWaypointIndex);

        #endregion ~ PATROL BEHAVIOR ~


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
