using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using Unity.Cinemachine;
using Unity.VisualScripting;

public class Player_Controller : NetworkBehaviour
{
    Player_Model playerModel;
    Player_View playerView;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {

            GetReferences();
            NullReferenceCheck();
            playerModel.CurrentMovementSpeed = playerModel.WalkMovementSpeed;

            // TurnOffCollidersOnBody();
            TurnOffMeshRenderersOnBody();
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            gameObject.GetComponent<Player_Controller>().enabled = false;
        }

    }
    private void Update()
    {
        IsGrounded();
        MovePlayer();
        TurnPlayerWithCamera();

        //
        TriggerColorChange();
        Crouch();
        CrouchTrigger();

        if (base.IsOwner)
        {
            playerModel.SetMovementSpeed();
        }
    }
    void IsGrounded()
    {
        if (base.IsOwner)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 3f))
            {
                playerModel.IsGrounded = true;
                playerModel.InAirModifier = 1f;
            }
            else
            {
                playerModel.IsGrounded = false;
                playerModel.InAirModifier = 0.2f;
            }
        }
    }
    void MovePlayer()
    {
        if (base.IsOwner == false)
        {
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        playerModel.CharacterController.Move(move * playerModel.CurrentMovementSpeed * Time.deltaTime);

        // Jump input
        if (Input.GetButtonDown("Jump") && playerModel.IsGrounded)
        {
            // v = sqrt(h * -2 * g)
            playerModel.VelocityY = Mathf.Sqrt(playerModel.JumpStrength * -2f * playerModel.Gravity);
            playerModel.Velocity = playerModel.Velocity;

            if(playerModel.IsCrouching) playerModel.IsCrouching = false;
        }

        // Apply gravity over time
        playerModel.VelocityY += playerModel.Gravity * Time.deltaTime;

        // Apply vertical movement (falling/jumping)
        playerModel.CharacterController.Move(playerModel.Velocity * Time.deltaTime);
    }
    void TurnPlayerWithCamera()
    {
        if (base.IsOwner == false) return;

        Vector3 direction = playerModel.PlayerCamera.transform.forward;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void TurnOffCollidersOnBody()
    {
        BoxCollider[] colliders = playerModel.Collider_Object.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
        }
    }
    void TurnOffMeshRenderersOnBody()
    {
        MeshRenderer[] renderers = playerModel.Collider_Object.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

    public void GetReferences()
    {
        playerModel = GetComponent<Player_Model>();
        playerView = GetComponent<Player_View>();

        playerModel.Collider_Object = transform.Find("Collider").gameObject;
        playerModel.Visual_Object = transform.Find("Visual").gameObject;
        playerModel.CameraHolder = transform.Find("CameraHolder").gameObject;

        playerModel.PlayerCamera = GameObject.FindWithTag("CineCam").GetComponent<CinemachineCamera>();
        playerModel.PlayerCamera.Target.TrackingTarget = playerModel.CameraHolder.transform;
        playerModel.PlayerCamera.transform.SetParent(playerModel.CameraHolder.transform);

        playerModel.CharacterController = GetComponent<CharacterController>();
        playerModel.PlayerCollider = playerModel.Collider_Object.GetComponent<CapsuleCollider>();
    }
    void NullReferenceCheck()
    {
        if (playerModel == null) Debug.Log("playerModel is null");
        if (playerView == null) Debug.Log("playerView is null");
        if (playerModel.Collider_Object == null) Debug.Log("playerModel.Body is null");
        if (playerModel.CameraHolder == null) Debug.Log("playerModel.CameraHolder is null");
        if (playerModel.PlayerCamera == null) Debug.Log("playerModel.PlayerCamera is null");
    }







    // TEST
    // COLOR CHANGE
    [ObserversRpc]
    private void ChangeColor(GameObject player, Color color)
    {
        player.GetComponent<Player_Model>().GetReference_VisualObject();
        player.GetComponent<Player_Model>().Visual_Object.GetComponent<MeshRenderer>().material.color = color;
    }
    [ServerRpc]
    public void ChangeColorServerRpc(GameObject player, Color color)
    {
        ChangeColor(player, color);
    }
    void TriggerColorChange()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeColorServerRpc(gameObject, Color.red);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeColorServerRpc(gameObject, Color.green);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeColorServerRpc(gameObject, Color.blue);
        }
    }

    // CROUCH
    public void Crouch()
    {
        if (base.IsOwner)
        {
            if (playerModel.IsCrouching)
            {
                playerModel.CharacterController.height = playerModel.CharacterCrouchHeight;
                playerModel.PlayerCollider.height = playerModel.CharacterCrouchHeight;
            }
            else
            {
                playerModel.CharacterController.height = playerModel.CharacterBaseHeight;
                playerModel.PlayerCollider.height = playerModel.CharacterBaseHeight;
            }
        }
    }

    public void CrouchTrigger()
    {
        if (Input.GetKeyDown(KeyCode.C) && base.IsOwner)
        {
            if (!playerModel.IsGrounded) return;

            playerModel.IsCrouching = !playerModel.IsCrouching;
        }
    }

}
