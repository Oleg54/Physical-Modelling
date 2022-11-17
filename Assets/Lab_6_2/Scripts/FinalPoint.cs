using UnityEngine;

public class FinalPoint : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovementBody body))
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MovementBody body))
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
        }
    }
}
