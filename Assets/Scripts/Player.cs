using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float accel;
    [SerializeField] float rotateSpeed;
    [SerializeField] Animator animator;
    [SerializeField] float jumpSpeed;
    [SerializeField] float groundNormalYMin = 0.7f;
    [SerializeField] float groundDamping = 8f;
    [SerializeField] float airDamping = 0.5f;
    [SerializeField] GameObject firePrefab;
    [SerializeField] float fireSpeed;
    [SerializeField] Vector3 fireOffset;

    [SerializeField] int hp = 5;
    public float invincibleTime;
    [SerializeField] float invincibleTimeMax = 0.5f;
    [SerializeField] float knockbackSpeed = 5;

    PlayerInput playerInput;
    Rigidbody rb;
    Vector3 rotateTarget;
    bool isGrounded = true;

    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = -1;

        rotateTarget = transform.forward;
        rotateTarget.y = 0;
        rotateTarget.Normalize();
    }

    private void FixedUpdate()
    {
        
        if (isGrounded)
        {
            rb.linearDamping = groundDamping;
        }
        else
        {
            rb.linearDamping = airDamping;
        }

        
        isGrounded = false;
    }


    void Update()
    {
        if (invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }
        if (isGrounded)
        {
            var accelVec = playerInput.actions["Move"].ReadValue<Vector2>();

            var cameraForwardHorizontal = playerInput.camera.transform.forward;
            cameraForwardHorizontal.y = 0;
            cameraForwardHorizontal.Normalize();

            var cameraRightHorizontal = playerInput.camera.transform.right;
            cameraRightHorizontal.y = 0;
            cameraRightHorizontal.Normalize();

            var accelVec3D = cameraForwardHorizontal * accelVec.y * accel
                             + cameraRightHorizontal * accelVec.x * accel;
            rb.AddForce(accelVec3D, ForceMode.Acceleration);


            if (accelVec3D != Vector3.zero)
            {
                rotateTarget = accelVec3D.normalized;
            }
        }


       var currentForward = transform.forward;
        currentForward.y = 0;
        currentForward.Normalize();

        if (rotateTarget != Vector3.zero)
        {
            Vector3 nextForward = Vector3.Slerp(currentForward, rotateTarget, rotateSpeed * Time.deltaTime);
            if (nextForward != Vector3.zero)
            {
                // LookRotationを使って、上方向（Vector3.up）を固定したまま回転を適用
                transform.rotation = Quaternion.LookRotation(nextForward, Vector3.up);
            }
        }

        


        Vector3 velocityXZ = rb.linearVelocity;
        velocityXZ.y = 0;
        animator.SetFloat("MoveSpeed", velocityXZ.magnitude);


        if (playerInput.actions["Jump"].WasPressedThisFrame() && isGrounded)
        {
            Vector3 jumpVec = new Vector3(0, jumpSpeed, 0);
            rb.AddForce(jumpVec, ForceMode.VelocityChange);
        }

        if (playerInput.actions["Attack"].WasPressedThisFrame())
        {
            var position = transform.position + transform.TransformVector(fireOffset);
            var fireObj = Object.Instantiate(firePrefab, position, transform.rotation);
            var fireRB = fireObj.GetComponent<Rigidbody>();
            if (fireRB != null)
            {
                fireRB.linearVelocity = transform.forward * fireSpeed;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
        var attackObj = collision.gameObject.GetComponent<AttackObj>();

        foreach (var contact in collision.contacts)
        {
            
            if (attackObj == null && contact.normal.y >= groundNormalYMin)
            {
                isGrounded = true;
            }
        }

        
        if (attackObj != null && invincibleTime <= 0)
        {
            hp -= attackObj.power;

        
            var dir = transform.position - collision.transform.position;
            dir.y = 0;

            
            if (dir == Vector3.zero)
            {
                dir = -transform.forward;
            }

            var krockbackVec = dir.normalized * knockbackSpeed;

          
            isGrounded = false;
            rb.linearDamping = airDamping;

           
            rb.linearVelocity = new Vector3(krockbackVec.x, jumpSpeed * 0.5f, krockbackVec.z); // 少し上に浮き上がらせると綺麗に飛びます

            invincibleTime = invincibleTimeMax;

            Debug.Log($"プレイヤーがダメージを受けました！ 残りHP: {hp}"); // ログで確認用

            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


}
