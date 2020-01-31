using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float RepairDuration = 2f;

    private float _repairTimer;
    private bool _intact = true;
    private List<Rigidbody> DestructibleParts;

    void Awake()
    {
        DestructibleParts = GetComponentsInChildren<Rigidbody>().ToList();
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
        // Release all rigidbodies and apply a small explosion on all of them
        foreach (var part in DestructibleParts)
        {
            part.isKinematic = false;

            var explosionStrength = Random.Range(5f, 10f);
            part.AddExplosionForce(explosionStrength, transform.position, 2f, 0f, ForceMode.Impulse);

            // Also add a random direction force
            var randomForceDirection = Random.onUnitSphere;
            var randomForceStrength = Random.Range(1f, 2f);
            part.AddForce(randomForceStrength * randomForceDirection, ForceMode.Impulse);
        }
    }

    public void StartRepair()
    {

    }

    public void StopRepair()
    {

    }
}
