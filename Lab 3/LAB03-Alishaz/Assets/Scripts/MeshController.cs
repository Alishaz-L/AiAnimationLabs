using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeshController : MonoBehaviour
{
    //GameObject that the agent will move towards
    public GameObject Target;
    private NavMeshAgent agent;

    //determine if the agent is currently walking
    bool isWalking = true;
    private Animator animator;

    //Initializes the NavMeshAgent and Animator components on start
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    //Updates the agent's destination based on its walking state
    void Update()
    {
        if (isWalking)
        {
            agent.destination = Target.transform.position; 
        }
        else
        {
            agent.destination = transform.position; 
        }
    }

    // Triggers when the agent enters a collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Dragon")
        {
            isWalking = false; 
            animator.SetTrigger("Attacking"); 
        }
    }

    // Triggers when the agent exits a collider
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Dragon")
        {
            isWalking = true; 
            animator.SetTrigger("Walking"); 
        }
    }
}
