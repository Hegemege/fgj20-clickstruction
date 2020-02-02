using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRexController : MonoBehaviour, IResetableBehaviour
{
    [HideInInspector]
    public Vector3 TargetLocation;
    public float MovementSpeed = 15f;
    public float SwervingSpeed = 2.5f;
    public float SwervingAngle = 40f;

    private float _swervingTimer;

    public float SpawnDistanceFromOrigin;

    private Vector3 _direction;

    public Transform ModelRoot;

    public ParticleSystem StompParticleLeft;
    public ParticleSystem StompParticleRight;

    public void Initialize()
    {
        // Move to the opposite side of origin from target location, far enough away
        var direction = TargetLocation;
        direction.y = 0f;

        direction.Normalize();

        _direction = direction;

        transform.position = -_direction * SpawnDistanceFromOrigin;

        _swervingTimer = 0f;

        // Sound
        PoolManager.Instance.DinoSpawnAudio.GetPooledObject();
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        // If the trex has ran too far, return to pool
        if (Vector3.Distance(transform.position, Vector3.zero) > SpawnDistanceFromOrigin * 1.5f)
        {
            Reset();
        }
    }

    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        var velocity = _direction * MovementSpeed;

        // Add swerving
        _swervingTimer += dt;
        var swervePhase = Mathf.Sin(_swervingTimer * SwervingSpeed);
        var swerveRotation = Quaternion.AngleAxis(swervePhase * SwervingAngle, Vector3.up);
        velocity = swerveRotation * velocity;

        // Turn the object to always point forward
        ModelRoot.transform.LookAt(transform.position + velocity.normalized, Vector3.up);

        transform.Translate(velocity * dt);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DestructibleStaticTrigger")) return;

        var destructible = other.GetComponentInParent<Destructible>();
        destructible.Destruct();
    }

    public void StompLeft()
    {
        PoolManager.Instance.DinoStompAudio.GetPooledObject();
        StompParticleLeft.Play();
    }

    public void StompRight()
    {
        PoolManager.Instance.DinoStompAudio.GetPooledObject();
        StompParticleRight.Play();
    }
}
