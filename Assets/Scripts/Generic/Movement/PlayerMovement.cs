// Author: Harry Donovan
// Based off of 'PlayerMovement.cs' script from https://github.com/HDonovan96/Glass-Nomad.

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float movementSpeed = 10.0f;
    public float sprintMultiplier = 2.0f;
    public float groundDelta;

    [SerializeField]
    protected float lookSensitivity = 1.0f;
    [SerializeField]
    protected float jumpForce = 10.0f;

    [SerializeField]
    // Acts as a vertical clamp on the camera to stop the player looking under themselves.
    protected float yLookClamp = 70.0f;

    // Used to check if the player is grounded.
    // This is used for stopping the player jumping mid air.
    protected float distGround;
    protected Rigidbody charRigidBody;
    protected Collider charCollider;

    // Used to keep track of the local players camera.
    // All other cameras in the scene must be disabled.
    protected Camera charCamera;

    // Stores raw mouse input.
    protected float camerRotation = 0.0f;

    // Used when calculating camera movement from mouse.
    protected Quaternion CharCameraTargetRotation;

    protected void Start()
    {
        
        // Sets the object name in heirarchy to the players net name.
        //gameObject.name = photonView.Owner.Nickname;

        charCamera = gameObject.GetComponentInChildren<Camera>();
        charCollider = gameObject.GetComponent<Collider>();
        charRigidBody = gameObject.GetComponent<Rigidbody>();

        distGround = charCollider.bounds.extents.y;

        // Locks the cursor to the centre of the screen.
        Cursor.lockState = CursorLockMode.Locked;
        // Hides cursor.
        Cursor.visible = false;

        // All cameras that aren't the clients are disabled.
        if (!photonView.IsMine)
        {
            charCamera.enabled = false;
        }

        CharCameraTargetRotation = charCamera.transform.localRotation;
    }

    protected void Update()
    {
        // If the player isn't the client returns.
        if (!photonView.IsMine)
        {
            return;
        }

        HandleUnlockingMouse();

        HandleCameraRotation();

        Vector3 movementInput = GetMovementInput();

        charRigidBody.velocity = transform.TransformDirection(movementInput);
    }

    private Vector3 GetMovementInput()
    {
        float x;
        float y;
        float z;

        // If the player is on the ground and has pressed space.
        if (Input.GetAxisRaw("Jump") != 0 && IsGrounded(Vector3.up))
        {
            y = jumpForce;
        }
        else
        {
            y = charRigidBody.velocity.y;
        }

        x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        z = Input.GetAxisRaw("Vertical") * movementSpeed;

        if (Input.GetAxisRaw("Sprint") != 0)
        {
            x *= sprintMultiplier;
            z *= sprintMultiplier;
        }

        return new Vector3(x, y, z);
    }

    private bool IsGrounded(Vector3 normal)
    {
        return Physics.Raycast(transform.position, -normal, distGround + groundDelta);
    }

    private void HandleCameraRotation()
    {
        // Fetches camera movement.
        Vector3 mouseInput = MouseInput();

        // Rotates the player based on x-axis mouse movement.
        Vector3 playerRotation = new Vector3(0.0f, mouseInput.x, 0.0f) * lookSensitivity;
        transform.Rotate(playerRotation);

        // Holds camera rotation before being clamped.
        CharCameraTargetRotation *= Quaternion.Euler(-mouseInput.y * lookSensitivity, 0.0f, 0.0f);

        CharCameraTargetRotation = ClampRotationAroundXAxis(CharCameraTargetRotation);

        // Updates camera rotation.
        charCamera.transform.localRotation = CharCameraTargetRotation;
    }

    private void HandleUnlockingMouse()
    {
        // Allows the cursor to be unlocked within the unity editor.
        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        #elif UNITY_STANDALONE_WIN
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        #endif
    }

    private Vector3 MouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        return new Vector3(mouseX, mouseY, 0.0f);
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        // Quaternion is 4*1 matrix.
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        // Clamps view angle between +-yLookClamp
        angleX = Mathf.Clamp(angleX, -yLookClamp, yLookClamp);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}