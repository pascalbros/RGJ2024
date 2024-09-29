using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public enum LevelTileType {
    NONE,
    NUMBER_1 = 1, NUMBER_2 = 2, NUMBER_3 = 3, NUMBER_4 = 4, NUMBER_5 = 5, NUMBER_6 = 6, NUMBER_7 = 7, NUMBER_8 = 8, NUMBER_9 = 9,
    PLAYER,
    RIGHT,
    LEFT,
    RIGHTLEFT,
    UPLEFT,
    UPRIGHT,
    DOWNLEFT,
    DOWNRIGHT,
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
}

public class LevelGenerator: MonoBehaviour {

    public TextAsset level;
    public static string levelContent;
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
    public GameObject upLeftObject;
    public GameObject upRightObject;
    public GameObject downLeftObject;
    public GameObject downRightObject;
    public GameObject rotateObject;
    public GameObject rotateCounterObject;
    public GameObject doorObject;
    public GameObject keyObject;
    public GameObject wallObject;
    public GameObject mirrorObject;
    public GameObject warpObject;

    [Header("Background objects")]
    public GameObject borderObject;

    [Header("Other")]
    public TextAsset[] levels;

    private PlayerController currentPlayer;

    public static int currentLevel = 0;

    private static readonly Dictionary<char, LevelTileType> CharToLevelTile = new() {
        { '.', LevelTileType.NONE },
        { 'P', LevelTileType.PLAYER },
        { '>', LevelTileType.RIGHT },
        { '<', LevelTileType.LEFT },
        { '-', LevelTileType.RIGHTLEFT },
        { '^', LevelTileType.UP },
        { 'V', LevelTileType.DOWN },
        { '|', LevelTileType.UPDOWN },
        { 'J', LevelTileType.UPLEFT },
        { 'L', LevelTileType.UPRIGHT },
        { 'T', LevelTileType.DOWNLEFT },
        { 'F', LevelTileType.DOWNRIGHT },
        { '@', LevelTileType.ROTATE_CLOCKWISE },
        { 'G', LevelTileType.ROTATE_COUNTERCLOCKWISE },
        { 'E', LevelTileType.DOOR },
        { 'K', LevelTileType.KEY },
        { '#', LevelTileType.WALL },
        { 'O', LevelTileType.MIRROR },
        { '*', LevelTileType.WARP },
    };


    private void Start() {
        if (currentLevel >= levels.Length || currentLevel < 0) {
            currentLevel = 0;
        }
        if (level == null) {
            level = levels[currentLevel];
        } 
        if (levelContent == null || levelContent.Length == 0) {
            levelContent = level.text;
        } else {
            level = null;
            currentLevel = -1;
        }
        var size = SizeFromLevel(levelContent);
        DrawCheckboard(size.Item1, size.Item2, lightTile, darkTile);
        DrawLevel();
        DrawBorder(size.Item1, size.Item2);
        DrawInitialObjects();
        levelContent = null;
    }

    public static LevelTileType[,] ParseLevel(string level) {
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

    public static LevelTileType[,] ParseNumbers(string level) {
        string[] rows = level
        .Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
        .Select(row => new string(row.Trim().Where(x => !char.IsWhiteSpace(x)).ToArray()))
        .ToArray();
        List<string> oldRowsList = new();
        foreach (string row in rows) {
            if (row == "!") {
                rows = oldRowsList.ToArray();
                break;
            }
            oldRowsList.Add(row);
        }

        List<string> rowsList = new();
        foreach (var row in oldRowsList) {
            List<int> indexes = new();
            char[] cRow = row.ToCharArray();
            string sRow = "";
            for (int i = 0; i < cRow.Length; i++) {
                if (char.IsDigit(cRow[i])) {
                    sRow = sRow.Remove(sRow.Length - 1) + cRow[i];
                } else {
                    sRow += ".";
                }
            }
            rowsList.Add(sRow);
            //Debug.Log(sRow);
        }

        int width = rowsList[0].Length;
        int height = rowsList.Count;

        LevelTileType[,] parsedLevel = new LevelTileType[height, width];
        for (int y = 0; y < height; y++) {
            char[] currentRow = rowsList[y].ToArray();
            for (int x = 0; x < width; x++) {
                var tile = LevelTileFromChar(currentRow[x]);
                parsedLevel[y, x] = LevelTileFromChar(currentRow[x]);
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

    private Vector3 GetStartPosition(int width, int height)
    {
        Vector3 ts;
        return GetStartPosition(width, height, out ts);
    }
    private Vector3 GetStartPosition(int width, int height, out Vector3 tileSize)
    {
        Vector3 startPosition = Vector3.zero;
        tileSize = lightTile.GetComponent<SpriteRenderer>().bounds.size;
        startPosition.x -= Mathf.Round((tileSize.x * width * 0.5f) - tileSize.x * 0.5f);
        startPosition.y -= Mathf.Round((tileSize.y * height * 0.5f) - tileSize.y * 0.5f);
        return startPosition;
    }

    private void DrawLevel() {
        LevelTileType[,] numbers = ParseNumbers(levelContent);
        LevelTileType[,] level = ParseLevel(levelContent);
        GameObject lastObject = null;
        int width = level.GetLength(1);
        int height = level.GetLength(0);
        Vector3 tileSize;
        Vector3 startPosition = GetStartPosition(width, height, out tileSize);
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Vector3 position = new(startPosition.x + tileSize.x * x, startPosition.y + tileSize.y * (height - 1 - y), startPosition.z);
                var number = NumberFromTileType(numbers[y, x]);
                //Debug.Log(number + " " + x + y + "<-");
                lastObject = AddObjectToLevel(level[y, x], position, x, height - 1 - y);
                if (number != -1) {
                    if (lastObject != null) {
                        // Add number of actions to the gameobject
                        lastObject.GetComponent<Portable>().SetUsages(number);
                    }
                    continue;
                }
                if (lastObject) {
                    var player = lastObject.GetComponentInChildren<PlayerController>();
                    if (player) {
                        currentPlayer = player;
                    }
                }
            }
        }
    }

    private void DrawCheckboard(int width, int height, GameObject lightTile, GameObject darkTile) {
        Vector3 tileSize;
        Vector3 startPosition = GetStartPosition(width, height, out tileSize);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject tileToPlace = (x + y) % 2 == 0 ? lightTile : darkTile;
                Vector3 position = new(startPosition.x + tileSize.x * x, startPosition.y + tileSize.y * y, startPosition.z);
                CreateTile(position, $"{tileToPlace.name} ({x}, {y})", tileToPlace);
            }
        }
    }

