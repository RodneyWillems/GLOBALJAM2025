using UnityEngine;

public class BubbleBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_VFX;

    [SerializeField]
    protected int m_scoreToAdd;

    [SerializeField]
    protected float m_lerpTime = 5;

    protected bool m_destroyedByPlayer;




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
            Instantiate(m_VFX, transform.position, Quaternion.identity);
            GameManager.Instance.AddScore(m_scoreToAdd);
        }
    }
}
