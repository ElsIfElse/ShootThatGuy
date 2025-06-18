using UnityEngine;

public class DesertEagle : Pistol
{
    public override void Update()
    {
        if(base.IsOwner == false) return;
        
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse is clicked");
            Fire(gameObject.GetComponent<Pistol>());
        }
    }
    public override void Start()
    {
        base.Start();
    }
}
