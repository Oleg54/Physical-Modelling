using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private MovementBody _movementBody;
    [SerializeField] private Vector2Int _tileSize;
    [SerializeField] private Vector2Int _backgroundSize;
    [SerializeField] private GameObject _tile;

    private GameObject[][] _tiles;

    private void Awake()
    {
        _tiles = new GameObject[_backgroundSize.x][];

        for (int x = -_backgroundSize.x / 2; x <= _backgroundSize.x / 2; x++)
        {
            int xIndex = x + _backgroundSize.x / 2;
            _tiles[xIndex] = new GameObject[_backgroundSize.y];

            for (int y = -_backgroundSize.y / 2; y <= _backgroundSize.y / 2; y++)
            {
                int yIndex = y + _backgroundSize.y / 2;

                Vector2 position = new Vector2(x * _tileSize.x, y * _tileSize.y);
                GameObject tile = Instantiate(_tile, position, Quaternion.identity, transform);

                _tiles[xIndex][yIndex] = tile;
            }
        }
    }

    private void Update()
    {
        for (int x = 0; x < _backgroundSize.x; x++)
        {
            for (int y = 0; y < _backgroundSize.y; y++)
            {
                Vector2Int centerOffcet = new Vector2Int(x - _backgroundSize.x / 2, y - _backgroundSize.y / 2);
                Vector2Int realCenterOffcet = Vector2Int.Scale(centerOffcet, _tileSize);

                Vector2 startBodyPosition = (Vector2)_movementBody.transform.position;

                float xRound = startBodyPosition.x % _tileSize.x;
                float yRound = startBodyPosition.y % _tileSize.y;

                Vector2 roundedBodyPosition = new Vector2(xRound < _tileSize.x / 2
                    ? startBodyPosition.x - xRound : startBodyPosition.x - xRound + _tileSize.x,
                    yRound < _tileSize.y / 2
                    ? startBodyPosition.y - yRound : startBodyPosition.y - yRound + _tileSize.y);

                Vector2 targetTilePosition = roundedBodyPosition + (Vector2)realCenterOffcet;

                _tiles[x][y].transform.position = targetTilePosition;
            }
        }
    }
}
