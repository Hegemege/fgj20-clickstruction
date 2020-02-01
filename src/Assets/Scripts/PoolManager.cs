using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public class PoolManager : GenericManager<PoolManager>, ILoadedManager
{
    [HideInInspector]
    public TRexPool TRexPool;
    [HideInInspector]
    public AsteroidPool AsteroidPool;

    public void Initialize()
    {
        if (!InitializeSingleton(this)) return;

        TRexPool = GetComponentInChildren<TRexPool>();
        AsteroidPool = GetComponentInChildren<AsteroidPool>();
    }

    public void PostInitialize() { }
}