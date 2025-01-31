using UnityEngine;

public class Bubbles : MonoBehaviour
{
    [SerializeField]
    private int m_scoreToAdd;

    private bool m_destroyedByPlayer;

    private float m_lerpTime = 5;

    private void Start()
    {

    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 5 , Time.deltaTime / m_lerpTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            m_destroyedByPlayer = true;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_destroyedByPlayer){
            GameManager.Instance.AddScore(m_scoreToAdd);
        }
    }
}
