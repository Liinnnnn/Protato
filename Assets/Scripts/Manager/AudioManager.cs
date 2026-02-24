using System;
using UnityEngine;
using UnityEngine.Audio;
public enum BGMMusic
{
    MENUBGM,
    GAMEBGM,
    SHOPBGM,
    WAVETRANSBGM,
    GAMEOVERBGM,
    WEAPONCHOSEBGM,
    COMPLETEBGM
}
public class AudioManager : MonoBehaviour,IGameStateListener
{
    public AudioClip[] clips;
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    public bool isSfxOn {get;private set;}
    public bool isMusicOn {get;private set;}
    void Awake()
    {
        instance = this;
        SettingManager.onMusicChanged += MusicChangedCallback;
        SettingManager.onSfxChanged += SFXChangedCallback;
    }
    void Update()
    {
        
    }
    private void PlayBGM(BGMMusic bGM, float volumn =1, bool loop = true)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(clips[Array.IndexOf(Enum.GetValues(typeof(BGMMusic)),bGM)]);
        if (!isMusicOn) return;
        audioMixer.SetFloat("Music",0f);
        audioSource.loop = loop;
    }

    private void SFXChangedCallback(bool obj)
    {
        isSfxOn = obj;
        
    }

    private void MusicChangedCallback(bool obj)
    {
        isMusicOn = obj;
        if (obj == false)
        {
            audioMixer.SetFloat("Music",-80f);
        }else 
            audioMixer.SetFloat("Music",0f);
    }

    void OnDestroy()
    {
        SettingManager.onMusicChanged -= MusicChangedCallback;
        SettingManager.onSfxChanged -= SFXChangedCallback;
    }

    public void GameStateChangeCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU :
                PlayBGM(BGMMusic.MENUBGM);
                break;
            case GameState.GAME :
                PlayBGM(BGMMusic.GAMEBGM);
                break;
            case GameState.WAVETRANS :
                PlayBGM(BGMMusic.WAVETRANSBGM,1,false);
                break;            
            case GameState.SHOP :
                PlayBGM(BGMMusic.SHOPBGM,0.6f);
                break;
            case GameState.GAMEOVER:
                PlayBGM(BGMMusic.GAMEOVERBGM,1,false);
                break;
            case GameState.WEAPONCHOSE:
                PlayBGM(BGMMusic.WEAPONCHOSEBGM);
                break;
            case GameState.COMPLETE:
                PlayBGM(BGMMusic.COMPLETEBGM,1,false);
                break;
        }
    }
}
