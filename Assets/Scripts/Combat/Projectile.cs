using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(Health))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 4f;

    private Vector3 direction;
    private GameObject owner;

    public GameObject Owner => owner;

    /// <summary>
    /// Called by whoever fires the projectile.
    /// </summary>

    public void Launch(Vector3 newDirection, GameObject newOwner)
    {
        // TODO 1: Store the direction (normalized) and the owner.
        // setting transform.forward = to direction; makes an object face a direction.
        //  Also make the projectile face its direction.
        direction = newDirection.normalized;
        transform.forward = direction;
        owner = newOwner;


        // TODO 2: Destroy this projectile after 'lifetime' seconds. so missed shots don't pile up forever.
        // using gameObject here means "the object this script is attached to.".
        Destroy(gameObject, lifetime);

    }

    /// <summary>
    /// Used by the Polarity Shield later.
    /// </summary>
    public void Reflect(Vector3 newDirection, GameObject newOwner)
    {
        direction = newDirection.normalized;
        owner = newOwner;
        transform.forward = direction;
    }

    // Update is called once per frame
    private void Update()
    {
        // TODO 3: Move along 'direction' at 'speed' units per second.
        // This is simiar to the movement code: direction x speed x Time.deltaTime
        transform.position += speed * Time.deltaTime * direction;
       
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO 4: If we hit our own owner, ignore it.
        if (owner != null && other.transform.IsChildOf(owner.transform))
        {
            return;
        }
        // TODO 5: If the thing we hit has Health, damage it.
        Health health = other.GetComponentInParent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage, owner);
        }
        // TODO 6: Destroy the projectile (whether it hit a bot, the player, or a wall).
        Destroy(gameObject);
    }
}
