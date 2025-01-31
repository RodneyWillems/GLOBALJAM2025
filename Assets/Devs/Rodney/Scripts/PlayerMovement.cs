using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject m_tridentPrefab;
    [SerializeField] private int m_throwingSpeed;

    private PlayerControls m_playerControls;
    private Trident m_thrownTrident;

    #endregion

    #region Setup

    private void Awake()
    {
        SetupControls();
    }

    private void OnEnable()
    {
        m_playerControls.DefaultMovement.Enable();
    }

    private void OnDisable()
    {
        m_playerControls.DefaultMovement.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetupControls()
    {
        m_playerControls = new();
        m_playerControls.DefaultMovement.Shoot.started += ThrowTrident;
        m_playerControls.DefaultMovement.Pause.started += Pause;

    }

    #endregion

    private void ThrowTrident(InputAction.CallbackContext context)
    {
        // Throw the fucking Trident you moron READ THE NAME
        if (m_thrownTrident == null) 
        {
            m_thrownTrident = Instantiate(m_tridentPrefab, transform.position + transform.forward, Quaternion.identity).GetComponent<Trident>();
            m_thrownTrident.Player = this;
            m_thrownTrident.gameObject.transform.forward = transform.forward;
            m_thrownTrident.GetComponent<Rigidbody>().linearVelocity = transform.forward * m_throwingSpeed;
        }
    }

    public void TridentHitSomething(Vector3 newPos)
    {
        transform.position = newPos;
        m_thrownTrident = null;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        // PAUSE THE GODDAMN GAME
        /*if (GameManager.Instance.Pause())
        {
            m_playerControls.DefaultMovement.Shoot.Disable();
        }
        else 
        {
            m_playerControls.DefaultMovement.Shoot.Enable();
        }
        */
    }


    void Update()
    {

    }
}
