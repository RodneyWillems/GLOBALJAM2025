using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBubble : MonoBehaviour
{
    [SerializeField]
    private int m_scoreToAdd;

    [SerializeField]
    private int m_bossHealth = 10;

    [SerializeField]
    private GameObject m_bossBubblePrefab;

    private bool m_destroyedByPlayer;
    private List<GameObject> m_spawnedBubbleList;
    private float m_spawnTime = 5;

    private PlayerMovement m_playerControls;


    private void Awake()
    {
        m_playerControls = FindFirstObjectByType<PlayerMovement>();
        m_spawnedBubbleList = new();
        SpawnBombs();
        transform.LookAt(m_playerControls.transform.position);
    }

    #region Spawner
    private void SpawnBombs()
    {
        int spawnBombs = Random.Range(5, 10);
        for (int i = 0; i < spawnBombs; i++)
        {
            Vector3 randomPos = Random.insideUnitSphere * 5 + transform.position;
            Instantiate(GameManager.Instance.m_bombPrefab, randomPos, Quaternion.identity);
        }
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(m_spawnTime);
        SpawnBubbles();
    }

    private void SpawnBubbles()
    {
        StopCoroutine(Timer());
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPos = Random.insideUnitSphere * 5 + transform.position;
            GameObject SpawnedBubble = Instantiate(m_bossBubblePrefab, randomPos, Quaternion.identity);
            m_spawnedBubbleList.Add(SpawnedBubble);
        }
    }
    #endregion

    #region OnHit
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(m_spawnedBubbleList.Count > 0)
            {
                foreach (GameObject bubble in m_spawnedBubbleList)
                {
                    Destroy(bubble);
                }
            }


            m_bossHealth--;
            
            if (m_bossHealth <= 0)
            {
                m_destroyedByPlayer = true;
                if (!GameManager.Instance.m_endless)
                {
                    GameManager.Instance.m_winScreen.SetActive(true);
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    GameManager.Instance.BossDeath();
                }

                Destroy(gameObject);

            }
            transform.LookAt(m_playerControls.transform.position);
            SpawnBombs();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Bomb") && !other.gameObject.CompareTag("Bubble"))
        {
            Destroy(other.gameObject);
        }

        else
        {
            other.transform.position += transform.forward * 5;
        }

    }
    

    private void OnDestroy()
    {
        if (m_destroyedByPlayer)
        {
            GameManager.Instance.AddScore(m_scoreToAdd);
        }
    }
    #endregion
}
