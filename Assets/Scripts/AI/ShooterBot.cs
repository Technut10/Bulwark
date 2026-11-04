using UnityEngine;
using UnityEngine.AI;
using UnityEngine.WSA;
[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class ShooterBot : MonoBehaviour
{

    private enum State {Chase, Attack, Retreat }

    [Header("Target")]
    [SerializeField] private Transform target;
    
    [Header("Ranges")]
    [Tooltip("Close enough to start shooting.")]
    [SerializeField] private float attackRange = 12f;
    [Tooltip("Too close: back off.")]
    [SerializeField] private float retreatRange = 5f;


    [Header("Movement")]
    [SerializeField] private float repathInterval = 0.25f;
    [SerializeField] private float retreatDistance = 4f;


    [Header("Shooting")]
    [SerializeField] private float fireInterval = 1f;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;


    private NavMeshAgent agent;
    private State currentState = State.Chase;
    private float repathTimer;
    private float fireTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        // Same as ChaserBot: find the Player if no target was assigned.
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning($"{name}: no object tagged Player found.");
            }
        }
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        // Step 1: Decide
        UpdateState(distance);

        switch (currentState)
        {
            case State.Chase: Chase(); break;
            case State.Attack: Attack(); break;
            case State.Retreat: Retreat(); break;
        }

        // Step 2: Act

        switch (currentState)
        {
            case State.Chase: Chase(); break;
            case State.Attack: Attack(); break;
            case State.Retreat: Retreat(); break;
        }
    }

    private void UpdateState(float distance)
    {
        // TODO 1: Set currentState based on distance:
        // closer than retreatRange -> Retreat
        // farther than attackRange -> Chase
        // anything in between -> Attack

        if (distance <= retreatRange)
        {
            currentState = State.Retreat;
        }
        else if (distance >= attackRange)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Attack;
        }
    }
      

    private void Chase()
    {
        agent.isStopped = true;
        // TODO 2: Make sure the agent is allowed to move,
        // then use the same repath countdown as ChaserBot.
        if (target == null) return;
        Vector3 position = target.position;
        
        repathTimer -= Time.deltaTime; 

        if (repathTimer <= 0f)
        {
            agent.SetDestination(position);
            repathTimer = repathInterval;
        }
        agent.isStopped = false;
    }

    private void Attack()
    {
        // TODO 3: Stop the agent, turn to face the target, and count down fireTimer. When it hits zero, Fire() and reset it.
        agent.isStopped = true;

        // Direction FROM this bot TO the player
        Vector3 direction = target.position - transform.position;

        // Stay upright, don't tilt
        direction.y = 0f;

        transform.rotation = Quaternion.LookRotation(direction);


        Quaternion.LookRotation(target.position);
        fireTimer -= Time.deltaTime;
        if(fireTimer <= 0)
        {
            Fire();
            fireTimer = fireInterval;
        }
  
      
      
    }

    private void Retreat()
    {
        // TODO 4: Make sure the agent can move, then send it to a point retreatDistance away from the target, in the direction pointing from the target to this bot. 
        agent.isStopped = false;
        Vector3 away = (transform.position - target.position).normalized;
        agent.SetDestination((transform.position + away) * retreatDistance);

    }
    
    private void Fire()
    {
        Debug.Log($"{name}: Bang!");

        Projectile projectile  = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);


        Vector3 direction = (target.position + Vector3.up * 1f) - firePoint.position;

        projectile.Launch(direction, gameObject);
        

    }
}
