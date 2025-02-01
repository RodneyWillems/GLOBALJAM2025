using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Trident shit")]
    [SerializeField] private GameObject m_tridentPrefab;
    [SerializeField] private Transform m_tridentSpawnPoint;
    [SerializeField] private int m_throwingSpeed;

    [Header("Kanker camera")]
    [SerializeField] private Transform m_cameraBS;

    [Header("Je kanker health")]
    [SerializeField] private GameObject[] m_unactiveLives;

    private int m_health;

    //Miscellaneous
    private PlayerControls m_playerControls;
    private Trident m_thrownTrident;

    #endregion

    #region Setup

    private void Awake()
    {
        SetupControls();
        m_health = 3;
    }

    private void OnEnable()
    {
        m_playerControls.DefaultMovement.Enable();
    }

    private void OnDisable()
    {
        m_playerControls.DefaultMovement.Disable();
    }

    private void SetupControls()
    {
        //THIS IS JUST SETUP LOOK AWAY YOU FUCKING DUMBASS I HATE YOU
        m_playerControls = new();
        m_playerControls.DefaultMovement.Shoot.started += ThrowTrident;
        m_playerControls.DefaultMovement.Pause.started += Pause;

    }

    #endregion

    #region Damage

    private void OnCollisionEnter(Collision collision)
    {
        // When player gets hit by bomb he go DIE
        if (collision.gameObject.CompareTag("Bomb"))
        {
            Destroy(collision.gameObject);
            m_health--;
            m_unactiveLives[m_health].SetActive(true);
            if (m_health <= 0)
            {
                m_playerControls.DefaultMovement.Disable();
                GameManager.Instance.PlayerDeath();
            }
        }
    }

    #endregion

    #region INPUTS

    private void ThrowTrident(InputAction.CallbackContext context)
    {
        // Throw the fucking Trident you moron READ THE NAME
        if (m_thrownTrident == null) 
        {
            m_thrownTrident = Instantiate(m_tridentPrefab, m_tridentSpawnPoint.position, Quaternion.identity).GetComponent<Trident>();
            m_thrownTrident.Player = this;
            m_thrownTrident.gameObject.transform.forward = m_cameraBS.forward;
            m_thrownTrident.GetComponent<Rigidbody>().linearVelocity = m_cameraBS.forward * m_throwingSpeed;
        }
    }

    public void TridentHitSomething(Vector3 newPos, bool teleport = false)
    {
        StartCoroutine(WaitTeleport(newPos, teleport));
    }

    private IEnumerator WaitTeleport(Vector3 newPos, bool teleport = false)
    {
        yield return new WaitForSeconds(0.4f);
        // When the trident fucking hits a bubble you TELEPORT
        if (teleport)
        {
            transform.position = newPos + transform.forward * 2;
        }
        m_thrownTrident = null;
        yield return null;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        // PAUSE THE GODDAMN GAME
        if (GameManager.Instance.Pause())
        {
            m_playerControls.DefaultMovement.Shoot.Disable();
            m_playerControls.DefaultMovement.Look.Disable();
        }
        else 
        {
            m_playerControls.DefaultMovement.Shoot.Enable();
            m_playerControls.DefaultMovement.Look.Enable();
        }
    }

    #endregion
}
