using R3;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public Collider playerCollider { get; set; }

    [SerializeField] private float moveSpeed;

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
        if (isSeenPlayer)
        {
            var subVec = playerCollider.bounds.center - rb.position;
            subVec.y = 0;
            rb.linearVelocity = subVec.normalized * moveSpeed;
        }
    }
}
