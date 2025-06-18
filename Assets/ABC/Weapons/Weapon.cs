using UnityEngine;
using FishNet.Object;

public abstract class Weapon : NetworkBehaviour
{
    int damage;
    string weaponName;
    public abstract void Fire(Weapon script);
    [HideInInspector] public Effect_Manager effectManager;

    public string WeaponName { get => weaponName; set => weaponName = value; }
    public int Damage { get => damage; set => damage = value; }

    public virtual void Start()
    {
        effectManager = GameObject.FindWithTag("EffectManager").GetComponent<Effect_Manager>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        
    }
}
