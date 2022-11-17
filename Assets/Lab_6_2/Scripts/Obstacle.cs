using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleType _obstacleType;

    public Rigidbody Rigidbody { get; private set; }
    public ObstacleType ObstacleType => _obstacleType;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();

        SetObstacleType(_obstacleType);

        if (_obstacleType != ObstacleType.Dynamic)
            Rigidbody.isKinematic = true;
    }

    public void SetObstacleType(ObstacleType obstacleType)
    {
        _obstacleType = obstacleType;
        
        switch(obstacleType)
        {
            case ObstacleType.Static: GetComponent<MeshRenderer>().material.color = Color.grey;
                break;

            case ObstacleType.Static_Accelerate:
                GetComponent<MeshRenderer>().material.color = Color.green;
                break;

            case ObstacleType.Dynamic:
                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
        }
    }
}
