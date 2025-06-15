using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using System.Collections;
using Unity.Cinemachine;

public class CharacterController_Test : NetworkBehaviour
{
    CinemachineCamera playerCamera;
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpStrength;
    bool isGrounded = true;
    CharacterController characterController;

    GameObject body;
    GameObject cameraHolder;
    private Vector3 velocity;
    float gravity = -9.81f;
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            body = transform.Find("Body").gameObject;
            cameraHolder =  transform.Find("CameraHolder").gameObject;

            playerCamera = GameObject.FindWithTag("CineCam").GetComponent<CinemachineCamera>();
            playerCamera.Target.TrackingTarget = cameraHolder.transform;
            playerCamera.transform.SetParent(cameraHolder.transform);

            TurnOffCollidersOnBody();
            TurnOffMeshRenderersOnBody();
        }
        else
        {
            gameObject.GetComponent<CharacterController_Test>().enabled = false;
        }
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        MovePlayer();
        IsGrounded();
        TurnPlayerWithCamera();
    }

    void IsGrounded()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 3f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
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
        characterController.Move(move * movementSpeed * Time.deltaTime);

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpStrength * -2f * gravity);
        }

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement (falling/jumping)
        characterController.Move(velocity * Time.deltaTime);
    }

    void TurnPlayerWithCamera()
    {
        if (base.IsOwner == false) return;
        
        Vector3 direction = playerCamera.transform.forward;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void TurnOffCollidersOnBody()
    {
        BoxCollider[] colliders = body.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
        }
    }
    void TurnOffMeshRenderersOnBody()
    {
        MeshRenderer[] renderers = body.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
