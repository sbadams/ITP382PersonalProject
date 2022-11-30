using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    [SerializeField] private AudioClip buttonDownSound;
    [SerializeField] private AudioClip chaChingSound;

    private AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void PlayButtonDownSound()
    {
        audioSrc.PlayOneShot(buttonDownSound);

    }

    public void PlayChaChingSound()
    {
        audioSrc.PlayOneShot(chaChingSound);
    }
}
