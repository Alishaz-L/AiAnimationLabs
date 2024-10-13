using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class waypoint
{
    [SerializeField] public Vector3 pos;

    // Sets the position of the waypoint to newPos
    public void SetPos(Vector3 newPos)
    {
        pos = newPos;
    }

    // Returns the current position of the waypoint
    public Vector3 GetPos()
    {
        return pos;
    }

    // Constructor that initializes position to (0, 0, 0)
    public waypoint()
    {
        pos = new Vector3(0, 0, 0);
    }
}
