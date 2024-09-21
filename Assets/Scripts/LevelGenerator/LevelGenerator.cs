using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum LevelTileType {
    NONE,
    PLAYER,
    RIGHT,
    LEFT,
    RIGHTLEFT,
    UP,
    DOWN,
    UPDOWN,
    ROTATE_CLOCKWISE,
    ROTATE_COUNTERCLOCKWISE,
    DOOR,
    KEY,
    WALL,
    MIRROR,
    WARP,
    NUMBER_1 = 1, NUMBER_2 = 2, NUMBER_3 = 3, NUMBER_4 = 4, NUMBER_5 = 5, NUMBER_6 = 6, NUMBER_7 = 7, NUMBER_8 = 8, NUMBER_9 = 9
}

public class LevelGenerator: MonoBehaviour {

    public TextAsset level;
    public GameObject lightTile;
    public GameObject darkTile;

    [Header("Level objects")]
    public GameObject playerObject;
    public GameObject rightObject;
    public GameObject leftObject;
    public GameObject rightLeftObject;
    public GameObject upObject;
    public GameObject downObject;
    public GameObject upDownObject;
    public GameObject rotateObject;
    public GameObject rotateCounterObject;
    public GameObject doorObject;
    public GameObject keyObject;
    public GameObject wallObject;
    public GameObject mirrorObject;
    public GameObject warpObject;

    [Header("Background objects")]
    public GameObject borderObject;

    private static readonly Dictionary<char, LevelTileType> CharToLevelTile = new() {
        { '.', LevelTileType.NONE },
        { 'P', LevelTileType.PLAYER },
        { '>', LevelTileType.RIGHT },
        { '<', LevelTileType.LEFT },
        { '-', LevelTileType.RIGHTLEFT },
        { '^', LevelTileType.UP },
        { 'V', LevelTileType.DOWN },
        { '|', LevelTileType.UPDOWN },
        { '@', LevelTileType.ROTATE_CLOCKWISE },
        { 'G', LevelTileType.ROTATE_COUNTERCLOCKWISE },
        { 'D', LevelTileType.DOOR },
        { 'K', LevelTileType.KEY },
        { '#', LevelTileType.WALL },
        { 'O', LevelTileType.MIRROR },
        { '*', LevelTileType.WARP },
    };


    private void Start() {
        var size = SizeFromLevel(level.text);
        DrawCheckboard(size.Item1, size.Item2, lightTile, darkTile);
        DrawLevel();
        DrawBorder(size.Item1, size.Item2);
        Debug.Log(GetInitialObjectsFromLevel(level.text));
    }

    public static LevelTileType[,] ParseLevel(string level) {
        string[] rows = level
        .Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
        .Select(row => new string(row.Trim().Where(x => !char.IsWhiteSpace(x)).ToArray()))
        .ToArray();
        List<string> rowsList = new();
        foreach (string row in rows) {
            if (row == "!") {
                rows = rowsList.ToArray();
                break;
            }
            rowsList.Add(row);
        }
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
        string[] rows = level
        .Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
        .Select(row => new string(
            row.Trim()
            .Where(x => !char.IsWhiteSpace(x) && !char.IsDigit(x)).ToArray()))
        .ToArray();
        List<string> rowsList = new();
        foreach (string row in rows) {
            if (row == "!") {
                rows = rowsList.ToArray();
                break;
            }
            rowsList.Add(row);
        }
        return (rows[0].Length, rows.Length);
    }

