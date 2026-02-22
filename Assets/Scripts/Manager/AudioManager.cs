using System;
using UnityEngine;
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
    public bool isSfxOn {get;private set;}
    public bool isMusicOn {get;private set;}
    void Awake()
    {
        instance = this;
        SettingManager.onMusicChanged += MusicChangedCallback;
        SettingManager.onSfxChanged += SFXChangedCallback;
    }
    private void PlayBGM(BGMMusic bGM, float volumn =1, bool loop = true)
    {
        audioSource.Stop();
        if (!isMusicOn) return;
        audioSource.volume = volumn;
        audioSource.loop = loop;
        audioSource.PlayOneShot(clips[Array.IndexOf(Enum.GetValues(typeof(BGMMusic)),bGM)]);
    }

    private void SFXChangedCallback(bool obj)
    {
        isSfxOn = obj;
    }

    private void MusicChangedCallback(bool obj)
    {
        isMusicOn = obj;
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
                PlayBGM(BGMMusic.SHOPBGM);
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
