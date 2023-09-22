using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour{
    AudioSource source;

    [SerializeField] AudioClip[] clips;

    private void Start() {
        source = GetComponent<AudioSource>();
    }
    public void PlaySound(int clip) {
        if (UI.soundsEnabled) {
            source.clip = clips[clip];
            source.Play();
        }
    }
}
