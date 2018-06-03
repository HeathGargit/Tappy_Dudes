/*---------------------------------------------------------
File Name: CameraFollow.cs
Purpose: Attaches to the camera to follow the player.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform m_Target;
    public float m_SmoothSpeed;
    private Vector3 m_Offset;
    private float m_SteadyY;
    private bool m_IsFollowing;

    private void Start()
    {
        m_IsFollowing = false;
    }

    private void FixedUpdate()
    {
        if(m_IsFollowing)
        {
            Vector3 desiredPosition = m_Target.position + m_Offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_SmoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, m_SteadyY, smoothedPosition.z);
        }
    }

    /// <summary>
    /// set the target (player) to follow
    /// </summary>
    /// <param name="target">the target to follow</param>
    public void SetTarget(GameObject target)
    {
        m_Target = target.transform;
        m_IsFollowing = true;
    }

    /// <summary>
    /// Sets the offest to keep from the player
    /// </summary>
    /// <param name="target"></param>
    public void SetOffset(GameObject target)
    {
        m_Offset = transform.position - target.transform.position + new Vector3 (-2, 0, 0);
        m_SteadyY = m_Offset.y;
    }
}
