using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float RepairBaseDuration = 2f;

    public Transform DestructiblePartRoot;

    private float _repairTimer; // Represents "health" of the animation. Does not reset if repairing is stopped
    [HideInInspector]
    public bool Intact = true;
    private List<DestructiblePart> DestructibleParts;
    private Coroutine _repairCoroutine;

    public int FixRewardCoins = 8;

    public float WrenchBoostSeconds = 0.5f;
    public float MinimumRepairTime = 0.3f;

    public bool Repairable = true;

    private float _repairAudioTimer = 0f;
    private float _repairAudioInterval = 0.5f;

    public float RepairLength
    {
        get
        {
            var duration = RepairBaseDuration - GameManager.Instance.CollectedWrenches * WrenchBoostSeconds;
            return Mathf.Clamp(duration, MinimumRepairTime, RepairBaseDuration);
        }
    }

    void Awake()
    {
        DestructibleParts = GetComponentsInChildren<DestructiblePart>().ToList();
        foreach (var part in DestructibleParts)
        {
            part.Parent = this;
        }
    }

    void Start()
    {
        //GameManager.Instance.Destructibles.Add(this);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            Destruct();
        }
#endif
    }

    public void Destruct(bool spawnMana = true, bool playSound = true, bool force = false)
    {
        if (!Intact && !force) return;

        Intact = false;

        if (Repairable)
        {
            if (spawnMana)
            {
                GameManager.Instance.PickupMana();
            }
            if (playSound)
            {
                PoolManager.Instance.ManaCollectAudio.GetPooledObject();
            }
        }

        if (playSound)
        {
            if (Repairable)
            {
                PoolManager.Instance.DestructionBigAudio.GetPooledObject();
            }
            else
            {
                PoolManager.Instance.DestructionSmallAudio.GetPooledObject();
            }
        }

        // Release all rigidbodies and apply a small explosion on all of them
        foreach (var part in DestructibleParts)
        {
            part.Release(transform.position);
        }
    }

    // Control interface
    public void StartRepair()
    {
        _repairTimer = 0f;
        _repairAudioTimer = 0f;

        // Stop physics on the parts and animate them towards
        foreach (var part in DestructibleParts)
        {
            part.StartReturnToOriginal();
        }

        _repairCoroutine = StartCoroutine(DoRepair());
    }

    private IEnumerator DoRepair()
    {
        while (_repairTimer < RepairLength)
        {
            _repairTimer += Time.deltaTime;

            _repairAudioTimer -= Time.deltaTime;
            if (_repairAudioTimer < 0f)
            {
                _repairAudioTimer = _repairAudioInterval;
                PoolManager.Instance.RepairAudio.GetPooledObject();
            }

            var t = Mathf.Clamp01(_repairTimer / RepairLength);

            // Animate parts back to where they came from
            foreach (var part in DestructibleParts)
            {
                part.UpdateReturnToOriginal(t);
            }

            yield return null;
        }

        EndRepair();
    }

    public void StopRepair()
    {
        // If the part was already repaired, don't do anything
        if (Intact) return;

        if (_repairCoroutine != null)
        {
            StopCoroutine(_repairCoroutine);
        }

        foreach (var part in DestructibleParts)
        {
            part.InterruptReturnToOriginal();
        }
    }

    private void EndRepair()
    {
        _repairTimer = 0f;
        Intact = true;
        foreach (var part in DestructibleParts)
        {
            part.Reset();
        }

        // Spawn coin collectibles
        for (var i = 0; i < FixRewardCoins; i++)
        {
            var coin = PoolManager.Instance.CoinCollectiblePool.GetPooledObject();

            // Move coin slightly up
            coin.gameObject.transform.position = transform.position + Vector3.up * 1.5f;

            coin.component.Initialize();

            // Add random force
            var randomDirection = Random.onUnitSphere;
            randomDirection.y = Mathf.Abs(randomDirection.y) / 2f;

            coin.component.RigidBody.AddForce(randomDirection * 7.5f, ForceMode.Impulse);
        }

    }
}
