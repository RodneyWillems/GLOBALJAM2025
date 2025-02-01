using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject m_bossBubblePrefab, m_pipePrefab;

    [SerializeField]
    private GameObject m_death;

    [SerializeField]
    private TextMeshProUGUI m_scoreText;

    public GameObject m_bombPrefab;
    public GameObject[] m_bubblePrefab;

    private List<GameObject> m_pipeSpawns;

    private GameObject m_spawnedPipes;
    private Coroutine m_bubbleSpawner;

    private bool m_isPaused;
    private bool m_bossSpawned;
    private bool m_endless;

    private float m_wait;

    private int m_amountOfPipes = 10;
    private int m_score;
    private int m_bossScore;


    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
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
                Vector3 randomLook = Random.insideUnitSphere + m_spawnedPipes.transform.position;
                randomLook.y = 0;
                m_spawnedPipes.transform.LookAt(randomLook);
                m_pipeSpawns.Add(m_spawnedPipes);
            }
        }

        m_bubbleSpawner = StartCoroutine(BubblesSpawner());
    }

    private void BossSpawner()
    {
        m_bossSpawned = true;
        Instantiate(m_bossBubblePrefab, new Vector3(0, 10, 0), Quaternion.identity);
    }

    private IEnumerator BubblesSpawner()
    {
        while (!m_bossSpawned)
        {
            int randomPipe = Random.Range(0, m_pipeSpawns.Count);
            int randombomb = Random.Range(0, 20);
            int randomBubble = Random.Range(0, m_bubblePrefab.Length);
            if(randombomb == 5)
            {
                Instantiate(m_bombPrefab, m_pipeSpawns.ElementAt(randomPipe).transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(m_bubblePrefab[randomBubble], m_pipeSpawns.ElementAt(randomPipe).transform.position, Quaternion.identity);
            }
 
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

    public void PlayerDeath()
    {
        Time.timeScale = 0;
        Cursor.lockState= CursorLockMode.None;
        m_death.SetActive(true);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void AddScore(int scoreToAdd)
    {
        m_score += scoreToAdd;
        m_bossScore += scoreToAdd;
        m_scoreText.text = "Score: " + m_score.ToString(); 

        if(m_bossScore >= 1000)
        {
            BossSpawner();
            m_bossScore = 0;
        }
    }
}
