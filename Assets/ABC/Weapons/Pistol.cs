using FishNet.Object;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Pistol : Gun
{
    public override void Fire(Weapon script)
    {
        Debug.Log("Fire is fired");
        if (!OutOfAmmo)
        {
            Debug.Log("Player has ammo. Shooting Raycast");
            if (!CanShoot) return;
            if (IsReloading) return;
            CanShoot = false;

            script.effectManager.CreateEffect(EffectTypes.EffectType.MuzzleFlash, MuzzleEffectPosition.transform.position);

            StartCoroutine(this.FireRateTimer());
            this.DecreaseCurrentAmmo();
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 1000f);

            Shooting(hit.point, hit.transform.gameObject.GetComponentInParent<NetworkObject>());
        }
        else
        {
            Reload();
            Debug.Log("Out of ammo. Reloading...");
        }
    }

    [ServerRpc]
    void Shooting(Vector3 hitPoint,NetworkObject target)
    {
        // Debug.Log("SHOOTING_SCRIPT: Bullet hit: " + target.name);
        
        if (target == null)
        {
            Debug.Log("Bullet hit nothing or something without a NetworkObject");
            effectManager.CreateEffect(EffectTypes.EffectType.BulletOnSurface, hitPoint);
            return;
        }
        if (target.gameObject.transform.Find("Collider").gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            // Debug.Log("Bullet hit player");
            
            if (target.transform.gameObject.GetComponentInParent<NetworkObject>() == null) Debug.Log("NetworkObject is null");
            if (effectManager == null) Debug.Log("EffectManager is null");

            target.GetComponent<PlayerHealth_Controller>().TakeDamage(Damage);
            effectManager.CreateEffect(EffectTypes.EffectType.BloodOnEnemyHit, hitPoint);
            return;
        }
        else
        {
            Debug.Log("Bullet hit nothing");
            return;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            this.enabled = false;
        }
    }
}
