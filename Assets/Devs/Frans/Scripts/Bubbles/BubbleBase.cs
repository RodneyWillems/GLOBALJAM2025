using UnityEngine;

public class BubbleBase : MonoBehaviour
{
    [SerializeField]
    protected int m_scoreToAdd;

    protected bool m_destroyedByPlayer;

    protected float m_lerpTime = 5;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_destroyedByPlayer = true;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_destroyedByPlayer)
        {
            GameManager.Instance.AddScore(m_scoreToAdd);
        }
    }
}
