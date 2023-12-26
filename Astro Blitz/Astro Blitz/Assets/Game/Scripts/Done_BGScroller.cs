﻿using UnityEngine;
using System.Collections;

public class Done_BGScroller : MonoBehaviour
{
    public float scrollSpeed;
    public float tileSizeZ;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        scrollSpeed = Mathf.Lerp(scrollSpeed, -30, Time.deltaTime / 120);

        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        transform.position = startPosition + Vector3.forward * newPosition;
    }
}
