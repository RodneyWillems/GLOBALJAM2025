using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject m_bubblePrefab, m_bossBubblePrefab, m_pipePrefab;

    private List<GameObject> m_pipeSpawns;
    private GameObject m_spawnedPipes;
    private Coroutine m_bubbleSpawner;

    private bool m_isPaused;
    private bool m_bossSpawned;

    private float m_wait;

    private int m_amountOfPipes = 10;
    private int m_score;



    private void Awake()
    {
        Instance = this;
        m_wait = 0.5f;
        m_pipeSpawns = new();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        while (m_pipeSpawns.Count < m_amountOfPipes)
        {
            Vector3 randomPos = Random.insideUnitSphere * 10;
            randomPos.y = 0;
            Collider[] colliderOverlap = Physics.OverlapSphere(randomPos, 3);

            if(colliderOverlap.Length == 0)
            {
                m_spawnedPipes = Instantiate(m_pipePrefab, new Vector3(randomPos.x, 0, randomPos.z), Quaternion.identity);
                m_pipeSpawns.Add(m_spawnedPipes);
            }
        }

        m_bubbleSpawner = StartCoroutine(BubblesSpawner());
    }

    private void BossSpawner()
    {
        m_bossSpawned = true;
        Instantiate(m_bossBubblePrefab);

    }

    private IEnumerator BubblesSpawner()
    {
        while (!m_bossSpawned)
        {
            int randomPipe = Random.Range(0, m_pipeSpawns.Count);
            Instantiate(m_bubblePrefab, m_pipeSpawns.ElementAt(randomPipe).transform.position, Quaternion.identity);

            yield return new WaitForSeconds(m_wait);
        }
    }

    public bool Pause()
    {
        m_isPaused = !m_isPaused;

        if (m_isPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
        return m_isPaused;
    }

    public void AddScore(int scoreToAdd)
    {
        m_score += scoreToAdd;
    }
}
