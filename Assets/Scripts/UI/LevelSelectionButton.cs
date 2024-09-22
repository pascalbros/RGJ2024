using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionButton : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        LevelGenerator.currentLevel = level;
        SceneTransitionManager.Instance.ChangeScene("LevelTest");
    }
}
