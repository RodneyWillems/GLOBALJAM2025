using UnityEngine;

public class Trident : MonoBehaviour
{
    public PlayerMovement Player;

    private void Awake()
    {
        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Player.TridentHitSomething(collision.gameObject.transform.position);
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
