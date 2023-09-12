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

    [SerializeField] private AudioClip ball;
    [SerializeField] private AudioClip ball_cage;
    [SerializeField] private AudioClip stageClear;
    [SerializeField] private AudioClip gameOver;



    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "IntroScene")
        {
            BgmPlay(BGM_Home);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PlayClickEffect()
    {
        effectAudioSource.PlayOneShot(clickEffect);
    }

    public void PlayReturnEffect()
    {
        effectAudioSource.PlayOneShot(returnEffect);
    }

    public void PlayAcceptEffect()
    {
        effectAudioSource.PlayOneShot(acceptEffect);
    }

    public void PlayErrorEffect()
    {
        effectAudioSource.PlayOneShot(errorEffect);
    }

    public void PauseBGM()
    {
        bgmAudioSource.Pause();
    }
    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void ResumeBGM()
    {
        bgmAudioSource.Play();
    }

    public void PlayBallEffect()
    {
        effectAudioSource.PlayOneShot(ball);
    }

    public void PlayBallEffectOnCage()
    {
        effectAudioSource.PlayOneShot(ball_cage);
    }

    public void PlayStageClear()
    {
        effectAudioSource.PlayOneShot(stageClear);
    }

    public void PlayGameOver()
    {
        effectAudioSource.PlayOneShot(gameOver);
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("OnSceneLoaded");
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
            if (bgmAudioSource.clip != BGM_Home)
            {
                BgmPlay(BGM_Home);
            }
        }
    }

    private void BgmPlay(AudioClip clip)
    {
        bgmAudioSource.clip = clip;
        bgmAudioSource.loop = true;
        bgmAudioSource.volume = 0.5f;
        bgmAudioSource.Play();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
