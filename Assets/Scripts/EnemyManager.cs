using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Collider playerCollider;
    [SerializeField] Enemy[] enemies;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        foreach(var enemy in enemies)
        {
            enemy.playerCollider = playerCollider;
        }
    }
}
