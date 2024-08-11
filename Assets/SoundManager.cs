using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);

        }
        else if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }

    }
    public void PlaySound(AudioClip _sound)
    {
        audioSource.PlayOneShot(_sound);
    }
}
