using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo : MonoBehaviour {

    [SerializeField]
    private Vector2Int m_GridLocation;

    public Vector2Int GridLocation
    {
        get
        {
            return m_GridLocation;
        }

        set
        {
            m_GridLocation = value;
        }
    }
}
