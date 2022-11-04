using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgSound;
    public AudioClip[] bglist;
    public static SoundManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for(int i = 0; i < bglist.Length; i++)
        {
            if(arg0.name == bglist[i].name)
            {
                BgSoundPlay(bglist[i]);
            }
        }
    }

    public void SFXPlay(string sfxName, AudioSource clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.Play();

        Destroy(go, clip);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 1.0f;
        bgSound.Play();
    }
}
