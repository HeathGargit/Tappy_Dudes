using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private bool m_Tap, m_SwipeUp, m_SwipeDown, m_SwipeRight, m_SwipeLeft, m_IsDragging = false;
    private Vector2 m_StartTouch, m_SwipeDelta;

    public bool Tap
    {
        get
        {
            return m_Tap;
        }
    }
    public bool SwipeUp
    {
        get
        {
            return m_SwipeUp;
        }
    }
    public bool SwipeDown
    {
        get
        {
            return m_SwipeDown;
        }
    }
    public bool SwipeRight
    {
        get
        {
            return m_SwipeRight;
        }
    }
    public bool SwipeLeft
    {
        get
        {
            return m_SwipeLeft;
        }
    }

    public void ProcessTouchInput()
    {
        //reset the bools
        m_Tap = false;
        m_SwipeUp = false;
        m_SwipeDown = false;
        m_SwipeRight = false;
        m_SwipeLeft = false;

        //player swipe control
        //if the mouse button is pressed this frame
        if(Input.GetMouseButtonDown(0))
        {
            //set that the "tap" has been started
            m_Tap = true;
            //set that the drag has been started
            m_IsDragging = true;
            //Get the start of the tap position
            m_StartTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) //else if the mouse button ahs been released this fram
        {
            m_IsDragging = false;
            Reset();
        }

        //mobile input swipe control
        if(Input.touches.Length > 0)
        {
            //if the touch began this frame
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                //set thast the "tap" has started
                m_Tap = true;
                //set that the drag has been started
                m_IsDragging = true;
                //Get the start position of the tap
                m_StartTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) //else if the touch finished this frame or is cancelled.
            {
                m_IsDragging = false;
                Reset();
            }
        }

        //Calculate how far a drag has gone
        m_SwipeDelta = Vector2.zero;
        //if we are dragging
        if(m_IsDragging)
        {
            //if touch drag
            if(Input.touches.Length > 0)
            {
                //set drag delta (how far drag has gone from drag start)
                m_SwipeDelta = Input.touches[0].position - m_StartTouch;
            }
            else if (Input.GetMouseButton(0)) //else if standalone player drag
            {
                //set drag delta (how far mouse has gone from drag start)
                m_SwipeDelta = (Vector2)Input.mousePosition - m_StartTouch;
            }
        }

        //has the deadzone been crossed
        if(m_SwipeDelta.magnitude > 125)
        {
            //check direction
            float x = m_SwipeDelta.x;
            float y = m_SwipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or Right
                if(x < 0)
                {
                    m_SwipeLeft = true;
                }
                else
                {
                    m_SwipeRight = true;
                }
            }
            else
            {
                //up or down
                if(y < 0)
                {
                    m_SwipeDown = true;
                }
                else
                {
                    m_SwipeUp = true;
                }
            }

            Reset();
        }
    }

    private void Reset()
    {
        m_StartTouch = Vector2.zero;
        m_SwipeDelta = Vector2.zero;
        m_IsDragging = false;
    }
}
