using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioClip BGM_Home;
    [SerializeField] private AudioClip BGM_Game_1;
    [SerializeField] private AudioClip BGM_Game_2;

    public AudioClip clickEffect;
    public AudioClip returnEffect;
    public AudioClip acceptEffect;
    public AudioClip errorEffect;



    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayClickEffect()
    {
        audioSource.PlayOneShot(errorEffect);
    }

    public void PlayReturnEffect()
    {
        audioSource.PlayOneShot(returnEffect);
    }

    public void PlayAcceptEffect()
    {
        audioSource.PlayOneShot(acceptEffect);
    }

    public void PlayErrorEffect()
    {
        audioSource.PlayOneShot(errorEffect);
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "GameScene")
        {
            BgmPlay(BGM_Game_1);
        }
        else if (scene.name == "IntroScene")
        {
            BgmPlay(BGM_Home);
        }
        else
        {
            if (audioSource.clip != BGM_Home)
            {
                BgmPlay(BGM_Home);
            }
        }
    }

    private void BgmPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
}
