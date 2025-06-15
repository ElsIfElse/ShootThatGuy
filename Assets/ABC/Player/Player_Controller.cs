using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using Unity.Cinemachine;

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

            TurnOffCollidersOnBody();
            TurnOffMeshRenderersOnBody();
        }
        else
        {
            gameObject.GetComponent<Player_Controller>().enabled = false;
        }

    }
    void Update()
    {
        IsGrounded();
        MovePlayer();
        TurnPlayerWithCamera();
    }

    void IsGrounded()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 3f))
        {
            playerModel.IsGrounded = true;
        }
        else
        {
            playerModel.IsGrounded = false;
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
        playerModel.CharacterController.Move(move * playerModel.MovementSpeed * Time.deltaTime);

        // Jump input
        if (Input.GetButtonDown("Jump") && playerModel.IsGrounded)
        {
            // v = sqrt(h * -2 * g)
            playerModel.VelocityY = Mathf.Sqrt(playerModel.JumpStrength * -2f * playerModel.Gravity);
            playerModel.Velocity = playerModel.Velocity;
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
        BoxCollider[] colliders = playerModel.Body.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
        }
    }
    void TurnOffMeshRenderersOnBody()
    {
        MeshRenderer[] renderers = playerModel.Body.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

    void GetReferences()
    {
            playerModel = GetComponent<Player_Model>();
            playerView = GetComponent<Player_View>();

            playerModel.Body = transform.Find("Body").gameObject;
            playerModel.CameraHolder = transform.Find("CameraHolder").gameObject;

            playerModel.PlayerCamera = GameObject.FindWithTag("CineCam").GetComponent<CinemachineCamera>();
            playerModel.PlayerCamera.Target.TrackingTarget = playerModel.CameraHolder.transform;
            playerModel.PlayerCamera.transform.SetParent(playerModel.CameraHolder.transform);

            playerModel.CharacterController = GetComponent<CharacterController>();
    }
    void NullReferenceCheck()
    {
        if (playerModel == null) Debug.Log("playerModel is null");
        if (playerView == null) Debug.Log("playerView is null");
        if (playerModel.Body == null) Debug.Log("playerModel.Body is null");
        if (playerModel.CameraHolder == null) Debug.Log("playerModel.CameraHolder is null");
        if (playerModel.PlayerCamera == null) Debug.Log("playerModel.PlayerCamera is null");
    }
}
