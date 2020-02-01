using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public class PoolManager : GenericManager<PoolManager>, ILoadedManager
{
    [HideInInspector]
    public TRexPool TRexPool;

    public void Initialize()
    {
        if (!InitializeSingleton(this)) return;

        TRexPool = GetComponentInChildren<TRexPool>();
    }

    public void PostInitialize() { }
}