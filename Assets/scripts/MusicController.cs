using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the GameObject.");
            return;
        }

        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void ResumeMusic()
    {
        audioSource.Play();
    }
}
