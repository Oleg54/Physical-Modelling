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
        Camera.main.orthographicSize = Mathf.Max(5, new Vector3(_movementBody.transform.position.x, 0, _movementBody.transform.position.z).magnitude);
    }
}


