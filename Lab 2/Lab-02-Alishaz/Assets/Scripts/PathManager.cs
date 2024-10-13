using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<waypoint> path;

    public GameObject prefab;
    int currentPointIndex = 0;

    public List<GameObject> prefabPoints;

    // Returns the list of waypoints in the path
    public List<waypoint> GetPath()
    {
        if (path == null)
            path = new List<waypoint>();

        return path;
    }

    // Creates and adds a new waypoint to the path
    public void CreateAddPoint()
    {
        waypoint go = new waypoint();
        path.Add(go);
    }

    // Returns the next waypoint target in the path
    public waypoint GetNextTarget()
    {
        int nextPointIndex = (currentPointIndex + 1) % (path.Count);
        currentPointIndex = nextPointIndex;
        return path[nextPointIndex];
    }

    // Instantiates prefabs at each waypoint position on start
    private void Start()
    {
        prefabPoints = new List<GameObject>();
        foreach (waypoint p in path)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = p.pos;
            prefabPoints.Add(go);
        }
    }

    // Updates the position of each prefab to match waypoint positions
    private void Update()
    {
        for (int i = 0; i < path.Count; i++)
        {
            waypoint p = path[i];
            GameObject g = prefabPoints[i];
            g.transform.position = p.pos;
        }
    }
}
