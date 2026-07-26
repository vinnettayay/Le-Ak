using UnityEngine;

public class RotateVFX : MonoBehaviour
{
    [SerializeField] float speed = 15f;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
