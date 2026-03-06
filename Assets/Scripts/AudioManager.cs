using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour{
    public static AudioManager Instance;

    public AudioSource sfxSource;

    public AudioClip blockClip;

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
            return;
        }
        sfxSource = GetComponent<AudioSource>();
    }

    private void PlaySFX(AudioClip clip){
        sfxSource.PlayOneShot(clip);

    }

    public void PlayBlockSFX() => PlaySFX(blockClip);
}