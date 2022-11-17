using UnityEngine;

public class LevelPreset : MonoBehaviour
{
    [SerializeField] private bool _isGeneratable;

    private Obstacle[] _tiles;

    private void Awake()
    {
        _tiles = GetComponentsInChildren<Obstacle>(true);

        if (!_isGeneratable)
            return;

        foreach (Obstacle tile in _tiles)
        {
            Vector3 scale = tile.transform.localScale;
            scale.x *= Random.Range(0.4f, 1.3f);
            tile.transform.localScale = scale;

            tile.transform.Rotate(Vector3.forward * Random.Range(0f, 359f));

            tile.gameObject.SetActive(Random.Range(0, 4) != 0);

            tile.SetObstacleType((ObstacleType)Random.Range(0, 3));
        }
    }
}