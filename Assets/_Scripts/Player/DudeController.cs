/*---------------------------------------------------------
File Name: DudeController.cs
Purpose: Controls player specific systems needed for gameplay.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeController : MonoBehaviour {

    //movement related
    private bool m_isJumping;
    private Vector3 m_StartNode, m_FinishNode, m_ApexNode;
    public float m_JumpTime;
    private float m_CurrentJumpProgress;

    //gameplay related
    private bool m_isPlayerAlive;
    public GameObject m_Dudesplosion;
    public GameObject m_CharacterModel;

    //read-only access to see if the player is currently jumping or not.
    public bool IsJumping
    {
        get
        {
            return m_isJumping;
        }
    }

    public bool IsPlayerAlive
    {
        get
        {
            return m_isPlayerAlive;
        }

        set
        {
            m_isPlayerAlive = value;
        }
    }

    private void Start()
    {
        m_isJumping = false;
        m_CurrentJumpProgress = 0.0f;
        m_isPlayerAlive = true;
    }

    /// <summary>
    /// Sets the node to be moved to and stars the movement process
    /// </summary>
    /// <param name="startNode">Node on the board the player is starting at</param>
    /// <param name="endNode">Node on the board wher ethe player should end up</param>
    public void MoveTo(NodeInfo startNode, NodeInfo endNode)
    {
        //get the start and end nodes
        m_StartNode = startNode.gameObject.transform.position;
        m_FinishNode = endNode.gameObject.transform.position;
        //get the distance between the nodes, to calculate the apex
        float distance = Vector3.Distance(m_StartNode, m_FinishNode);
        //figure out the apex
        m_ApexNode = new Vector3(m_FinishNode.x + ((m_StartNode.x - m_FinishNode.x) * (distance * 0.5f)),m_FinishNode.y + distance, m_FinishNode.z + (m_StartNode.z - m_FinishNode.z) * (distance * 0.5f));
        //set that the jump has started
        m_isJumping = true;
    }

    private void FixedUpdate()
    {
        //if we are currently jumping, process the jump
        if(m_isJumping)
        {
            //get the total progress of the jump
            m_CurrentJumpProgress += Time.deltaTime * (1 / m_JumpTime);
            //figure out the Lerp values. Like a bezier curve.
            Vector3 firstLerp = Vector3.Lerp(m_StartNode, m_ApexNode, m_CurrentJumpProgress);
            Vector3 secondLerp = Vector3.Lerp(m_ApexNode, m_FinishNode, m_CurrentJumpProgress);
            Vector3 lastlerp = Vector3.Lerp(firstLerp, secondLerp, m_CurrentJumpProgress);

            //set the new position along the jump curve.
            transform.position = lastlerp;

            //if the jump is finished, then reset the jump stuff.
            if(m_CurrentJumpProgress >= 1.0)
            {
                m_isJumping = false;
                m_CurrentJumpProgress = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Enemy")
        {
            //Debug.Log("Hit!");
            m_isPlayerAlive = false;
        }
    }

    /// <summary>
    /// stuff to do when the player dies
    /// </summary>
    public void DudeDeath()
    {
        GameObject explosion = Instantiate(m_Dudesplosion, gameObject.transform.position + transform.forward, Quaternion.identity);
        m_CharacterModel.SetActive(false);
        Destroy(explosion, 3.0f);
    }
}
