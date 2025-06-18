using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using FishNet.Component.Transforming;
using System.Collections.Generic;
using TMPro;

public class CharacterController_Test : NetworkBehaviour
{
    List<NetworkObject> objs = new List<NetworkObject>();
    public NetworkObject objectToSpawn;

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(SpawnWithDelay(1f));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MoveObj_01();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            MoveObj_02();
        }
    }

    void SpawnObject()
    {
        NetworkObject netObj = Instantiate(objectToSpawn);

        if (netObj.GetComponent<NetworkObject>() == null)
        {
            Debug.Log("No NetworkObject component found! Adding one.");
            netObj.gameObject.AddComponent<NetworkObject>();
        }
        if (netObj.GetComponent<NetworkTransform>() == null)
        {
            Debug.Log("No NetworkTransform component found! Adding one.");
            netObj.gameObject.AddComponent<NetworkTransform>();
        }

        netObj.transform.position = new Vector3(0, 0, 0);
        Spawn(netObj);
        objs.Add(netObj);
    }

    IEnumerator SpawnWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        for(int i = 0; i < 10; i++)
        {
            SpawnObject();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void MoveObj_01()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].transform.position += new Vector3(i * 2, 0, 0);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void MoveObj_02()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].transform.position += new Vector3(0, 0, 1);
        }
    }

    

}
