using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.VirtualTexturing;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    [SerializeField] PathManager pathManager;

    [SerializeField] List<waypoint> ThePath;
    List<int> toDelete;

    waypoint selectedPoint = null;
    bool doRepaint = true;

    // Draws the path in the scene view
    private void OnSceneGUI()
    {
        ThePath = pathManager.GetPath();
        DrawPath(ThePath);
    }

    // Initializes values and clears the deletion list on enable
    private void OnEnable()
    {
        pathManager = target as PathManager;
        toDelete = new List<int>();
    }

    // Custom inspector GUI for managing waypoints in the path
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        ThePath = pathManager.GetPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        DrawGUIForPoints();

        // Button to add a new point to the path
        if (GUILayout.Button("Add point to path"))
        {
            pathManager.CreateAddPoint();
        }

        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    // Draws GUI elements for each waypoint in the path
    void DrawGUIForPoints()
    {
        if (ThePath != null && ThePath.Count > 0)
        {
            for (int i = 0; i < ThePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                waypoint p = ThePath[i];

                Color c = GUI.color;
                if (selectedPoint == p) GUI.color = Color.green;

                Vector3 oldPos = p.GetPos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);

                if (EditorGUI.EndChangeCheck()) p.SetPos(newPos);

                // Button to remove a waypoint
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i);
                }

                GUI.color = c;
                EditorGUILayout.EndHorizontal();

            }
        }

        // Deletes waypoints that are marked for removal
        if (toDelete.Count > 0)
        {
            foreach (int i in toDelete)
                ThePath.RemoveAt(i);
            toDelete.Clear();
        }
    }

    // Draws the entire path and connects waypoints with lines
    public void DrawPath(List<waypoint> path)
    {
        if (path != null)
        {
            int current = 0;
            foreach (waypoint wp in path)
            {
                // Draw the current waypoint
                doRepaint = DrawPoint(wp);
                int next = (current + 1) % path.Count;
                waypoint wpnext = path[next];

                // Connect this waypoint to the next with a line
                DrawPathLine(wp, wpnext);
                current += 1;
            }
            if (doRepaint) Repaint();
        }
    }

    // Draws a line between two waypoints
    public void DrawPathLine(waypoint p1, waypoint p2)
    {
        Color c = Handles.color;
        Handles.color = Color.gray;
        Handles.DrawLine(p1.GetPos(), p2.GetPos());
        Handles.color = c;
    }

    // Draws or selects a single waypoint in the scene view
    public bool DrawPoint(waypoint p)
    {
        bool isChanged = false;

        if (selectedPoint == p)
        {
            Color c = Handles.color;
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Vector3 oldpos = p.GetPos();
            Vector3 newpos = Handles.PositionHandle(oldpos, Quaternion.identity);

            float handleSize = HandleUtility.GetHandleSize(newpos);
            Handles.SphereHandleCap(-1, newpos, Quaternion.identity, 0.4f * handleSize
                , EventType.Repaint);

            if (EditorGUI.EndChangeCheck())
            {
                p.SetPos(newpos);
            }

            Handles.color = c;
        }
        else
        {
            Vector3 currPos = p.GetPos();
            float handleSize = HandleUtility.GetHandleSize(currPos);
            if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize
                , 0.25f * handleSize, Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }
        }
        return isChanged;
    }
}
