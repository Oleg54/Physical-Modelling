using UnityEngine;

public class RefractionSurface : MonoBehaviour
{
    public float RefractionFactor { get; private set; } = 1;

    public void SetRefractionFactor(float value) => RefractionFactor = value;
}