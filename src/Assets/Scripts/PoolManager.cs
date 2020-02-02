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
    [HideInInspector]
    public PickupPool PickupPool;

    public CollectiblePool ManaCollectiblePool;
    public CollectiblePool CoinCollectiblePool;

    public ParticleSystemPool AsteroidImpactParticlePool;
    public ParticleSystemPool AsteroidImpactCircleParticlePool;

    public void Initialize()
    {
        if (!InitializeSingleton(this)) return;

        TRexPool = GetComponentInChildren<TRexPool>();
        AsteroidPool = GetComponentInChildren<AsteroidPool>();
        PickupPool = GetComponentInChildren<PickupPool>();
    }

    public void PostInitialize() { }
}