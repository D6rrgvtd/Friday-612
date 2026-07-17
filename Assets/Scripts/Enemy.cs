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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isSeenPlayer = true;
        var direction = playerCollider.bounds.center - rb.position;
        if(Physics.Raycast(rb.position, direction.normalized,out var hitInfo))
        {
            //プレイヤー以外の障害物に当たった場合は見えない、
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
