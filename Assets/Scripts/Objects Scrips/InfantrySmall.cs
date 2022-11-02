using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantrySmall : MonoBehaviour
{
    private int life, attack, speed, cost, range, type, x, startx, y, starty, currentPathIndex;
    private List<Vector3> pathVectorList;
    [SerializeField] private GameObject bullet, bulletInstance;
    private List<PathNode> NodesInRange;
    private bool canMove = true, canShoot = true;

    void Start()
    {
        life = 4;
        attack = 1;
        speed = 2;
        cost = 1;
        range = 2;
        speed *= 35;
        type = 0;
    }

    public int GetUnitType()
    {
        return type;
    }

    #region PositionsFunctions
    public void UpdatePosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        startx = x;
        starty = y;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public int GetStartX()
    {
        return startx;
    }

    public int GetStartY()
    {
        return starty;
    }
    #endregion

    #region MovementFunctions
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
        Movement();
    }

    private void Movement()
    {
        if (pathVectorList != null && canMove)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + speed * Time.deltaTime * moveDir;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                }
            }
        }
    }

    private void StopMoving()
    {
        pathVectorList = null;
    }
    #endregion

    #region AttackFunctions
    private void GetCurrentNode()
    {
        PathNode currentNode = Pathfinding.Instance.GetGrid().GetGridObject(GetPosition());
        if (currentNode.GetX() != x || currentNode.GetY() != y)
        {
            if (Pathfinding.Instance.GetGrid().GetGridObject(x, y).GetValue() != 3)
            {
                Pathfinding.Instance.GetGrid().SetGridObject(x, y, new PathNode(Pathfinding.Instance.GetGrid(), x, y, 0));
            }
        }
        if (Pathfinding.Instance.GetGrid().GetGridObject(x, y).GetValue() != 3)
        {
            x = currentNode.GetX();
            y = currentNode.GetY();
            Pathfinding.Instance.GetGrid().SetGridObject(x, y, new PathNode(Pathfinding.Instance.GetGrid(), x, y, -1));
        }
        NodesInRange = Pathfinding.Instance.GetNodesInRange(currentNode, range);
        StartShooting();
    }

    private void StartShooting()
    {
        foreach (PathNode node in NodesInRange)
        {
            if (node != null && (node.GetValue() == 4 || node.GetValue() == 5) && canShoot == true)
            {
                canMove = false;
                canShoot = false;
                StartAttacking(node);
            }
        }
    }

    public void StartAttacking(PathNode node)
    {
        bulletInstance = Instantiate(bullet, Pathfinding.Instance.GetGrid().GetWorldPosition(x, y) + new Vector3(Pathfinding.Instance.GetGrid().GetCellSize(),
                                     Pathfinding.Instance.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
        Vector3 shootDir = (Pathfinding.Instance.GetGrid().GetWorldPosition(node.GetX(), node.GetY()) - Pathfinding.Instance.GetGrid().GetWorldPosition(x, y)).normalized;
        bulletInstance.GetComponent<BulletBehaviour>().Setup(shootDir, attack);
        StartCoroutine(SpawnBulletTimer());
    }

    private bool CheckEnemies()
    {
        bool result = false;
        foreach (PathNode node in NodesInRange)
        {
            if (node.GetValue() == 4 || node.GetValue() == 5)
            {
                result = true;
            }
        }
        return result;
    }

    public IEnumerator SpawnBulletTimer()
    {
        yield return new WaitForSeconds(3f);
        canShoot = true;
        if (!CheckEnemies())
        {
            canMove = true;
        }
    }
    #endregion

    #region DamageFunctions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet")
        {
            int damage = collision.gameObject.GetComponent<BulletBehaviour>().GetDamage();
            LoseHealth(damage);
            Destroy(collision.gameObject);
        }
    }

    public void LoseHealth(int damage)
    {
        life -= damage;
    }
    #endregion
    public void Delete(Grid<PathNode> grid)
    {
        grid.SetGridObject(x, y, new PathNode(grid, x, y, 0));
        Testing.Instance.IFTSUnits.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void Erase(Grid<PathNode> grid)
    {
        grid.SetGridObject(x, y, new PathNode(grid, x, y, 0));
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        GetCurrentNode();
        if (life <= 0)
        {
            Delete(Pathfinding.Instance.GetGrid());
        }
    }
}
