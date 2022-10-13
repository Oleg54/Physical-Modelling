using UnityEngine;

public class ScrewCameraMovement : MonoBehaviour
{
    [SerializeField] private float _yOffcet;

    private MovementBody _movementBody;

    private void Awake()
    {
        _movementBody = FindObjectOfType<MovementBody>(true);
    }

    private void Update()
    {
        Vector3 position = Camera.main.transform.position;
        position.y = _movementBody.transform.position.y + _yOffcet;

        Camera.main.transform.position = position;
    }
}
