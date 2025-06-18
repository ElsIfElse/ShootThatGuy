using UnityEngine;
using FishNet.Object;
using System.Threading.Tasks;
using System.Collections;
using FishNet.Object.Synchronizing;
public class PlayerHealth_Controller : NetworkBehaviour
{
    PlayerHealth_Model model;
    PlayerHealth_View view;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            this.enabled = false;
        }

        model = GetComponent<PlayerHealth_Model>();
        view = GetComponent<PlayerHealth_View>();

        model.currentHealth.OnChange += (prev, next, asServer) => model.OnHealthChanged(view, prev, next, asServer);

        StartCoroutine(DelayedHealthInit());

    }

    [ServerRpc (RequireOwnership = false)]
    public void TakeDamage(int damage)
    {
        Debug.Log("Taking damage. Incoming Damage Is: " + damage);
        model.TakeDamage(damage);
        view.UpdateHealth(model.currentHealth.Value);
    }
    [ServerRpc (RequireOwnership = false)]
    public void GainHealth(int health)
    {
        Debug.Log("Gaining health. Current Health Is: "+ (model.currentHealth.Value + health));
        model.GainHealth(health);
        view.UpdateHealth(model.currentHealth.Value);
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
    }
    private IEnumerator DelayedHealthInit()
    {
        // Wait one frame to let FishNet finish syncing
        yield return new WaitForSeconds(0.01f);
        view.UpdateHealth(model.currentHealth.Value);
    }
}
