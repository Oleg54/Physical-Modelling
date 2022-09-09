using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 _tileSize;
    [SerializeField] private Vector2Int _backgroundSize;
    [SerializeField] private GameObject _tile;

    private void Awake()
    {
        for (int x = -_backgroundSize.x / 2; x <= _backgroundSize.x / 2; x++)
        {
            for (int y = -_backgroundSize.y / 2; y <= _backgroundSize.y / 2; y++)
            {
                Vector2 position = new Vector2(x * _tileSize.x, y * _tileSize.y);
                Instantiate(_tile, position, Quaternion.identity, transform);
            }
        }
    }
}
