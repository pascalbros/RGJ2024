using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioClips = Resources.LoadAll<AudioClip>("Music");
    }

    private void Start()
    {
        StartCoroutine(C_PlayAllSequence());
    }

    IEnumerator C_PlayAllSequence()
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            audioSource.PlayOneShot(audioClips[i]);
            yield return new WaitForSeconds(audioClips[i].length);
        }
        StartCoroutine(C_PlayAllSequence());
    }

}
