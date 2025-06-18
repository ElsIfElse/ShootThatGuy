using System.Collections;
using System.Runtime.InteropServices;
using FishNet.Component.Prediction;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Gun : Weapon
{
    float fireRange;
    float fireRate;
    float reloadTime;
    int ammo;
    [SerializeField] int currentAmmo;
    bool outOfAmmo;
    bool isReloading;
    bool canShoot;
    [SerializeField] Scriptable_Gun gunData;
    [SerializeField] GameObject muzzleEffectPosition;


    public GameObject MuzzleEffectPosition { get => muzzleEffectPosition; set => muzzleEffectPosition = value; }
    public float FireRange { get => fireRange; set => fireRange = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public int Ammo { get => ammo; set => ammo = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public bool OutOfAmmo { get => outOfAmmo; set => outOfAmmo = value; }
    public bool IsReloading { get => isReloading; set => isReloading = value; }
    public bool CanShoot { get => canShoot; set => canShoot = value; }

    public void InitializeData()
    {
        FireRange = gunData.range;
        FireRate = gunData.fireRate;
        ReloadTime = gunData.reloadTime;
        Ammo = gunData.ammo;
        CurrentAmmo = ammo;
        Damage = gunData.damage;

        OutOfAmmo = false;
        IsReloading = false;
        CanShoot = true;
    }
    public void DecreaseCurrentAmmo()
    {
        CurrentAmmo--;

        if (CurrentAmmo <= 0)
        {
            CurrentAmmo = 0;
            OutOfAmmo = true;
        }
    }
    public void Reload()
    {
        if (!IsReloading && CurrentAmmo != Ammo && base.IsOwner)
        {
            isReloading = true;
            StartCoroutine(ReloadTimer());
        }
    }

    public void ReloadTrigger()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(ReloadTime);
        CurrentAmmo = ammo;
        isReloading = false;
        OutOfAmmo = false;

        Debug.Log("Reloaded. Current ammo is: " + CurrentAmmo);
    }
    public IEnumerator FireRateTimer()
    {
        yield return new WaitForSeconds(FireRate);
        CanShoot = true;
    }

    public override void Start()
    {
        base.Start();
        // InitializeData();
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        InitializeData();
    }
    
    virtual public void Update()
    {
        ReloadTrigger();
    }
}
