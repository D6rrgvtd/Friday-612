using R3;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Collider playerCollider { get; set; }
    [SerializeField] int hp = 2;
    [SerializeField] private float moveSpeed;
    public float invincibleTime;
    [SerializeField] float invincibleTimeMax = 0.5f;
    [SerializeField] float knockbackSpeed = 5;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (playerCollider == null)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        bool isSeenPlayer = true;
        var direction = playerCollider.bounds.center - rb.position;

       
        Vector3 rayStartPos = rb.position + direction.normalized * 0.5f;

       
        float rayLength = direction.magnitude - 0.5f;

        if (Physics.Raycast(rayStartPos, direction.normalized, out var hitInfo, rayLength))
        {
            if (hitInfo.collider != playerCollider)
            {
                isSeenPlayer = false;
            }
        }
    

        if (isSeenPlayer && invincibleTime <= 0)
        {
            var subVec = playerCollider.bounds.center - rb.position;
            subVec.y = 0;
            rb.linearVelocity = subVec.normalized * moveSpeed;
        }

        if (invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        var attackObj = collision.gameObject.GetComponent<AttackObj>();
        if (attackObj != null && invincibleTime <= 0)
        {
            hp -= attackObj.power;
            var dir = transform.position - collision.transform.position;
            dir.y = 0;
            var krockbackVec = dir.normalized * knockbackSpeed;
            rb.linearVelocity = krockbackVec;
            invincibleTime = invincibleTimeMax;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
