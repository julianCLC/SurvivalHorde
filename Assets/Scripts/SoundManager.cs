using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // public GameObject[] SoundsGO;
    public AudioSource[] SoundClips;

    public static SoundManager instance {get; private set;}

    void Awake(){
        if(instance != null && instance != this){
            Destroy(this);
            DontDestroyOnLoad(gameObject);
        }
        else {
            instance = this;
        }
    }

    void Start() {
        // Initialize Audio Clips
    }

    void Update() {
        
    }

    public void PlaySound(string soundName) {
        bool soundPlayed = false;
        foreach (AudioSource sound in SoundClips){
            if(sound.name == soundName){
                sound.Play();
                soundPlayed = true;
            }
            else if(sound.gameObject.name == soundName){
                sound.Play();
                soundPlayed = true;
            }
        }

        if(!soundPlayed){
            Debug.Log("Couldn't find " + soundName + ", check spelling and make sure to add to SoundManager");
        }        
    }

    public AudioSource GetSound(string soundName) {
        AudioSource soundToReturn = null;
        foreach (AudioSource sound in SoundClips){
            if(sound.name == soundName){
                soundToReturn = sound;
                break;
            }
            else if(sound.gameObject.name == soundName){
                sound.Play();
                soundToReturn = sound;
            }
        }
        if(soundToReturn == null){
            Debug.Log("Couldn't find " + soundName + ", check spelling and make sure to add to SoundManager");
        } 
        return soundToReturn;
    }
}
