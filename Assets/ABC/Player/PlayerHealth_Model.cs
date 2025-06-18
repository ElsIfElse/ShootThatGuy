using System.Collections;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth_Model : NetworkBehaviour
{
    int maxHealth = 100;
    public readonly SyncVar<int> currentHealth = new();

    [HideInInspector] public UnityEvent<int> gettingHitEvent = new();
    [HideInInspector] public UnityEvent<int> hpChangedEvent = new();

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            this.enabled = false;
        }        

        // StartCoroutine(Initialize_Late(0.05f));
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        currentHealth.Value = maxHealth;
    }
    public void OnHealthChanged(PlayerHealth_View view,int oldValue, int newValue, bool asServer)
    {
        Debug.Log($"Client {base.OwnerId} - Health changed to {newValue} - IsClient: {IsClientInitialized} - IsOwner: {IsOwner}");
        if (!IsClientInitialized) return;
        
        view.UpdateHealth(newValue);
    }
    public void TakeDamage(int damage)
    {
        Debug.Log("Being damaged. Incoming Damage Is: " + damage);
        currentHealth.Value -= damage;
        
        if(currentHealth.Value <= 0)
        {
            Debug.Log("Player is dead.");
        } 
    }
    public void GainHealth(int health)
    {
        Debug.Log("Being damaged. Incoming Damage Is: " + health);
        currentHealth.Value += health;
        
        if(currentHealth.Value >= 100)
        {
            currentHealth.Value = 100;
        } 
    }
}
