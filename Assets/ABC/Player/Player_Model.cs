using Unity.Cinemachine;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class Player_Model : MonoBehaviour
{
    private GameObject collider_Object;
    private GameObject visual_Object;
    private GameObject cameraHolder;
    private CinemachineCamera playerCamera;
    private CharacterController characterController;
    private CapsuleCollider playerCollider;

    private bool isGrounded = true;
    private Vector3 velocity = Vector3.zero;
    private float gravity = -9.81f;



    [Header("Movement Settings")]
    [Space(10)]
    [Header("Movement Speed")]
    [SerializeField] private float baseMovementSpeed;
    [SerializeField] private float walkMovementSpeed;
    [SerializeField] private float crouchMovementSpeed;
    [SerializeField] private float currentMovementSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpStrength;
    [SerializeField] private float inAirModifier = 1;
    [Header("Crouch Settings")]
    [SerializeField] private float characterBaseHeight = 1;
    [SerializeField] private float characterCrouchHeight = 0.5f;
    [SerializeField] private bool isCrouching = false;


    public float BaseMovementSpeed { get => baseMovementSpeed; set => baseMovementSpeed = value; }
    public float WalkMovementSpeed { get => walkMovementSpeed; set => walkMovementSpeed = value; }
    public float CrouchMovementSpeed { get => crouchMovementSpeed; set => crouchMovementSpeed = value; }
    public float CurrentMovementSpeed { get => currentMovementSpeed; set => currentMovementSpeed = value; }
    public float JumpStrength { get => jumpStrength; set => jumpStrength = value; }
    public float InAirModifier { get => inAirModifier; set => inAirModifier = value; }
    public float CharacterBaseHeight { get => characterBaseHeight; set => characterBaseHeight = value; }
    public float CharacterCrouchHeight { get => characterCrouchHeight; set => characterCrouchHeight = value; }
    public bool IsCrouching { get => isCrouching; set => isCrouching = value; }

    public GameObject Collider_Object { get => collider_Object; set => collider_Object = value; }
    public GameObject Visual_Object { get => visual_Object; set => visual_Object = value; }
    public GameObject CameraHolder { get => cameraHolder; set => cameraHolder = value; }
    public CinemachineCamera PlayerCamera { get => playerCamera; set => playerCamera = value; }
    public CharacterController CharacterController { get => characterController; set => characterController = value; }
    public CapsuleCollider PlayerCollider { get => playerCollider; set => playerCollider = value; }

    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public float VelocityY { get => velocity.y; set => velocity.y = value; }
    public float VelocityX { get => velocity.x; set => velocity.x = value; }
    public float VelocityZ { get => velocity.z; set => velocity.z = value; }
    public float Gravity { get => gravity; set => gravity = value; }

    public void GetReference_ColliderObject()
    {
        collider_Object = transform.Find("Collider").gameObject;
    }
    public void GetReference_VisualObject()
    {
        visual_Object = transform.Find("Visual").gameObject;
    }
    public void GetCameraHolderReference()
    {
        cameraHolder = transform.Find("CameraHolder").gameObject;
    }
    public void GetPlayerCameraReference()
    {
        playerCamera = GameObject.FindWithTag("CineCam").GetComponent<CinemachineCamera>();
    }
    public void GetCharacterControllerReference()
    {
        characterController = GetComponent<CharacterController>();
    }
    public void GetPlayerColliderReference()
    {
        GameObject playerBody = transform.Find("Body").gameObject;
        playerCollider = playerBody.GetComponent<CapsuleCollider>();
    }

    //
    
    public void SetMovementSpeed()
    {
        if(isCrouching)
        {
            currentMovementSpeed = crouchMovementSpeed;
        }
        else
        {
            currentMovementSpeed = walkMovementSpeed;
        }
    }

}
