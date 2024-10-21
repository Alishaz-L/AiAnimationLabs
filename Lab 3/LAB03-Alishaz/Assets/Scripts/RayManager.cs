using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayManager : MonoBehaviour
{
    [SerializeField] GameObject Target;
    private Camera cam;

    // Initializes the camera as the main camera in the scene
    void Start()
    {
        cam = Camera.main;
    }

    // Continuously checks for a mouse click to set the target's position
    void Update()
    {
        SetTarget();
    }

    // Moves the target to the position where the raycast hits after a mouse click
    private void SetTarget()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Target.transform.position = hit.point;
            }
        }
    }
}
