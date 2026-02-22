using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mapChunkPrefs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenMap();
    }
    private void GenMap()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                genMapChunk(i,j);
            }   
        }
    }
    private void genMapChunk(int x,int y)
    {
        Vector3 spawnPos = new Vector3(x * 20 ,y * 10) ;
        Instantiate(mapChunkPrefs,spawnPos,Quaternion.identity,transform);
    }
}
