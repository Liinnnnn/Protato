using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Action onPaused;
    public static Action onResume;
    public Difficulty currentDiff;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetGameState(GameState.MENU);
        Application.targetFrameRate = 60;
    }

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
        PlayerLevel.onLevelUp += LevelUpAndChestCallback;
    }
    void OnDestroy()
    {
        PlayerLevel.onLevelUp -= LevelUpAndChestCallback; 
    }
    public void startGame()
    {
        Time.timeScale =1;
        SetGameState(GameState.GAME);
    }
    public void openShop()
    {
        SetGameState(GameState.SHOP);
    }
    public void openWeapon()
    {
        SetGameState(GameState.WEAPONCHOSE);
    }
    public void openUpgrade()
    {
        SetGameState(GameState.WAVETRANS);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetGameState(GameState gameState)
    {
        IEnumerable<IGameStateListener> nameState =  
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).
        OfType<IGameStateListener>();
        foreach(IGameStateListener gameStateListener in nameState)
        {
            gameStateListener.GameStateChangeCallBack(gameState);
        }
    }
    public void SetDifficulty(Difficulty difficulty)
    {
        IEnumerable<IDifficultyListener> diff =  
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).
        OfType<IDifficultyListener>();
        foreach(IDifficultyListener diffListener in diff)
        {
           diffListener.DiffcultySettingCallBack(difficulty);
        }
        currentDiff = difficulty;
    }

    public void chooseDifficulty(int val)
    {
        switch (val)
        {
            case 1 :
                SetDifficulty(Difficulty.EASY);
                break;
            case 2:
                SetDifficulty(Difficulty.NORMAL);
                break;
            case 3:
                SetDifficulty(Difficulty.HARD);
                break;
            default :
                SetDifficulty(Difficulty.EASY);
                break;
        }
        Debug.Log(currentDiff);
    }
    public void pauseGame()
    {
        Time.timeScale = 0;
        onPaused?.Invoke();
    }
    public void resumeGame()
    {
        Time.timeScale = 1;
        onResume?.Invoke();
    }
    public void restartFromPause()
    {
        ManagerGameOver();
    }
    public void ManagerGameOver()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WaveCompleteCallBack()
    {
        Time.timeScale = 0;
        SetGameState(GameState.SHOP);
    }
    public void LevelUpAndChestCallback()
    {
        Time.timeScale = 0;
        SetGameState(GameState.WAVETRANS);
    }

}
public interface IGameStateListener
{
    void GameStateChangeCallBack(GameState gameState);
}
public interface IDifficultyListener
{
    void DiffcultySettingCallBack(Difficulty difficulty);
}