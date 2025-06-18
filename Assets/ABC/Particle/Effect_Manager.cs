using System.Collections;
using System.Collections.Generic;
using FishNet.Component.Transforming;
using FishNet.Object;
using UnityEngine;

public class Effect_Manager : NetworkBehaviour
{
    // public List<ParticleSystem> particleSystems = new();
    public List<NetworkObject> pool_Muzzle = new();
    public List<NetworkObject> pool_HitOnEnvironment = new();
    public List<NetworkObject> pool_BloodOnHit = new();

    public NetworkObject muzzleFlash;
    public NetworkObject hitOnEnvironment;
    public NetworkObject bloodOnHit;

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(CreateEffectsWithDelay(20, 1f));
    }

    [ServerRpc(RequireOwnership = false)]
    public void CreateEffect(EffectTypes.EffectType effectType, Vector3 position)
    {
        switch (effectType)
        {
            case EffectTypes.EffectType.MuzzleFlash:
                if (pool_Muzzle.Count == 0)
                {
                    Debug.Log("Pool was empty, creating more with buffer");
                    CreateEffect_WithBuffer(pool_Muzzle.Count * 2,muzzleFlash, pool_Muzzle);
                }

                NetworkObject muzzle_Obj = DequeueEffect(pool_Muzzle);

                if (muzzle_Obj == null)
                {
                    Debug.Log("Pooled Effect is null");
                    return;
                }

                muzzle_Obj.transform.position = position;
                StartCoroutine(DelayedPlay(muzzle_Obj, 0.05f));
                StartCoroutine(WaitForEffectToFinishAndEnqueue(muzzle_Obj, pool_Muzzle));
                break;

            case EffectTypes.EffectType.BulletOnSurface:
                if (pool_HitOnEnvironment.Count == 0)
                {
                    Debug.Log("Pool was empty, creating more with buffer");
                    CreateEffect_WithBuffer(pool_HitOnEnvironment.Count * 2,hitOnEnvironment, pool_HitOnEnvironment);
                }

                NetworkObject hitOnEnvironment_Obj = DequeueEffect(pool_HitOnEnvironment);

                if (hitOnEnvironment_Obj == null)
                {
                    Debug.Log("Pooled Effect is null");
                    return;
                }

                hitOnEnvironment_Obj.transform.position = position;
                StartCoroutine(DelayedPlay(hitOnEnvironment_Obj, 0.05f));
                StartCoroutine(WaitForEffectToFinishAndEnqueue(hitOnEnvironment_Obj, pool_HitOnEnvironment));
                break;

            case EffectTypes.EffectType.BloodOnEnemyHit:
                if (pool_BloodOnHit.Count == 0)
                {
                    Debug.Log("Pool was empty, creating more with buffer");
                    CreateEffect_WithBuffer(pool_BloodOnHit.Count * 2,bloodOnHit, pool_BloodOnHit);
                }

                NetworkObject bloodOnEnemyHit_Obj = DequeueEffect(pool_BloodOnHit);

                if (bloodOnEnemyHit_Obj == null)
                {
                    Debug.Log("Pooled Effect is null");
                    return;
                }

                bloodOnEnemyHit_Obj.transform.position = position;
                StartCoroutine(DelayedPlay(bloodOnEnemyHit_Obj, 0.05f));
                StartCoroutine(WaitForEffectToFinishAndEnqueue(bloodOnEnemyHit_Obj, pool_BloodOnHit));
                break;
        }
    }

    [ObserversRpc(BufferLast = false, ExcludeOwner = true, ExcludeServer = false)]
    private void PlayEffect(NetworkObject obj)
    {
        obj.GetComponent<ParticleSystem>().Play();
    }

    private void CreateEffect_AtStart(int amount, NetworkObject effect, List<NetworkObject> pool)
    {
        for (int i = 0; i < amount; i++)
        {
            NetworkObject obj = Instantiate(effect);
            // obj.gameObject.AddComponent<NetworkTransform>();
            obj.GetComponent<ParticleSystem>().Stop();
            pool.Add(obj);
            Spawn(obj);
        }
    }
    private void CreateEffect_WithBuffer(int size,NetworkObject effect, List<NetworkObject> pool)
    {
        for (int i = 0; i < size; i++)
        {
            NetworkObject obj = Instantiate(effect);
            obj.gameObject.AddComponent<NetworkTransform>();
            obj.GetComponent<ParticleSystem>().Stop();
            pool.Add(obj);
            Spawn(obj);
        }
    }
    private void EnqueueEffect(NetworkObject source, List<NetworkObject> particleSystems)
    {
        particleSystems.Add(source);
    }
    private NetworkObject DequeueEffect(List<NetworkObject> particleSystems)
    {
        if (particleSystems.Count == 0)
        {
            Debug.LogError("Tried to dequeue from an empty pool!");
            return null;
        }

        NetworkObject source = particleSystems[0];

        if (source == null)
        {
            Debug.LogError("ParticleSystem in pool is null!");
        }

        particleSystems.RemoveAt(0);
        return source;
    }

    private IEnumerator WaitForEffectToFinishAndEnqueue(NetworkObject source, List<NetworkObject> particleSystems)
    {
        ParticleSystem particleSystem = source.GetComponent<ParticleSystem>();
        yield return new WaitWhile(() => particleSystem.isPlaying);
        EnqueueEffect(source, particleSystems);
    }
    private IEnumerator CreateEffectsWithDelay(int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        CreateEffect_AtStart(amount, muzzleFlash, pool_Muzzle);
        CreateEffect_AtStart(amount, hitOnEnvironment, pool_HitOnEnvironment);
        CreateEffect_AtStart(amount, bloodOnHit, pool_BloodOnHit);
    }
    private IEnumerator DelayedPlay(NetworkObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayEffect(obj); // RPC to all clients
    }

}
