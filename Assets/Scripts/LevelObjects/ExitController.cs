using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController: MonoBehaviour {

    public void OnExit() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
