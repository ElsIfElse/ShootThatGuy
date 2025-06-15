using Unity.Cinemachine;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class Player_Model : MonoBehaviour
{
    private GameObject body;
    private GameObject cameraHolder;
    private CinemachineCamera playerCamera;
    private CharacterController characterController;

    private bool isGrounded = true;
    private Vector3 velocity = Vector3.zero;
    private float gravity = -9.81f;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpStrength;

    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float JumpStrength { get => jumpStrength; set => jumpStrength = value; }

    public GameObject Body { get => body; set => body = value; }
    public GameObject CameraHolder { get => cameraHolder; set => cameraHolder = value; }
    public CinemachineCamera PlayerCamera { get => playerCamera; set => playerCamera = value; }
    public CharacterController CharacterController { get => characterController; set => characterController = value; }

    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public float VelocityY { get => velocity.y; set => velocity.y = value; }
    public float VelocityX { get => velocity.x; set => velocity.x = value; }
    public float VelocityZ { get => velocity.z; set => velocity.z = value; }
    public float Gravity { get => gravity; set => gravity = value; }
}
