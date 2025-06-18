using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class PlayerHealth_View : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] UnityEngine.UI.Image healthBar_Bar;
    [SerializeField] GameObject health_Oject;

    public void UpdateHealth(int currentHealth)
    {
        Debug.Log("Updating health text: " + currentHealth);
        this.hpText.text = currentHealth.ToString();
        this.healthBar_Bar.fillAmount = currentHealth / 100f;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
        {
            health_Oject.SetActive(false);
            this.enabled = false;
        }
    }
}
