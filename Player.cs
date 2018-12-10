using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GridMap grid;
    public PathFinder pathFinder;
    Vector3 destination = new Vector3(0, 0, -0.1f);

    private void Start()
    {
        
    }

    private void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, destination, 1);
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathFinder.FindPath(transform.position, clickPoint);
            destination = new Vector3(pathFinder.destination.x, pathFinder.destination.y, transform.position.z);
            
        }
    }
}
