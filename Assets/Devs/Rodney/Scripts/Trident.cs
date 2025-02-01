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
            Player.TridentHitSomething(collision.gameObject.transform.position, true);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Bomb"))
        {
            Player.TridentHitSomething(Vector3.zero, false);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            Player.TridentHitSomething(Vector3.zero, false);
        }
        Destroy(gameObject);
    }
}
