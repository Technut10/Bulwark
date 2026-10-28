using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class ChaserBot : TestDummy
{
    [SerializeField] private Transform target;
    [SerializeField] private float repathInterval = 0.25f;
    [SerializeField] private float stopDistance = 2f;

    private NavMeshAgent agent;
    private float repathTimer;

    private void Awake()
    {
        // TODO 1: Get the NavMeshAgent component and store it in 'agent'.
        agent = GetComponent<NavMeshAgent>();


        // TODO 2: Set the agent's stopping distance so it doesn't walk into you.


    }

    private void Start()
    {
        // TODO 3: If no target was assigned in the Inspector, find the object tagged "Player" and use its transform.

    }

    private void Update()
    {
        // TODO 4: If there's no target, do nothing.
        // TODO 5: Count repathTimer down by the time since last frame. When it reaches zero, send the agent to the target's position and reset the timer.
    }

}
