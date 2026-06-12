using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [SerializeField] float speedMax;
    PlayerInput playerInput;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        var moveVec = playerInput.actions["Move"].ReadValue<Vector2>();
        var cameraDir = playerInput.camera.transform.forward;
        var cameraRight = playerInput.camera.transform.right;
        var moveVec3D = cameraDir * moveVec.y * speedMax + cameraRight *moveVec.x * speedMax;
    }
}
