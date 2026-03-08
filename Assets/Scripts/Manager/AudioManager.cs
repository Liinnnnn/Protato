using System;
using System.Collections.Generic;
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
    private BGMMusic lastMusicType; 
    private float gameMusicTimestamp = 0f;
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
       
        if (lastMusicType == BGMMusic.GAMEBGM && bGM != BGMMusic.GAMEBGM)
        {
            gameMusicTimestamp = audioSource.time;
        }
        audioSource.Stop();

        AudioClip nextClip = clips[Array.IndexOf(Enum.GetValues(typeof(BGMMusic)), bGM)];
        
        audioSource.clip = nextClip;
        audioSource.volume = volumn;
        audioSource.loop = loop;

        
        if (bGM == BGMMusic.GAMEBGM)
        {
            audioSource.time = gameMusicTimestamp;
        }
        else
        {
            // Các state khác (MENU, SHOP...) thì luôn chơi từ đầu (0s)
            audioSource.time = 0f;
        }

        audioSource.Play();
        lastMusicType = bGM; 

        // Mixer logic
        if (!isMusicOn) return;
        audioMixer.SetFloat("Music", 0f);
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
