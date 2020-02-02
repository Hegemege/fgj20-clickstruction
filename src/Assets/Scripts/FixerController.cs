using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Transform ModelRoot;

    private Quaternion _targetModelRotation;
    public float RotationSmoothing;

    public float BootsMultiplier = 0.25f;

    // Props
    public float MovementSpeed
    {
        get
        {
            return _baseMovementSpeed * (1f + GameManager.Instance.CollectedBoots * BootsMultiplier);
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

        // Get Input
        HandleMovement();
        HandleRepair();
    }

    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        if (!CanMove)
        {
            return;
        }


        var velocity = new Vector3(_inputDirection.x, 0f, _inputDirection.y) * MovementSpeed;

        // Turn the object to always point forward
        // Smoothed
        if (velocity.magnitude > 0f)
        {
            _targetModelRotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
        }

        ModelRoot.rotation = Quaternion.Slerp(ModelRoot.rotation, _targetModelRotation, RotationSmoothing);

        transform.Translate(velocity * dt);
    }

    void OnTriggerEnter(Collider other)
    {
        // Pickup coins
        if (other.CompareTag("CoinCollectible"))
        {
            PickupCoin(other);
        }

        // Powerups
        if (other.CompareTag("Pickup"))
        {
            PickupPowerup(other);
        }
    }

    private void PickupCoin(Collider other)
    {
        var coin = other.GetComponentInParent<Collectible>();
        coin.Reset();

        GameManager.Instance.PickupCoin();
    }

    private void PickupPowerup(Collider other)
    {
        var powerup = other.GetComponentInParent<PickupController>();
        if (GameManager.Instance.SpendCoins(powerup.Cost))
        {
            switch (powerup.Type)
            {
                case PickupType.Boots:
                    GameManager.Instance.CollectedBoots += 1;
                    break;
                case PickupType.Wrench:
                    GameManager.Instance.CollectedWrenches += 1;
                    break;
                case PickupType.Shield:
                    break;
            }
            powerup.Reset();
        }
    }

    // Input System Events
    public void HandleMovement()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        _inputDirection = new Vector2(inputX, inputY);
    }

    public void HandleRepair()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            State = PlayerState.Repairing;
            OnStartRepair();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            State = PlayerState.Idle;
            OnStopRepair();
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
