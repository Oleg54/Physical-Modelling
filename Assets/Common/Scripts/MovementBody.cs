using UnityEngine;

public class MovementBody : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetPosition(Vector3 position)
    {
        _rigidbody.position = position;
    }
}
