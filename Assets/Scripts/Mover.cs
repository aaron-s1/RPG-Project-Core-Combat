using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        
        NavMeshAgent navMeshAgent;
        Animator playerAnim;

        Ray lastRay;

        void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerAnim = GetComponent<Animator>();
        }


        void Update() =>
            UpdateAnimator();

        
        // Moves but first stops attacking.
        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        

        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination;
            navMeshAgent.isStopped = false;
            // playerAgent.destination = destination;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        

        void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;

            playerAnim.SetFloat("forwardSpeed", speed);
        }
    }
}