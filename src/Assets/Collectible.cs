using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, IResetableBehaviour
{
    public Transform ModelRoot;
    public float RotationSpeed = 250f;
    public float HoverHeight = 2f;
    public float HoverSpeed = 2f;

    private float _hoverTimer;
    private Vector3 _startModelPosition;

    void Awake()
    {
        _startModelPosition = ModelRoot.position;

        // Add randomness to rotation
        ModelRoot.RotateAround(transform.position, Vector3.up, Random.Range(0f, 360f));
    }

    public void Initialize()
    {
        _hoverTimer = 0f;
    }

    public void Reset()
    {

    }

    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        // Rotation
        ModelRoot.RotateAround(transform.position, Vector3.up, RotationSpeed * dt);

        // Hover
        _hoverTimer += dt;
        var hoverEffect = (Mathf.Sin(_hoverTimer * HoverSpeed) + 1f) / 2f;
        ModelRoot.transform.position = _startModelPosition + Vector3.up * hoverEffect;
    }
}
