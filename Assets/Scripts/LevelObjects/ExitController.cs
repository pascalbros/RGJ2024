using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController: MonoBehaviour {
    public void OnExit() {
        LevelGenerator.currentLevel += 1;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
