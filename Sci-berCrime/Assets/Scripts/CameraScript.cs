﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float m_fPlayerDistance;
    private Vector3 m_v3PlayerDifference;

    public float m_fZoomScalar = 5f;
    public float m_fZoomOffset = 8f;
    public float m_fCameraCenterTether = 1.2f;

    public Animator m_aAnimatior;

    [Header("Camera Objects")]
    public GameObject m_goCameraDolly;
    public GameObject m_goCameraPlane_Zoom;
    public GameObject m_goCameraPlane_Pan;

    [Header("Players")]
    public GameObject m_goPlayerOne;
    public GameObject m_goPlayerTwo;

    private void Update ()
    {
        if (m_goPlayerOne.GetComponent<PlayerController>().IsAlive && m_goPlayerTwo.GetComponent<PlayerController>().IsAlive)
        {
            // Calculates the distance between players
            m_v3PlayerDifference = m_goPlayerOne.transform.position - m_goPlayerTwo.transform.position;
            m_fPlayerDistance = m_v3PlayerDifference.magnitude;

            // Calculates the midpoint
            Vector3 averagePos = m_goPlayerOne.transform.position + m_goPlayerTwo.transform.position;
            averagePos /= 2;

            // Applies the zoom to the object
            m_goCameraPlane_Zoom.transform.position = m_goCameraDolly.transform.position
                + new Vector3(0, m_fPlayerDistance / m_fZoomScalar - m_fZoomOffset, -m_fPlayerDistance / m_fZoomScalar + m_fZoomOffset);

            // Applies the transform to the object
            m_goCameraPlane_Pan.transform.position = averagePos / m_fCameraCenterTether + m_goCameraPlane_Zoom.transform.position;
        }
        else if (m_goPlayerOne.GetComponent<PlayerController>().IsAlive && !m_goPlayerTwo.GetComponent<PlayerController>().IsAlive)
        {
            m_v3PlayerDifference = m_goPlayerOne.transform.position;
            m_goCameraPlane_Zoom.transform.position = m_goCameraDolly.transform.position + new Vector3(0, m_fZoomOffset);
            m_goCameraPlane_Pan.transform.position = m_v3PlayerDifference / m_fCameraCenterTether + m_goCameraPlane_Zoom.transform.position;
        }
        else if (!m_goPlayerOne.GetComponent<PlayerController>().IsAlive && m_goPlayerTwo.GetComponent<PlayerController>().IsAlive)
        {
            m_v3PlayerDifference = m_goPlayerTwo.transform.position;
            m_goCameraPlane_Zoom.transform.position = m_goCameraDolly.transform.position + new Vector3(0, m_fZoomOffset);
            m_goCameraPlane_Pan.transform.position = m_v3PlayerDifference / m_fCameraCenterTether + m_goCameraPlane_Zoom.transform.position;
        }
    }

    public void PlayExplosion ()
    {
        m_aAnimatior.Play("ExplosionShake");
    }
}
