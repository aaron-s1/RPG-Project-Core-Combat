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


    void Update()
    {
        if (Input.GetMouseButton(0))
            MoveToCursor();

        UpdateAnimator();
    }
    

    void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
            playerAgent.destination = hit.point;
            // playerAgent.destination = target.position;
    }

    void UpdateAnimator()
    {
        Vector3 velocity = playerAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float speed = localVelocity.z;

        playerAnim.SetFloat("forwardSpeed", speed);
    }
}