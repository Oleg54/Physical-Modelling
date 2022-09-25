using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private MovementBody _movementBody;

    private void Awake()
    {
        _movementBody = FindObjectOfType<MovementBody>(true);
    }

    private void Update()
    {
        //Vector3 position = transform.position;
        //position.z = -Mathf.Max(10, _movementBody.transform.position.magnitude);
        //transform.position = position;
        Camera.main.orthographicSize = Mathf.Max(5, _movementBody.transform.position.magnitude);
    }
}
