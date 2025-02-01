using UnityEngine;

public class Bubbles : BubbleBase
{
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 5 , Time.deltaTime / m_lerpTime);
    }
}
