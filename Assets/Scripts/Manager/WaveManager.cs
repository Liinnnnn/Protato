using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegments> waveSegments;
}
[Serializable]
public struct WaveSegments
{
    public Vector2 tStartEnd;
    public float spawnRate;
    public GameObject enemy;
}
public class WaveManager : MonoBehaviour,IGameStateListener
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Settings")]
    [SerializeField] private float waveDuration;
    [SerializeField] private Player Player;
    [SerializeField] private Wave[] waves;
    private List<float> localCounter = new List<float>();
    private float timer;
    private bool isTimerOn;
    private int currentWave;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(!isTimerOn) return;
        if(timer < waveDuration)
        {
            ManageCurrentWave();
        }else
        {
            StartNextWave();
        }
    }
    private void StartWave(int v)
    {
        localCounter.Clear();
        foreach(WaveSegments segments in waves[v].waveSegments)
        {
            localCounter.Add(1);
        }
        
        timer = 0;
        isTimerOn = true;
    }

    private void ManageCurrentWave()
    {
        Wave Wave = waves[currentWave];
        for (int i = 0; i < Wave.waveSegments.Count; i++)
        {
            WaveSegments segments = Wave.waveSegments[i];

            float tStart = segments.tStartEnd.x / 100 * waveDuration;
            float tEnd = segments.tStartEnd.y / 100 * waveDuration;

            if(timer < tStart || timer > tEnd)
                continue;
            float timeSinceStartSpawn = timer - tStart;
            float spawnDelay = 1f / segments.spawnRate;
            if(timeSinceStartSpawn / spawnDelay > localCounter[i]) 
            {
                Instantiate(segments.enemy,SpawnPosition(),Quaternion.identity,transform);
                localCounter[i]++;
            }

        }
        timer += Time.deltaTime;
    }
    private void StartNextWave()
    {
        isTimerOn = false;
        currentWave++;
        if(currentWave >= waves.Length)
        {
            GameManager.instance.SetGameState(GameState.COMPLETE);
        }
        else
        { 
            GameManager.instance.WaveCompleteCallBack();
            timer = 0;
            // StartCoroutine(StartWave(currentWave));
        } 
    }
    
    private Vector2 SpawnPosition()
    {
        Vector2 direction = UnityEngine.Random.onUnitSphere;
        Vector2 offs = direction * UnityEngine.Random.Range(6,9);
        Vector2 targetPos = (Vector2)Player.transform.position + offs;
        return targetPos;
    }


    public void GameStateChangeCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME :
                if(timer == 0)
                StartWave(currentWave);
                else ManageCurrentWave();
                break;
            case GameState.SHOP :
            case GameState.MENU :
            case GameState.COMPLETE :
                GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
                if (e!=null)
                {
                    foreach (GameObject obj in e)
                    {
                        Destroy(obj);
                    }
                }
                break;
        }
    }
}
