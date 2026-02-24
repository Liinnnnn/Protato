using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public AudioClip clips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button[] button = FindObjectsByType<Button>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        for (int i = 0; i < button.Length; i++)
        {
            button[i].onClick.AddListener(()=>PlaySFX());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void PlaySFX()
    {
        if(!AudioManager.instance.isSfxOn) return;
        audioSource.PlayOneShot(clips);
        audioSource.pitch = Random.Range(0.9f, 1.2f);
        audioMixer.SetFloat("SFX",0f);
    }
}
