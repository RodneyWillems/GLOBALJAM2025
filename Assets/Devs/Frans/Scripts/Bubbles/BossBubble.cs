using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBubble : MonoBehaviour
{
    [SerializeField]
    private int m_scoreToAdd;

    [SerializeField]
    private int m_bossHealth = 10;

    private bool m_destroyedByPlayer;
    private GameObject[] m_spawnPoints;
    private List<GameObject> m_spawnedBubbleList;
    private float m_spawnTime = 5;

    private PlayerMovement m_playerControls;


    private void Awake()
    {
        m_playerControls = FindFirstObjectByType<PlayerMovement>();
        SpawnBombs();
    }

    private void SpawnBombs()
    {

        for (int i = 0; i < m_spawnPoints.Length; i++)
        {
            Vector3 randomPos = Random.insideUnitSphere * 5;
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
        int randomBubble = Random.Range(0, GameManager.Instance.m_bubblePrefab.Length);
        for (int i = 0; i < m_spawnPoints.Length; i++)
        {
            Vector3 randomPos = Random.insideUnitSphere * 5;
            GameObject SpawnedBubble = Instantiate(GameManager.Instance.m_bubblePrefab[randomBubble], randomPos, Quaternion.identity);
            m_spawnedBubbleList.Add(SpawnedBubble);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach(GameObject bubble in m_spawnedBubbleList)
            {
                Destroy(bubble);
            }

            m_bossHealth--;
            
            if (m_bossHealth <= 0)
            {
                m_destroyedByPlayer = true;
                Destroy(gameObject);
            }
            transform.LookAt(m_playerControls.transform.position);
            SpawnBombs();
        }
    }

    private void OnDestroy()
    {
        if (m_destroyedByPlayer)
        {
            GameManager.Instance.AddScore(m_scoreToAdd);
        }
    }
}
