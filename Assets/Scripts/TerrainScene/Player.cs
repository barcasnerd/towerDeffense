using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ref: https://drive.google.com/file/d/1WiF2LwM-6WvEnas9vw32YrYPly9K0Qrv/view

public class Player : MonoBehaviour
{
    List<Cell> path;
    [SerializeField]
    private float moveSpeed = 2f;
    public Vector2 GetPosition => transform.position;
    private bool startMoving = false;
    private Grid grid;
    private bool changedCells = false;
    private Rigidbody2D rb;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

    

    void FixedUpdate()
    {
        if (startMoving)
            Move();
    }



    public void starMoving(Grid grid, float speed)
    {
        
        this.grid = grid;
        calculatePath();
        startMoving = true;
        moveSpeed = speed;
    }

    private void calculatePath()
    {
        waypointIndex = 0;
        path = PathManager.Instance.FindPath(grid, (int)GetPosition.x, (int)GetPosition.y);
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PowerSource")
        {
            Debug.Log("Made it");
            path = null;
        }
    }

    private void Move()
    {
        // If player didn't reach last waypoint it can move
        // If player reached last waypoint then it stops
        if (path == null)
            return;

        if (waypointIndex <= path.Count - 1)
        {
            //Debug.Log("Moving to " + path[waypointIndex].transform.position.x.ToString() + " "
            //    + path[waypointIndex].transform.position.y.ToString());

            if (changedCells) {
                changedCells = false;
                if (!grid.isWalkable((int)path[waypointIndex].transform.position.x, (int)path[waypointIndex].transform.position.y))
                {
                    //Debug.Log("not walkable");
                    //path = null;
                    calculatePath();
                    return;
                } else
                {
                    grid.setBusyCell((int)path[waypointIndex - 1].transform.position.x,
                        (int)path[waypointIndex - 1].transform.position.y,
                        (int)path[waypointIndex].transform.position.x,
                        (int)path[waypointIndex].transform.position.y);
                }
                
            }
            // Move player from current waypoint to the next one
            // using MoveTowards method
            transform.position = Vector2.MoveTowards(transform.position,
               path[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime);

            // If player reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and player starts to walk to the next waypoint
            if (transform.position == path[waypointIndex].transform.position)
            {
                waypointIndex += 1;
                changedCells = true;
            }
        }
    }
}
