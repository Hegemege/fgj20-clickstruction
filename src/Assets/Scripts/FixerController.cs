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
    public PlayerState State = PlayerState.Idle;

    private Vector2 _inputDirection;
    private Destructible _currentRepairTarget;

    public Transform ModelRoot;

    private Quaternion _targetModelRotation;
    public float RotationSmoothing;

    public float BootsMultiplier = 0.25f;

    public float QuicksandMultiplier = 0.5f;
    private bool _inQuicksand;

    public Animator Animator;

    // Props
    public float MovementSpeed
    {
        get
        {
            if (_inQuicksand)
            {
                return _baseMovementSpeed * QuicksandMultiplier;
            }
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

    private bool _oldRunState;

    void Awake()
    {

    }

    void Update()
    {
        // Update State for moving/idle
        if (CanMove)
        {
            //State = _inputDirection.magnitude > 0f ? PlayerState.Moving : PlayerState.Idle;
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

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Quicksand"))
        {
            _inQuicksand = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Quicksand"))
        {
            _inQuicksand = false;
        }
    }

    private void PickupCoin(Collider other)
    {
        var coin = other.GetComponentInParent<Collectible>();
        coin.Reset();

        GameManager.Instance.PickupCoin();
        PoolManager.Instance.CoinPickupAudio.GetPooledObject();
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
            powerup.transform.position += Vector3.up * 50f;
            PoolManager.Instance.PowerupPickupAudio.GetPooledObject();
        }

    }

    // Input System Events
    public void HandleMovement()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        _inputDirection = new Vector2(inputX, inputY);

        if (_inputDirection.magnitude > 0f && State != PlayerState.Idle)
        {
            State = PlayerState.Moving;
            Animator.SetTrigger("StartRun");
        }

        if (_inputDirection.magnitude < 0.01f && State == PlayerState.Moving)
        {
            State = PlayerState.Idle;
            Animator.SetTrigger("Idle");
        }
    }

    public void HandleRepair()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Animator.SetTrigger("StartRepair");
            State = PlayerState.Repairing;
            OnStartRepair();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Animator.SetTrigger("Idle");
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
            if (destructible.Repairable == false) continue;

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
