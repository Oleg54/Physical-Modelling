using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public void SetPointColor(Color color)
    {
        _meshRenderer.material.color = color;
    }
}
