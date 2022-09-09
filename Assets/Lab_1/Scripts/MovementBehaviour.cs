using UnityEngine;

public class MovementBehaviour : IMovementInfo
{
    private Vector3 _startPosition;
    private Vector3 _accelerate;
    private Vector3 _velocity;
    private Vector3 _commonOffcets;

    private float _moveTime;

    public Vector3 StartPosition => _startPosition;
    public Vector2 Velocity => _velocity;
    public Vector2 Accelerate => _accelerate;
    public Vector2 Position => GetPosition();
    public float Time => _moveTime;
    public float Way => _commonOffcets.magnitude;

    public MovementBehaviour(Vector3 startPosition, Vector3 velocity, Vector3 accelerate)
    {
        _startPosition = startPosition;
        _velocity = velocity;
        _accelerate = accelerate;
    }

    public Vector3 GetMoveOffcet(float time)
    {
        Vector3 offcet = _velocity * time;
        _velocity += _accelerate * time;
        _moveTime += time;

        _commonOffcets += offcet;

        return offcet;
    }

    public Vector3 GetPosition()
    {
        return _startPosition + _commonOffcets;
    }

    public void SetStartPosition(Vector3 startPosition) => _startPosition = startPosition;
    public void SetVelocity(Vector3 velocity) => _velocity = velocity;
    public void SetAccelerate(Vector3 accelerate) => _accelerate = accelerate;
}
