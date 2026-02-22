using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button sfx;
    [SerializeField] private Button music;
    [Header("Colors")]
    [SerializeField] private Color on;
    [SerializeField] private Color off;
    [Header("Data")]
    private bool sfxState ;
    private bool musicState ;
    public static Action<bool> onMusicChanged;
    public static Action<bool> onSfxChanged;
    void Awake()
    {
        Load();
        sfx.onClick.RemoveAllListeners();
        sfx.onClick.AddListener(()=>sfxButtonClickCallback());

        music.onClick.RemoveAllListeners();
        music.onClick.AddListener(()=>musicButtonClickCallback());
        
    }
    void Start()
    {
        onMusicChanged?.Invoke(musicState);
        onSfxChanged?.Invoke(sfxState);
        Debug.Log(PlayerPrefs.GetInt("SFX").ToString() + PlayerPrefs.GetInt("MUSIC") );
    }
    private void Load()
    {
        int sfxSve = PlayerPrefs.GetInt("SFX",1);
        sfxState = sfxSve == 1 ? true :false;
        int musicSave = PlayerPrefs.GetInt("MUSIC",1);
        musicState = musicSave == 1 ? true :false;
        updateSfxVisuals();
        updateMusicVisuals();
    }
    
    private void musicButtonClickCallback()
    {
        musicState = !musicState;
        updateMusicVisuals();
        SaveManager.instance.Save("MUSIC",musicState,Type.BUTTON);
        onMusicChanged?.Invoke(musicState);
    }

    private void sfxButtonClickCallback()
    {   
        sfxState = !sfxState;
        updateSfxVisuals();
        SaveManager.instance.Save("SFX",sfxState,Type.BUTTON);
        onSfxChanged?.Invoke(sfxState);
    }
    private void updateMusicVisuals()
    {
        if (musicState)
        {
            music.image.color = on;
            music.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }else
        {
            music.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
            music.image.color = off;
        }
    }

    private void updateSfxVisuals()
    {
        if (sfxState)
        {
            sfx.image.color = on;
            sfx.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }else
        {
            sfx.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
            sfx.image.color = off;
        }
    }
   
}
