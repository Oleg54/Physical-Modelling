using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class RefractionSurface : MonoBehaviour
{
    [SerializeField] private bool _isMirror;

    public float RefractionFactor { get; private set; } = 1;
    public bool IsMirror => _isMirror;

    public void SetRefractionFactor(float value) => RefractionFactor = Mathf.Max(1, value);
}