    private void DrawLevel() {
        LevelTileType[,] level = ParseLevel(this.level.text);
        GameObject lastObject = null;
        int width = level.GetLength(1);
        int height = level.GetLength(0);
        Vector3 startPosition = Vector3.zero;
        var tileSize = lightTile.GetComponent<SpriteRenderer>().bounds.size;
        startPosition.x -= (tileSize.x * width * 0.5f) - tileSize.x * 0.5f;
        startPosition.y -= (tileSize.y * height * 0.5f) - tileSize.y * 0.5f;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Vector3 position = new(startPosition.x + tileSize.x * x, startPosition.y + tileSize.y * y, startPosition.z);
                int number = NumberFromTileType(level[y, x]);
                if (number != -1) {
                    if (lastObject != null) {
                        // Add number of actions to the gameobject
                        Debug.Log(lastObject.name + " " + number);
                    }
                    continue;
                }
                lastObject = AddObjectToLevel(level[y, x], position, x, y);
            }
        }
    }

    private void DrawCheckboard(int width, int height, GameObject lightTile, GameObject darkTile) {
        Vector3 startPosition = Vector3.zero;
        var tileSize = lightTile.GetComponent<SpriteRenderer>().bounds.size;
        startPosition.x -= (tileSize.x * width * 0.5f) - tileSize.x * 0.5f;
        startPosition.y -= (tileSize.y * height * 0.5f) - tileSize.y * 0.5f;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject tileToPlace = (x + y) % 2 == 0 ? lightTile : darkTile;
                Vector3 position = new(startPosition.x + tileSize.x * x, startPosition.y + tileSize.y * y, startPosition.z);
                CreateTile(position, $"{tileToPlace.name} ({x}, {y})", tileToPlace);
            }
        }
    }

    public void DrawBorder(int width, int height) {
        Vector3 startPosition = Vector3.zero;
        var tileSize = lightTile.GetComponent<SpriteRenderer>().bounds.size.x;
        startPosition.x -= (tileSize * width * 0.5f) + tileSize * 0.5f;
        startPosition.y -= (tileSize * height * 0.5f) + tileSize * 0.5f;

        for (int x = 0; x < width + 2; x++) {
            var topPosition = new Vector3(x * tileSize, (height + 1) * tileSize, 0) + startPosition;
            CreateTile(topPosition, "Top", borderObject);
            var bottomPosition = new Vector3(x * tileSize, 0, 0) + startPosition;
            CreateTile(bottomPosition, "Bottom", borderObject);
        }

        startPosition.y += tileSize;

        for (int y = 0; y < height; y++) {
            Vector3 leftPosition = new Vector3(0, y * tileSize, 0) + startPosition;
            CreateTile(leftPosition, "Left", borderObject);
            Vector3 rightPosition = new Vector3((width + 1) * tileSize, y * tileSize, 0) + startPosition;
            CreateTile(rightPosition, "Right", borderObject);
        }
    }

    public static (LevelTileType, LevelTileType) GetInitialObjectsFromLevel(string level) {
        string[] rows = level
            .Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(row => new string(
                row.Trim()
                .Where(x => !char.IsWhiteSpace(x) && !char.IsDigit(x)).ToArray()))
            .ToArray();
        List<string> rowsList = new();

        int delimitersFound = 0;
        foreach (string row in rows) {
            if (row == "!") {
                delimitersFound += 1;
                continue;
            }
            if (delimitersFound == 1) {
                rowsList.Add(row);
            }
        }
        if (rowsList.Count != 2) {
            return (LevelTileType.NONE, LevelTileType.NONE);
        }
        return (LevelTileFromChar(rowsList[0].ToCharArray()[0]), LevelTileFromChar(rowsList[1].ToCharArray()[0]));
    }

    private GameObject CreateTile(Vector3 position, string name, GameObject tile) {
        GameObject tileInstance = Instantiate(tile, position, Quaternion.identity);
        tileInstance.name = name;
        tileInstance.transform.parent = transform;
        return tileInstance;
    }

    private GameObject AddObjectToLevel(LevelTileType tile, Vector3 position, int x, int y) {
        GameObject currentObject;
        switch (tile) {
            case LevelTileType.PLAYER:
                currentObject = playerObject;
                break;
            case LevelTileType.RIGHT:
                currentObject = rightObject;
                break;
            case LevelTileType.LEFT:
                currentObject = leftObject;
                break;
            case LevelTileType.RIGHTLEFT:
                currentObject = rightLeftObject;
                break;
            case LevelTileType.UP:
                currentObject = upObject;
                break;
            case LevelTileType.DOWN:
                currentObject = downObject;
                break;
            case LevelTileType.UPDOWN:
                currentObject = upDownObject;
                break;
            case LevelTileType.ROTATE_CLOCKWISE:
                currentObject = rotateObject;
                break;
            case LevelTileType.ROTATE_COUNTERCLOCKWISE:
                currentObject = rotateCounterObject;
                break;
            case LevelTileType.DOOR:
                currentObject = doorObject;
                break;
            case LevelTileType.KEY:
                currentObject = keyObject;
                break;
            case LevelTileType.WALL:
                currentObject = wallObject;
                break;
            case LevelTileType.MIRROR:
                currentObject = mirrorObject;
                break;
            case LevelTileType.WARP:
                currentObject = warpObject;
                break;
            default:
                return null;
        }
        return CreateTile(position, $"{currentObject.name} ({x}, {y})", currentObject);
    }

    static int NumberFromTileType(LevelTileType tileType) {
        return tileType switch {
            LevelTileType.NUMBER_1 => 1,
            LevelTileType.NUMBER_2 => 2,
            LevelTileType.NUMBER_3 => 3,
            LevelTileType.NUMBER_4 => 4,
            LevelTileType.NUMBER_5 => 5,
            LevelTileType.NUMBER_6 => 6,
            LevelTileType.NUMBER_7 => 7,
            LevelTileType.NUMBER_8 => 8,
            LevelTileType.NUMBER_9 => 9,
            _ => -1,
        };
    }

    static private LevelTileType LevelTileFromChar(char character) {
        if (char.IsDigit(character)) {
            int index = character - '0';
            LevelTileType[] numbers = {
                LevelTileType.NUMBER_1,
                LevelTileType.NUMBER_2,
                LevelTileType.NUMBER_3,
                LevelTileType.NUMBER_4,
                LevelTileType.NUMBER_5,
                LevelTileType.NUMBER_6,
                LevelTileType.NUMBER_7,
                LevelTileType.NUMBER_8,
                LevelTileType.NUMBER_9 };
            return numbers[index - 1];
        }
        return CharToLevelTile.TryGetValue(character, out var value)
            ? value : LevelTileType.NONE;
    }


}
