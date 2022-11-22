using UnityEngine;

public class Surface : MonoBehaviour
{
    [SerializeField] private SurfaceType _surfaceType;

    public SurfaceType SurfaceType => _surfaceType;
}
