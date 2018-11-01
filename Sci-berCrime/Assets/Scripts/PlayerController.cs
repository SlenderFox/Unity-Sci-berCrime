﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool m_bIsAlive { get; set; }

    public bool isInShop = false;
    public bool m_bPlayerOne;

    public int m_iHealth = 100;
    public int m_iMaxHealth = 100;

    public float m_fMovementSpeed = 50.0f;

    // Refernce to the gun firing script
    public GunController m_gcGun;
    public UIController m_uicUIController;

    public GameObject m_goAimTarget;

    // Reference to the rigidbody
    public Rigidbody m_rbRigidBody;

    private void Awake ()
    {
        m_rbRigidBody = GetComponent<Rigidbody>();
        m_bIsAlive = true;
    }

    private void FixedUpdate ()
    {
        if (m_bIsAlive && !isInShop)
        {
            // All controls for player one
            if (m_bPlayerOne)
            {
                //----------Movement----------
                // Horizontal movement
                m_rbRigidBody.AddForce(new Vector3(Input.GetAxis("P1 LS Horizontal") * m_fMovementSpeed, 0, 0), ForceMode.Force);
                // Vertical movement
                m_rbRigidBody.AddForce(new Vector3(0, 0, Input.GetAxis("P1 LS Vertical") * m_fMovementSpeed), ForceMode.Force);

                //----------Rotation----------
                // Moves target object
                m_goAimTarget.transform.position = transform.position + new Vector3(Input.GetAxis("P1 RS Horizontal"), 0, Input.GetAxis("P1 RS Vertical"));

                // Calculates direction needed for facing
                Vector3 targetDir = m_goAimTarget.transform.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 9999, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
            // All controls for player two
            else
            {
                //----------Movement----------
                // Horizontal movement
                m_rbRigidBody.AddForce(new Vector3(Input.GetAxis("P2 LS Horizontal") * m_fMovementSpeed, 0, 0), ForceMode.Force);
                // Vertical movement
                m_rbRigidBody.AddForce(new Vector3(0, 0, Input.GetAxis("P2 LS Vertical") * m_fMovementSpeed), ForceMode.Force);

                //----------Rotation----------
                // Moves target object
                m_goAimTarget.transform.position = transform.position + new Vector3(Input.GetAxis("P2 RS Horizontal"), 0, Input.GetAxis("P2 RS Vertical"));

                // Calculates direction needed for facing
                Vector3 targetDir = m_goAimTarget.transform.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 9999, 0.0f);
                this.transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }

    private void Update ()
    {
        if (m_iHealth == 0)
        {
            m_bIsAlive = false;
            // Fix this
            gameObject.SetActive(false);
        }

        if (m_bIsAlive && !isInShop)
        {
            if (m_bPlayerOne)
            {
                // Calls a specific update function for the currently active gun
                if (m_gcGun && m_bIsAlive && Input.GetButton("P1 Button RB"))
                    m_gcGun.ActiveGunUpdate(gameObject);
            }
            else
            {
                // Calls a specific update function for the currently active gun
                if (m_gcGun && m_bIsAlive && Input.GetButton("P2 Button RB"))
                    m_gcGun.ActiveGunUpdate(gameObject);
            }
        }

        // Call an update function for each of the equiped guns
        if (m_gcGun)
            m_gcGun.GunUpdate();
    }

    private void OnCollisionEnter (Collision collision)
    {
        // Player dies if hit by enemy
        if (collision.gameObject.tag == "Enemy" && m_bIsAlive)
            TakeDamage(collision.gameObject.GetComponent<EnemyController>().m_iDamage);
    }

    // Applies damage to the object
    public void TakeDamage (int pDamage)
    {
        m_iHealth -= pDamage;
        if (m_iHealth < 0)
            m_iHealth = 0;

        if (m_bPlayerOne)
            m_uicUIController.SetPlayerOneHealth(m_iHealth);
        else
            m_uicUIController.SetPlayerTwoHealth(m_iHealth);
    }
}