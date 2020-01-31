using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePart : MonoBehaviour
{
    [HideInInspector]
    public Destructible Parent;

    [HideInInspector]
    public Rigidbody Rigidbody;

    [HideInInspector]
    public bool Attached = true;

    private Vector3 _originalRelativePosition;
    private Quaternion _originalRelativeRotation;

    private Vector3 _repairStartRelativePosition;
    private Quaternion _repairStartRelativeRotation;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();

        _originalRelativePosition = transform.localPosition;
        _originalRelativeRotation = transform.localRotation;
    }

    public void Release(Vector3 explosionPosition)
    {
        Rigidbody.isKinematic = false;

        var explosionStrength = Random.Range(5f, 10f);
        Rigidbody.AddExplosionForce(explosionStrength, explosionPosition, 2f, 0f, ForceMode.Impulse);

        // Also add a random direction force and a slight upward force
        var randomForceDirection = Random.onUnitSphere;
        var randomForceStrength = Random.Range(1f, 2f);
        Rigidbody.AddForce(randomForceStrength * randomForceDirection, ForceMode.Impulse);
        Rigidbody.AddForce(randomForceStrength * Vector3.up, ForceMode.Impulse);

        Attached = false;
        transform.parent = null;
    }

    public void StartReturnToOriginal()
    {
        Rigidbody.isKinematic = true;
        transform.parent = Parent.transform;

        _repairStartRelativePosition = transform.localPosition;
        _repairStartRelativeRotation = transform.localRotation;
    }

    public void UpdateReturnToOriginal(float t)
    {
        var positionT = GameManager.Instance.PartRepairPositionAnimation.Evaluate(t);
        var rotationT = GameManager.Instance.PartRepairRotationAnimation.Evaluate(t);

        transform.localPosition = Vector3.Lerp(_repairStartRelativePosition, _originalRelativePosition, positionT);
        transform.localRotation = Quaternion.Lerp(_repairStartRelativeRotation, _originalRelativeRotation, rotationT);
    }

    public void InterruptReturnToOriginal()
    {
        Rigidbody.isKinematic = false;
        transform.parent = null;
    }

    public void Reset()
    {
        Attached = true;
        Rigidbody.isKinematic = true;
        transform.parent = Parent.transform;
        transform.localPosition = _originalRelativePosition;
        transform.localRotation = _originalRelativeRotation;
    }
}
