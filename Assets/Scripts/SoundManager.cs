using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip insertClip;
    public AudioClip successClip;
    public AudioClip wrongClip;
    public AudioClip gameOverClip;
    public AudioClip gameMusicClip;

    public AudioSource audioSource;

    private void Start()
    {
        //audioSource = gameMusicClip;
        audioSource.PlayOneShot(gameMusicClip);
        audioSource.loop = true;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayInsert()
    {
        audioSource.PlayOneShot(insertClip);
    }
    public void PlaySuccess()
    {
        audioSource.PlayOneShot(successClip);
    }

    public void PlayWrong()
    {
        audioSource.PlayOneShot(wrongClip);
    }

    public void PlayGameOver()
    {
        audioSource.PlayOneShot(gameOverClip);
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
    public void Back()
    {
        // Replace "GameScene" with the actual name of your main game scene
        SceneManager.LoadScene("Main");
    }

}
