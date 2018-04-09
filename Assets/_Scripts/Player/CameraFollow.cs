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

    public void SetTarget(GameObject target)
    {
        m_Target = target.transform;
        m_IsFollowing = true;
    }

    public void SetOffset(GameObject target)
    {
        m_Offset = transform.position - target.transform.position + new Vector3 (-2, 0, 0);
        m_SteadyY = m_Offset.y;
    }
}
