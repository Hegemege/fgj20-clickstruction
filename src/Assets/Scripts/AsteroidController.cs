using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour, IResetableBehaviour
{
    [HideInInspector]
    public Vector3 TargetLocation;
    public float MovementSpeed = 40f;
    public float Acceleration = 2000f;

    public float SpawnHeight;
    public float SpawnRadius;
    public float ExplosionRadius;
    public LayerMask ExplosionLayerMask;

    private Vector3 _direction;

    private bool _exploded;
    private Rigidbody _rigidBody;

    public Transform ModelRoot;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Initialize()
    {
        // Move to the sky to random spot
        var randomPoint = Random.onUnitSphere;
        randomPoint.y = SpawnHeight;
        randomPoint.x *= SpawnRadius;
        randomPoint.z *= SpawnRadius;

        transform.position = randomPoint;

        var direction = TargetLocation - transform.position;
        direction.Normalize();

        _direction = direction;

        _rigidBody.velocity = Vector3.zero;
        _rigidBody.AddForce(_direction.normalized * MovementSpeed, ForceMode.Impulse);

        _exploded = false;
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        _rigidBody.AddForce(_direction * Acceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("StaticEnvironment")) return;
        if (_exploded) return;

        _exploded = true;

        // Check all destructibles in radius and destroy them
        var collisions = Physics.OverlapSphere(transform.position, 3f, ExplosionLayerMask, QueryTriggerInteraction.Collide);

        foreach (var collision in collisions)
        {
            if (!collision.CompareTag("DestructibleStaticTrigger")) continue;

            var destructible = collision.gameObject.GetComponentInParent<Destructible>();
            destructible.Destruct();
        }

        // TODO: Spawn effets

        Reset();
    }

}
