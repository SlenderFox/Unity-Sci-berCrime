﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRenderer : MonoBehaviour
{
    // Object References
    public int distance;
    private float alpha;
    private Material m_mObjectRenderer;

    // Player references
    public GameObject m_goPlayerOne;
    public GameObject m_goPlayerTwo;

    private void Awake ()
    {
        m_mObjectRenderer = GetComponent<Renderer>().material;
        m_goPlayerOne = GameObject.FindGameObjectWithTag("PlayerOne");
        m_goPlayerTwo = GameObject.FindGameObjectWithTag("PlayerTwo");
    }

    private void Update ()
    {
        Vector3 playerOneDistance = transform.position - m_goPlayerOne.transform.position;
        Vector3 playerTwoDistance = transform.position - m_goPlayerTwo.transform.position;

        if (playerOneDistance.magnitude <= distance || playerTwoDistance.magnitude <= distance && m_mObjectRenderer.color.a != 0.01f)
        {
            // Object opacity
            // This coroutine sets the object's opacity to 99% over a second
            StartCoroutine(SetOpacity(0.01f, 0.5f));
        }
        else if (playerOneDistance.magnitude >= distance && playerTwoDistance.magnitude >= distance && m_mObjectRenderer.color.a < 1.0f)
        {
            // Object opaqueness
            // This coroutine sets the object's opacity to 100% over a second
            StartCoroutine(SetOpacity(1.0f, 0.5f));
        }
    }

    IEnumerator SetOpacity (float AlphaValue, float AlphaTime)
    {
        // This loops through to animate the fading effect of the objects going transparent

        alpha = transform.GetComponent<Renderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / AlphaTime))
        {
            Color opacity = m_mObjectRenderer.color;
            opacity.a = Mathf.Lerp(alpha, AlphaValue, t);
            m_mObjectRenderer.color = opacity;
            yield return null;
        }
    }
}
