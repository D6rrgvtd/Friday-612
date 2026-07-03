using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [SerializeField] float speedMax;
    PlayerInput playerInput;
    [SerializeField] float accel;
    [SerializeField] float rotatespeed;
    Rigidbody rb;
    private Vector3 rotateTarget;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var accelVec = playerInput.actions["Move"].ReadValue<Vector2>();

        var cameraDir = playerInput.camera.transform.forward;
        cameraDir.y = 0;
        cameraDir = cameraDir.normalized;

        var cameraRight = playerInput.camera.transform.right;

        var accelVec3D =
            cameraDir * accelVec.y * accel
            + cameraRight * accelVec.x * accel;
        rb.AddForce(accelVec3D, ForceMode.Acceleration);

        if (accelVec3D != Vector3.zero)
        {
            rotateTarget = accelVec3D.normalized;
        }
        Vector3 forward = transform.forward;
        transform.up = Vector3.up;
        transform.forward = Vector3.Slerp(forward,rotateTarget, rotatespeed * Time.deltaTime);

    }
}

