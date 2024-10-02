using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        LevelGenerator.currentLevel = level;
        SceneTransitionManager.Instance.ChangeScene("LevelTest");
    }

    public void LoadString(TMP_InputField inputField)
    {
        LoadString(inputField.text);
    }

    public void LoadString(string level)
    {
        Debug.Log(level);
        LevelGenerator.currentLevel = -1;
        LevelGenerator.levelContent = level.ToUpper();
        SceneTransitionManager.Instance.ChangeScene("LevelTest");
    }

    public void LoadUrl(TMP_InputField inputField)
    {
        LoadUrl(inputField.text);
    }

    public void LoadUrl(string url)
    {
        Debug.Log(url);
        StartCoroutine(LoadUrlAsync(url));
    }

    IEnumerator LoadUrlAsync(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            LoadString(www.downloadHandler.text);
        }
    }
}
