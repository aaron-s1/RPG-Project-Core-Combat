using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    public NavMeshAgent playerAgent;
    public Animator playerAnim;

    Ray lastRay;

    void Awake() {
        playerAgent = GetComponent<NavMeshAgent>();
        playerAnim = GetComponent<Animator>();
    }


    void Update() =>
        UpdateAnimator();
    

    public void MoveTo(Vector3 destination)
    {
        GetComponent<NavMeshAgent>().destination = destination;
        // playerAgent.destination = destination;
    }

    void UpdateAnimator()
    {
        Vector3 velocity = playerAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float speed = localVelocity.z;

        playerAnim.SetFloat("forwardSpeed", speed);
    }
}