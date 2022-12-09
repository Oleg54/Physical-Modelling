using UnityEngine;

public class FinalPoint : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public void SetPointColor(Color color)
    {
        _meshRenderer.material.color = color;
    }
}