    public void DrawBorder(int width, int height) {
        Vector3 tileSize;
        Vector3 startPosition = GetStartPosition(width, height, out tileSize);
        startPosition -= tileSize;

        for (int x = 0; x < width + 2; x++) {
            var topPosition = new Vector3(x * tileSize.x, (height + 1) * tileSize.x, 0) + startPosition;
            CreateTile(topPosition, "Top", borderObject);
            var bottomPosition = new Vector3(x * tileSize.x, 0, 0) + startPosition;
            CreateTile(bottomPosition, "Bottom", borderObject);
        }

        startPosition.y += tileSize.y;

        for (int y = 0; y < height; y++) {
            Vector3 leftPosition = new Vector3(0, y * tileSize.y, 0) + startPosition;
            CreateTile(leftPosition, "Left", borderObject);
            Vector3 rightPosition = new Vector3((width + 1) * tileSize.y, y * tileSize.y, 0) + startPosition;
            CreateTile(rightPosition, "Right", borderObject);
        }
    }

    public void DrawInitialObjects() {
        if (!currentPlayer) {
            return;
        }
        var objects = GetInitialObjectsFromLevel(levelContent);
        var portable1 = AddObjectToLevel(objects.Item1, Vector3.zero, 0, 0);
        var portable2 = AddObjectToLevel(objects.Item2, Vector3.zero, 0, 0);
        var p1 = portable1 != null ? portable1.GetComponent<Portable>() : null;
        if (p1 != null) {
            p1.SetUsages(objects.Item3);
        }
        var p2 = portable2 != null ? portable2.GetComponent<Portable>() : null;
        if (p2 != null) {
            p2.SetUsages(objects.Item4);
        }
        currentPlayer.InitPortables(p1, p2);
    }

    public static (LevelTileType, LevelTileType, int, int) GetInitialObjectsFromLevel(string level) {
        string[] rows = level
            .Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(row => new string(
                row.Trim()
                .Where(x => !char.IsWhiteSpace(x)).ToArray()))
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
            return (LevelTileType.NONE, LevelTileType.NONE, -1, -1);
        }
        var firstList = rowsList[0].ToCharArray();
        var secondList = rowsList[1].ToCharArray();
        var firstCount = firstList.Length > 1 ? firstList[1] - '0' : -1;
        var secondCount = secondList.Length > 1 ? secondList[1] - '0' : -1;
        return (LevelTileFromChar(firstList[0]), LevelTileFromChar(secondList[0]), firstCount, secondCount);
    }

    private GameObject CreateTile(Vector3 position, string name, GameObject tile) {
        GameObject tileInstance = Instantiate(tile, position, Quaternion.identity);
        tileInstance.name = name;
        tileInstance.transform.parent = transform;
        return tileInstance;
    }

    private GameObject GameObjectFromTile(LevelTileType tile) {
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
            case LevelTileType.UPLEFT:
                currentObject = upLeftObject;
                break;
            case LevelTileType.UPRIGHT:
                currentObject = upRightObject;
                break;
            case LevelTileType.DOWNLEFT:
                currentObject = downLeftObject;
                break;
            case LevelTileType.DOWNRIGHT:
                currentObject = downRightObject;
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
        return currentObject;
    }

    private GameObject AddObjectToLevel(LevelTileType tile, Vector3 position, int x, int y) {
        GameObject currentObject = GameObjectFromTile(tile);
        if (!currentObject) {
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
