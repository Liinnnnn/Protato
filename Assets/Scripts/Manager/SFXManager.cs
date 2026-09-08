using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip buttons;
    [SerializeField] private AudioClip enemyShoot;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    public static SFXManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button[] button = FindObjectsByType<Button>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        for (int i = 0; i < button.Length; i++)
        {
            button[i].onClick.AddListener(()=>PlaySFX());
        }
    }
    void Awake()
    {
        instance = this;
    }
    private void PlaySFX()
    {
        if(!AudioManager.instance.isSfxOn) return;
        audioSource.PlayOneShot(buttons);
        audioSource.pitch = Random.Range(0.7f, 1f);
        audioMixer.SetFloat("SFX",0f);
    }
    public void PlayEnemyShootSFX()
    {
        if(!AudioManager.instance.isSfxOn) return;
        audioSource.PlayOneShot(enemyShoot);
        audioSource.pitch = Random.Range(0.7f, 1f);
        audioMixer.SetFloat("SFX",0f);
    }
    public void PlayEnemyHitSFX()
    {
        if(!AudioManager.instance.isSfxOn) return;
        audioSource.PlayOneShot(enemyHit);
        audioSource.pitch = Random.Range(0.7f, 1f);
        audioMixer.SetFloat("SFX",0f);
    }
}
