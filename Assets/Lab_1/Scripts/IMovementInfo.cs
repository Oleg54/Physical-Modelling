using UnityEngine;

public interface IMovementInfo
{
    public Vector2 Velocity { get; }
    public Vector2 Accelerate { get; }
    public Vector2 Position { get; }
    public float Time { get; }
    public float Way { get; }
}