using UnityEngine;

public class FixedRelativeCameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _relativeOffcet;

    private MovementBody _movementBody;

    private void Awake()
    {
        _movementBody = FindObjectOfType<MovementBody>(true);
    }

    private void Update()
    {
        Camera.main.transform.position = _movementBody.transform.position + _relativeOffcet;
    }
}


