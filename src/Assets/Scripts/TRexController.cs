using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRexController : MonoBehaviour, IResetableBehaviour
{
    [HideInInspector]
    public Vector3 TargetLocation;
    public float MovementSpeed = 15f;

    public float SpawnDistanceFromOrigin;

    private Vector3 _direction;

    public Transform ModelRoot;

    public void Initialize()
    {
        // Move to the opposite side of origin from target location, far enough away
        var direction = TargetLocation;
        direction.y = 0f;

        direction.Normalize();

        _direction = direction;

        transform.position = -_direction * SpawnDistanceFromOrigin;
    }

    public void Reset()
    {

    }

    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        var velocity = _direction * MovementSpeed;

        // Turn the object to always point forward
        ModelRoot.transform.LookAt(transform.position + _direction, Vector3.up);

        transform.Translate(velocity * dt);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DestructibleStaticTrigger")) return;

        Debug.Log("Kill " + other);
        var destructible = other.GetComponentInParent<Destructible>();
        destructible.Destruct();
    }
}
