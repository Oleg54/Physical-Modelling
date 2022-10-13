using UnityEngine;

public class MovementBody : MonoBehaviour
{
    private Vector3 _lastPosition;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetPosition(Vector3 position)
    {
        _rigidbody.position = position;
    }

    private void FixedUpdate()
    {
        Vector3 direction = (_rigidbody.position - _lastPosition).normalized;

        if (direction.magnitude > 0.1f)
            _rigidbody.rotation = Quaternion.LookRotation(direction, transform.up);

        _lastPosition = _rigidbody.position;
    }
}
