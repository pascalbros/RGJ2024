using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController: MonoBehaviour {
    public void OnExit() {
        if (LevelGenerator.currentLevel >= 0)
        {
            LevelGenerator.currentLevel += 1;
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneTransitionManager.Instance.ChangeScene(currentSceneName);
        } else
        {
            SceneTransitionManager.Instance.ChangeScene("LevelSelection");
        }
    }
}
