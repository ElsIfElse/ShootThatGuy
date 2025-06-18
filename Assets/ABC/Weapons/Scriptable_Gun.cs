using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/Gun")]
public class Scriptable_Gun : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float range;
    public float fireRate;
    public float reloadTime;
    public int ammo;
}
