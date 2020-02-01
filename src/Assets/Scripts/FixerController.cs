using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtilities;

public class FixerController : MonoBehaviour
{
    // Inspector (public / serialized)
    public float RepairRadius = 2f;
    public LayerMask RepairableLayerMask;

    [SerializeField]
    private float _baseMovementSpeed; 

    // Hidden / privates
    [HideInInspector]
    public PlayerState State = PlayerState.Idle;

    private Vector2 _inputDirection;
    private Destructible _currentRepairTarget;

    // Props
    public float MovementSpeed
    {
        get
        {
            return _baseMovementSpeed;
        }
    }

    public bool CanMove
    {
        get
        {
            return State != PlayerState.Repairing && State != PlayerState.Stunned;
        }
    }

    void Awake()
    {

    }

    void Update()
    {
        // Update State for moving/idle
        if (CanMove)
        {
            State = _inputDirection.magnitude > 0f ? PlayerState.Moving : PlayerState.Idle;
        }
    }

    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        if (!CanMove)
        {
            return;
        }

        var velocity = new Vector3(_inputDirection.x, 0f, _inputDirection.y) * MovementSpeed;

        transform.Translate(velocity * dt);
    }

    // Input System Events
    public void OnMovement(InputAction.CallbackContext ctx)
    {
        _inputDirection = ctx.ReadValue<Vector2>();
    }

    public void OnRepair(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
                State = PlayerState.Repairing;
                OnStartRepair();
                break;
            case InputActionPhase.Canceled:
                State = PlayerState.Idle;
                OnStopRepair();
                break;
        }
    }

    private void OnStartRepair()
    {
        Destructible repairTarget = null;

        // Find close-by destructible and start repairing it
        var colliders = Physics.OverlapSphere(transform.position, RepairRadius, RepairableLayerMask, QueryTriggerInteraction.Collide);
        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("DestructibleStaticTrigger")) continue;

            var destructible = collider.GetComponentInParent<Destructible>();
            if (destructible == null || destructible.Intact) continue;

            repairTarget = destructible;
        }

        if (repairTarget == null) return;

        _currentRepairTarget = repairTarget;
        repairTarget.StartRepair();
    }

    private void OnStopRepair()
    {
        if (_currentRepairTarget != null)
        {
            _currentRepairTarget.StopRepair();
        }

        _currentRepairTarget = null;
    }
}
