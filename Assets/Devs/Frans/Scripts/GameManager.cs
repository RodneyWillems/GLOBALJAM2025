using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject m_bombPrefab;
    public GameObject[] m_bubblePrefab;

    [SerializeField]
    private GameObject m_bossBubblePrefab, m_pipePrefab;

    [Header ("Screens")]
    [SerializeField]
    private GameObject m_loseScreen;

    public GameObject m_winScreen;

    [Header ("Score")]
    [SerializeField]
    private TextMeshProUGUI m_scoreText;

    [Header ("Crosshair")]
    [SerializeField]
    private GameObject m_crosshair;

    [Header ("Audio")]
    [SerializeField]
    private AudioClip m_ambient;
    [SerializeField]
    private AudioClip m_bossMusic;

    private List<GameObject> m_pipeSpawns;

    private GameObject m_spawnedPipes;
    private Coroutine m_bubbleSpawner;

    private bool m_isPaused;
    private bool m_bossSpawned;
    public bool m_endless;

    private float m_wait;

    private int m_amountOfPipes = 10;
    private int m_score;
    private int m_bossScore;

    private AudioSource m_audioSource;

    #region Launch Game
    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
        m_wait = 0.9f;
        m_pipeSpawns = new();
        m_audioSource = GetComponent<AudioSource>();
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
    #endregion

    #region Spawns
    //This spawns the boss
    private void BossSpawner()
    {
        m_bossSpawned = true;
        m_audioSource.Stop();
        m_audioSource.resource = m_bossMusic;
        m_audioSource.volume = 0.7f;
        m_audioSource.Play();
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
    #endregion

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

    public void StartEndless()
    {
        m_endless = true;
        m_winScreen.SetActive(false);
        m_crosshair.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        BossDeath();
    }

    public void BossDeath()
    {
        m_bossSpawned = false;
        m_audioSource.Stop();
        m_audioSource.resource = m_ambient;
        m_audioSource.volume = 1f;
        m_audioSource.Play();

        StartCoroutine(BubblesSpawner());
    }

    public void PlayerDeath()
    {
        Time.timeScale = 0;
        m_crosshair.SetActive(false);
        Cursor.lockState= CursorLockMode.None;
        m_loseScreen.SetActive(true);
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
