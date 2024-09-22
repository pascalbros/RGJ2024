using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    AudioSource audioSource;
    [SerializeField] AudioClip menuClip;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Play(Track.Menu);
    }

    public void Play(Track track)
    {
        switch (track)
        {
            case Track.Menu:
                audioSource.PlayOneShot(menuClip);
                break;
            case Track.Game:
                break;
        }
    }

}

public enum Track
{
    Menu,
    Game
}