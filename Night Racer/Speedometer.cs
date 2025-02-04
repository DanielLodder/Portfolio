using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speedometer : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] private GameObject m_handle;
    [SerializeField] private float m_currenthandleRot;
    [SerializeField] private float m_maxHandleRot;
    [SerializeField] private float m_speed;
    private void Update()
    {
        SpeedometerHandle();
    }
    private void SpeedometerHandle()
    {
        Vector2 vel = new Vector2(playerMovement.m_rigidbody.velocity.x, playerMovement.m_rigidbody.velocity.z);
        m_speed = vel.magnitude;
        m_currenthandleRot = m_speed * 3;
        m_handle.transform.rotation = Quaternion.Euler(180, 0, -30 + m_currenthandleRot);
        if (m_currenthandleRot >= m_maxHandleRot)
        {
            m_currenthandleRot = m_maxHandleRot;
        }
    }
}
