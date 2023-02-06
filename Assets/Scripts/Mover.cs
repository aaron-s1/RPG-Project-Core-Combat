using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        
        
        
        NavMeshAgent navMeshAgent;
        Health health;
        Animator playerAnim;

        Ray lastRay;

        void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerAnim = GetComponent<Animator>();
            health = GetComponent<Health>();
        }


        void Update()
        {
            if (health.IsDead() && navMeshAgent.enabled)
                navMeshAgent.enabled = false;
            
            UpdateAnimator();
        }

        
        // Moves but first stops attacking.
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }
        

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }


        public void Cancel() {
            if (navMeshAgent.enabled)
                navMeshAgent.isStopped = true;
        }


        void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;

            playerAnim.SetFloat("forwardSpeed", speed);
        }

        #region Saving.
        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;

            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
        #endregion
    }
}