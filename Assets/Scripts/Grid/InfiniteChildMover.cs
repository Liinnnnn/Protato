using System;
using UnityEngine;

public class InfiniteChildMover : MonoBehaviour
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
        UpdateChild();
    }
    private void UpdateChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            Vector3 dist = playerTransform.position - child.position;

            float xThreshold = distThreshold * 20;
            float yThreshold = distThreshold * 10;
            if(Math.Abs(dist.x) > xThreshold)
            {
                child.position += Vector3.right * xThreshold* 2 *Math.Sign(dist.x);
            }
            if(Math.Abs(dist.y) > yThreshold)
            {
                child.position += Vector3.up * yThreshold * 2 *Math.Sign(dist.y);
            }
        }
    }
}
