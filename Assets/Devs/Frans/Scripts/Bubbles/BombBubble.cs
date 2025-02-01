using UnityEngine;
using System.Collections.Generic;

public class BombBubble : BubbleBase
{
    private PlayerMovement m_PlayerControls;

    void Start()
    {
        m_PlayerControls = FindFirstObjectByType<PlayerMovement>();
    }

    void Update()
    {
        if (transform.position.y <= 5)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 5, Time.deltaTime / m_lerpTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, m_PlayerControls.transform.position, Time.deltaTime / m_lerpTime);
        }
    }
}
