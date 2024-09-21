using UnityEngine;
using System.IO;

public enum LevelTileType {
    NONE,
    OBSTACLE
}

public static class LevelTileChar {
    public const char None = '0';
    public const char Obstacle = 'B';
}

public class LevelGenerator: MonoBehaviour {

    public TextAsset level;
    public GameObject obstacle;
    public GameObject lightTile;
    public GameObject darkTile;

    private void Start() {
        var size = SizeFromLevel(this.level.text);
        GenerateCheckboard(size.Item1, size.Item2, lightTile, darkTile);
        LevelTileType[,] level = ParseLevel(this.level.text);
    }

    public static LevelTileType[,] ParseLevel(string level) {
        string[] rows = level.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        var size = SizeFromLevel(level);
        int width = size.Item1;
        int height = size.Item2;

        LevelTileType[,] parsedLevel = new LevelTileType[height, width];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                parsedLevel[y, x] = LevelTileFromChar(rows[y][x]);
            }
        }

        return parsedLevel;
    }

    private static (int, int) SizeFromLevel(string level) {
        string[] rows = level.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        return (rows[0].Length, rows.Length);
    }

    private void GenerateCheckboard(int width, int height, GameObject lightTile, GameObject darkTile) {
        Vector3 startPosition = Vector3.zero;
        var tileSize = lightTile.GetComponent<SpriteRenderer>().bounds.size;
        startPosition.x -= (tileSize.x * width * 0.5f) - tileSize.x * 0.5f;
        startPosition.y -= (tileSize.y * height * 0.5f) - tileSize.y * 0.5f;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject tileToPlace = (x + y) % 2 == 0 ? lightTile : darkTile;
                Vector3 position = new(startPosition.x + tileSize.x * x, startPosition.y + tileSize.y * y, startPosition.z);
                GameObject tileInstance = Instantiate(tileToPlace, position, Quaternion.identity);
                tileInstance.name = $"{tileToPlace.name} ({x}, {y})";
                tileInstance.transform.parent = transform;
            }
        }
    }

    static LevelTileType LevelTileFromChar(char character) {
        return character switch {
            LevelTileChar.None => LevelTileType.NONE,
            LevelTileChar.Obstacle => LevelTileType.OBSTACLE,
            _ => LevelTileType.NONE,
        };
    }


}
