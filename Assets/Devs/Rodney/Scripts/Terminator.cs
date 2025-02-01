using UnityEngine;

public class Terminator : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 1);
    }
}
