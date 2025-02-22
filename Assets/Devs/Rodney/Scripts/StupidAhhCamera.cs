using UnityEngine;

public class StupidAhhCamera : MonoBehaviour
{
    [SerializeField] private float m_mouseSense;
    [SerializeField] private Transform m_playerBody;
    [SerializeField] private float m_minViewDistance;
    [SerializeField] private float m_xRotation;

    private PlayerControls m_playerControls;

    private void Awake()
    {
        m_playerControls = new();
        m_playerControls.DefaultMovement.Look.Enable();
    }

    private void OnDisable()
    {
        m_playerControls.DefaultMovement.Look.Disable();
    }

    private void Look()
    {
        // Don't fucking ask me go to Brackey's and watch
        float mouseX = m_playerControls.DefaultMovement.Look.ReadValue<Vector2>().x * m_mouseSense * Time.deltaTime;
        float mouseY = m_playerControls.DefaultMovement.Look.ReadValue<Vector2>().y * m_mouseSense * Time.deltaTime;

        m_xRotation -= mouseY;
        m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);
        m_playerBody.Rotate(Vector3.up * mouseX);
    }

    void Update()
    {
        Look();   
    }
}
