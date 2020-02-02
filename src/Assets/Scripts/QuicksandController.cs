using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuicksandController : MonoBehaviour, IResetableBehaviour
{
    private ParticleSystem _dust;

    void Awake()
    {
        _dust = GetComponentInChildren<ParticleSystem>();
    }

    public void Initialize()
    {
        _dust.Play();
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(6f);
        Reset();
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }
}
