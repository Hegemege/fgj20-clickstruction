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
    [HideInInspector]
    public QuicksandPool QuicksandPool;

    public CollectiblePool ManaCollectiblePool;
    public CollectiblePool CoinCollectiblePool;

    public ParticleSystemPool AsteroidImpactParticlePool;
    public ParticleSystemPool AsteroidImpactCircleParticlePool;

    public AudioPool AsteroidSpawnAudio;
    public AudioPool AsteroidImpactAudio;
    public AudioPool CoinPickupAudio;

    public AudioPool DestructionBigAudio;

    public AudioPool DestructionSmallAudio;

    public AudioPool DinoSpawnAudio;

    public AudioPool DinoStompAudio;

    public AudioPool ManaCollectAudio;

    public AudioPool PowerupPickupAudio;

    public AudioPool RepairAudio;

    public void Initialize()
    {
        if (!InitializeSingleton(this)) return;

        TRexPool = GetComponentInChildren<TRexPool>();
        AsteroidPool = GetComponentInChildren<AsteroidPool>();
        PickupPool = GetComponentInChildren<PickupPool>();
        QuicksandPool = GetComponentInChildren<QuicksandPool>();
    }

    public void PostInitialize() { }

    public void ResetPools()
    {
        TRexPool.ResetPool();
        AsteroidPool.ResetPool();
        QuicksandPool.ResetPool();
        PickupPool.ResetPool();
        ManaCollectiblePool.ResetPool();
        CoinCollectiblePool.ResetPool();
        AsteroidImpactCircleParticlePool.ResetPool();
        AsteroidImpactParticlePool.ResetPool();
    }
}