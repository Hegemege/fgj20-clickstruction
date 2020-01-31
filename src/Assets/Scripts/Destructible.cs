using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float RepairDuration = 2f;

    public Transform DestructiblePartRoot;

    private float _repairTimer; // Represents "health" of the animation. Does not reset if repairing is stopped
    [HideInInspector]
    public bool Intact = true;
    private List<DestructiblePart> DestructibleParts;
    private Coroutine _repairCoroutine;

    void Awake()
    {
        DestructibleParts = GetComponentsInChildren<DestructiblePart>().ToList();
        foreach (var part in DestructibleParts)
        {
            part.Parent = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Destruct();
        }
    }

    public void Destruct()
    {
        Intact = false;

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

        // Stop physics on the parts and animate them towards
        foreach (var part in DestructibleParts)
        {
            part.StartReturnToOriginal();
        }

        _repairCoroutine = StartCoroutine(DoRepair());
    }

    private IEnumerator DoRepair()
    {
        while (_repairTimer < RepairDuration)
        {
            _repairTimer += Time.deltaTime;

            var t = Mathf.Clamp01(_repairTimer / RepairDuration);

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
    }
}
