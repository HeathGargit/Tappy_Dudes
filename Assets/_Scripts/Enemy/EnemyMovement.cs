using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    private float m_Direction;
    private float m_Speed;

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

    private void Start()
    {
        /*m_Direction = 0;
        m_Speed = 0;*/
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        Vector3 newPos = new Vector3((m_Speed * Time.fixedDeltaTime) * m_Direction, 0.0f, 0.0f);
        gameObject.transform.position += newPos;
	}

    public void Init(float direction, float speed)
    {
        m_Direction = direction;
        m_Speed = speed;
    }
}
