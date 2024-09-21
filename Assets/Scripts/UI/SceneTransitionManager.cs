using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] Image fadePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(C_ChangeScene(sceneName));
    }
    
    IEnumerator C_ChangeScene(string sceneName)
    {
        fadePanel.color = new Color(0, 0, 0, 0);
        yield return fadePanel.DOColor(new Color(0, 0, 0, 1), fadeDuration / 2).WaitForCompletion();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        fadePanel.color = new Color(0, 0, 0, 1);
        yield return fadePanel.DOColor(new Color(0, 0, 0, 0), fadeDuration / 2).WaitForCompletion();
    }
}
