using System;
using UnityEngine;

public class NavMeshMover : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float distThreshold = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMap();
    }
    private void UpdateMap()
    {
        Vector3 dist = playerTransform.position - transform.position;
        float xThreshold = distThreshold * 33.8f;
        float yThreshold = distThreshold * 15.8f;
        if(Math.Abs(dist.x) > xThreshold)
        {
            transform.position += Vector3.right * xThreshold* 2 *Math.Sign(dist.x);
        }
        if(Math.Abs(dist.y) > yThreshold)
        {
            transform.position += Vector3.up * yThreshold * 2 *Math.Sign(dist.y);
        }
    }
}
