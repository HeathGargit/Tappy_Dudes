/*---------------------------------------------------------
File Name: EnemyMovement.cs
Purpose: Move the enemies that have been spanwed.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    //variables for direction and speed of enemy movment
    private float m_Direction;
    private float m_Speed;

    //accessors
    public float Direction
    {
        get
        {
            return m_Direction;
        }

        set
        {
            m_Direction = value;
        }
    }
    public float Speed
    {
        get
        {
            return m_Speed;
        }

        set
        {
            m_Speed = value;
        }
    }


    // Update is called once per frame
    void FixedUpdate ()
    {
        //move the enemies each fixed frame
        Vector3 newPos = new Vector3((m_Speed * Time.fixedDeltaTime) * m_Direction, 0.0f, 0.0f);
        gameObject.transform.position += newPos;
	}

    /// <summary>
    /// Initialise enemy movement
    /// </summary>
    /// <param name="direction">Direction on X axis of movement</param>
    /// <param name="speed">movement speed</param>
    public void Init(float direction, float speed)
    {
        m_Direction = direction;
        m_Speed = speed;
    }
}
