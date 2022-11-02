using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Testing : MonoBehaviour
{

    [SerializeField] private GameObject Object1, Object2, pcInstance, iftsInstance, ifthInstance, iftkInstance, twhInstance, twsInstance, wallInstance;
    private TextMeshProUGUI tcredits, tecredits;
    private bool SelectionActive = false;
    private Pathfinding pathfinding;
    List<PathNode> minpath;
    List<PathNode> minpathkiller;
    private int type = 3, credits, cost, enemycredits, enemycost, random, randomx, randomy, minpathCost, minpathKillerCost, x, y;
    public int starting, changeenemies;
    public string resultados;
    #region Lists
    public List<GameObject> PWUnits = new List<GameObject>();
    public List<GameObject> TWHUnits = new List<GameObject>();
    public List<GameObject> TWSUnits = new List<GameObject>();
    public List<int> TWHUnitsX = new List<int>();
    public List<int> TWHUnitsY = new List<int>();
    public List<int> TWSUnitsX = new List<int>();
    public List<int> TWSUnitsY = new List<int>();
    public List<GameObject> IFTSUnits = new List<GameObject>();
    public List<GameObject> IFTHUnits = new List<GameObject>();
    public List<GameObject> IFTKUnits = new List<GameObject>();
    #endregion
    #region ObjectsScripts
    private PowerCore pcscript;
    private TowerSmall twsscript;
    private TowerHeavy twhscript;
    private InfantrySmall iftsscript;
    private InfantryHeavy ifthscript;
    private InfantryKiller iftkscript;
    #endregion
    public static Testing Instance { get; private set; }

    void Start()
    {
        Instance = this;
        tcredits = GameManager.Instance.tcredits;
        tecredits = GameManager.Instance.tecredits;
        changeenemies = 0;
    }

    private void Update()
    {
        switch (GameManager.Instance._gameState)
        {
            #region Prepare
            case GameManager.GameState.prepare:
                if (starting == 0)
                {
                    resultados = "";
                    credits = 15;
                    enemycredits = 15;
                    #region BuildingGrid
                    pathfinding = new Pathfinding(18, 5, 0);
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            pathfinding.SetNodeValue(i, j, new PathNode(pathfinding.GetGrid(), i, j, 1));
                        }
                    }

                    for (int i = 17; i >= 12; i--)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            pathfinding.SetNodeValue(i, j, new PathNode(pathfinding.GetGrid(), i, j, 2));
                        }
                    }

                    for (int i = 1; i < 4; i++)
                    {
                        pathfinding.SetNodeValue(15, i, new PathNode(pathfinding.GetGrid(), 15, i, 3));
                        pathfinding.GetNode(15, i).SetIsWalkable(!pathfinding.GetNode(15, i).isWalkable);
                        Instantiate(wallInstance, pathfinding.GetGrid().GetWorldPosition(15, i) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                    pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                    }
                    #endregion
                    #region PowerCore
                    Object1 = Instantiate(pcInstance, pathfinding.GetGrid().GetWorldPosition(17, 2) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                          pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                    pcscript = Object1.GetComponent<PowerCore>();
                    pcscript.UpdatePosition(17, 2);
                    pathfinding.GetGrid().SetGridObject(pathfinding.GetGrid().GetWorldPosition(17, 2), new PathNode(pathfinding.GetGrid(), 17, 2, 5));
                    pathfinding.GetNode(17, 2).SetIsWalkable(!pathfinding.GetNode(17, 2).isWalkable);
                    PWUnits.Add(Object1);
                    #endregion
                    if (changeenemies != 0)
                    {
                        if (changeenemies == 3)
                        {
                            changeenemies = 0;
                        }
                        else
                        {
                            for (int i = 0; i < TWSUnitsX.Count; i++)
                            {
                                randomx = TWSUnitsX[i];
                                randomy = TWSUnitsY[i];
                                Object1 = Instantiate(twsInstance, pathfinding.GetGrid().GetWorldPosition(randomx, randomy) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                          pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                twsscript = Object1.GetComponent<TowerSmall>();
                                twsscript.UpdatePosition(randomx, randomy);
                                TWSUnits.Add(Object1);
                                pathfinding.GetGrid().SetGridObject(pathfinding.GetGrid().GetWorldPosition(randomx, randomy),
                                                                    new PathNode(pathfinding.GetGrid(), randomx, randomy, 4));
                                pathfinding.GetNode(randomx, randomy).SetIsWalkable(!pathfinding.GetNode(randomx, randomy).isWalkable);
                            }
                            for (int i = 0; i < TWHUnitsX.Count; i++)
                            {
                                randomx = TWHUnitsX[i];
                                randomy = TWHUnitsY[i];
                                Object1 = Instantiate(twhInstance, pathfinding.GetGrid().GetWorldPosition(randomx, randomy) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                          pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                twhscript = Object1.GetComponent<TowerHeavy>();
                                twhscript.UpdatePosition(randomx, randomy);
                                TWHUnits.Add(Object1);
                                pathfinding.GetGrid().SetGridObject(pathfinding.GetGrid().GetWorldPosition(randomx, randomy),
                                                                    new PathNode(pathfinding.GetGrid(), randomx, randomy, 4));
                                pathfinding.GetNode(randomx, randomy).SetIsWalkable(!pathfinding.GetNode(randomx, randomy).isWalkable);
                            }
                            changeenemies += 1;
                        }
                    }
                    if (changeenemies == 0)
                    {
                        TWSUnitsX.Clear();
                        TWHUnitsX.Clear();
                        TWSUnitsY.Clear();
                        TWHUnitsY.Clear();
                        #region EnemyPrepare
                        while (enemycredits - enemycost >= 0)
                        {
                            tecredits.text = "Enemy Credits: " + enemycredits;
                            randomx = Random.Range(12, 18);
                            randomy = Random.Range(0, 5);
                            if (pathfinding.GetGrid().GetGridObject(randomx, randomy).GetValue() == 2)
                            {
                                random = Random.Range(0, 2);
                                if (random == 0)
                                {
                                    enemycost = 2;
                                    if (enemycredits - enemycost >= 0)
                                    {
                                        Object1 = Instantiate(twsInstance, pathfinding.GetGrid().GetWorldPosition(randomx, randomy) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                          pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                        twsscript = Object1.GetComponent<TowerSmall>();
                                        twsscript.UpdatePosition(randomx, randomy);
                                        TWSUnits.Add(Object1);
                                        TWSUnitsX.Add(randomx);
                                        TWSUnitsY.Add(randomy);
                                        pathfinding.GetGrid().SetGridObject(pathfinding.GetGrid().GetWorldPosition(randomx, randomy),
                                                                            new PathNode(pathfinding.GetGrid(), randomx, randomy, 4));
                                        pathfinding.GetNode(randomx, randomy).SetIsWalkable(!pathfinding.GetNode(randomx, randomy).isWalkable);
                                        enemycredits -= enemycost;
                                    }
                                }
                                else
                                {
                                    enemycost = 3;
                                    if (enemycredits - enemycost >= 0)
                                    {
                                        Object1 = Instantiate(twhInstance, pathfinding.GetGrid().GetWorldPosition(randomx, randomy) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                          pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                        twhscript = Object1.GetComponent<TowerHeavy>();
                                        twhscript.UpdatePosition(randomx, randomy);
                                        TWHUnits.Add(Object1);
                                        TWHUnitsX.Add(randomx);
                                        TWHUnitsY.Add(randomy);
                                        pathfinding.GetGrid().SetGridObject(pathfinding.GetGrid().GetWorldPosition(randomx, randomy),
                                                                            new PathNode(pathfinding.GetGrid(), randomx, randomy, 4));
                                        pathfinding.GetNode(randomx, randomy).SetIsWalkable(!pathfinding.GetNode(randomx, randomy).isWalkable);
                                        enemycredits -= enemycost;
                                    }
                                }
                            }
                            tecredits.text = "Enemy Credits: " + enemycredits;
                            enemycost = 2;
                        }
                        #endregion
                        changeenemies += 1;
                    }
                    #region AutoPlayerPrepare
                    while (credits - cost >= 0)
                    {
                        tcredits.text = "Credits: " + credits;
                        randomx = Random.Range(0, 5);
                        randomy = Random.Range(0, 5);
                        if (pathfinding.GetGrid().GetGridObject(randomx, randomy).GetValue() == 1)
                        {
                            random = Random.Range(0, 3);
                            if (random == 0)
                            {
                                cost = 1;
                                if (credits - cost >= 0)
                                {
                                    Object1 = Instantiate(iftsInstance, pathfinding.GetGrid().GetWorldPosition(randomx, randomy) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                      pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                    iftsscript = Object1.GetComponent<InfantrySmall>();
                                    iftsscript.UpdatePosition(randomx, randomy);
                                    IFTSUnits.Add(Object1);
                                    pathfinding.GetGrid().SetGridObject(pathfinding.GetGrid().GetWorldPosition(randomx, randomy),
                                                                        new PathNode(pathfinding.GetGrid(), randomx, randomy, -1));
                                    pathfinding.GetNode(randomx, randomy).SetIsWalkable(!pathfinding.GetNode(randomx, randomy).isWalkable);
                                    credits -= cost;
                                }
                            }
                            if (random == 1)
                            {
                                cost = 2;
                                if (credits - cost >= 0)
                                {
                                    Object1 = Instantiate(ifthInstance, pathfinding.GetGrid().GetWorldPosition(randomx, randomy) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                      pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                    ifthscript = Object1.GetComponent<InfantryHeavy>();
                                    ifthscript.UpdatePosition(randomx, randomy);
                                    IFTHUnits.Add(Object1);
                                    pathfinding.GetGrid().SetGridObject(pathfinding.GetGrid().GetWorldPosition(randomx, randomy),
                                                                        new PathNode(pathfinding.GetGrid(), randomx, randomy, -1));
                                    pathfinding.GetNode(randomx, randomy).SetIsWalkable(!pathfinding.GetNode(randomx, randomy).isWalkable);
                                    credits -= cost;
                                }
                            }
                            if (random == 2)
                            {
                                cost = 3;
                                if (credits - cost >= 0)
                                {
                                    Object1 = Instantiate(iftkInstance, pathfinding.GetGrid().GetWorldPosition(randomx, randomy) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                      pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                    iftkscript = Object1.GetComponent<InfantryKiller>();
                                    iftkscript.UpdatePosition(randomx, randomy);
                                    IFTKUnits.Add(Object1);
                                    pathfinding.GetGrid().SetGridObject(pathfinding.GetGrid().GetWorldPosition(randomx, randomy),
                                                                        new PathNode(pathfinding.GetGrid(), randomx, randomy, -1));
                                    pathfinding.GetNode(randomx, randomy).SetIsWalkable(!pathfinding.GetNode(randomx, randomy).isWalkable);
                                    credits -= cost;
                                }
                            }
                        }
                        tcredits.text = "Credits: " + credits;
                        cost = 1;
                    }
                    #endregion
                    GetPositions();
                    starting = 1;
                }
                GameManager.Instance.UpdateGameState(GameManager.GameState.play);
                #region PlayerPrepare
                tcredits.text = "Credits: " + credits;
                if (Input.GetMouseButtonDown(0) && SelectionActive == true)
                {
                    if (pathfinding.GetGrid().GetGridObject(GetMouseWorldPosition()).GetValue() == 1)
                    {
                        switch (type)
                        {
                            case (0):
                                cost = 1;
                                if (credits >= cost)
                                {
                                    pathfinding.GetGrid().GetXY(GetMouseWorldPosition(), out x, out y);
                                    pathfinding.GetGrid().SetGridObject(GetMouseWorldPosition(), new PathNode(pathfinding.GetGrid(), x, y, -1));
                                    Object1 = Instantiate(iftsInstance, pathfinding.GetGrid().GetWorldPosition(x, y) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                          pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                    iftsscript = Object1.GetComponent<InfantrySmall>();
                                    iftsscript.UpdatePosition(x, y);
                                    IFTSUnits.Add(Object1);
                                    pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
                                    credits -= cost;
                                }
                                break;
                            case (1):
                                cost = 2;
                                if (credits >= cost)
                                {
                                    pathfinding.GetGrid().GetXY(GetMouseWorldPosition(), out x, out y);
                                    pathfinding.GetGrid().SetGridObject(GetMouseWorldPosition(), new PathNode(pathfinding.GetGrid(), x, y, -1));
                                    Object1 = Instantiate(ifthInstance, pathfinding.GetGrid().GetWorldPosition(x, y) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                          pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                    ifthscript = Object1.GetComponent<InfantryHeavy>();
                                    ifthscript.UpdatePosition(x, y); ;
                                    IFTHUnits.Add(Object1);
                                    pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
                                    credits -= cost;
                                }
                                break;
                            case (2):
                                cost = 3;
                                if (credits >= cost)
                                {
                                    pathfinding.GetGrid().GetXY(GetMouseWorldPosition(), out x, out y);
                                    pathfinding.GetGrid().SetGridObject(GetMouseWorldPosition(), new PathNode(pathfinding.GetGrid(), x, y, -1));
                                    Object1 = Instantiate(iftkInstance, pathfinding.GetGrid().GetWorldPosition(x, y) + new Vector3(pathfinding.GetGrid().GetCellSize(),
                                                          pathfinding.GetGrid().GetCellSize()) * .5f, Quaternion.identity);
                                    iftkscript = Object1.GetComponent<InfantryKiller>();
                                    iftkscript.UpdatePosition(x, y);
                                    IFTKUnits.Add(Object1);
                                    pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
                                    credits -= cost;
                                }
                                break;
                        }
                    }
                    SelectionState(false);
                }
                #endregion
                break;
            #endregion
            #region Play
            case GameManager.GameState.play:
                #region InfantrySmall
                foreach (GameObject currentUnit in IFTSUnits)
                {
                    minpathCost = 999999999;
                    foreach (GameObject enemyUnit in TWSUnits)
                    {
                        if(enemyUnit != null)
                        {
                            List<PathNode> path = pathfinding.FindPath(currentUnit.GetComponent<InfantrySmall>().GetX(),
                                                                   currentUnit.GetComponent<InfantrySmall>().GetY(),
                                                                   enemyUnit.GetComponent<TowerSmall>().GetX(),
                                                                   enemyUnit.GetComponent<TowerSmall>().GetY());
                            if (path != null)
                            {
                                if (pathfinding.GetPathCost() < minpathCost)
                                {
                                    minpathCost = pathfinding.GetPathCost();
                                    minpath = path;
                                }
                            }
                        }
                    }
                    foreach (GameObject enemyUnit in TWHUnits)
                    {
                        if(enemyUnit != null)
                        {
                            List<PathNode> path = pathfinding.FindPath(currentUnit.GetComponent<InfantrySmall>().GetX(),
                                                                   currentUnit.GetComponent<InfantrySmall>().GetY(),
                                                                   enemyUnit.GetComponent<TowerHeavy>().GetX(),
                                                                   enemyUnit.GetComponent<TowerHeavy>().GetY());
                            if (path != null)
                            {
                                if (pathfinding.GetPathCost() < minpathCost)
                                {
                                    minpathCost = pathfinding.GetPathCost();
                                    minpath = path;
                                }
                            }
                        }
                    }
                    foreach (GameObject enemyUnit in PWUnits)
                    {
                        if(enemyUnit != null)
                        {
                            List<PathNode> path = pathfinding.FindPath(currentUnit.GetComponent<InfantrySmall>().GetX(),
                                                                   currentUnit.GetComponent<InfantrySmall>().GetY(),
                                                                   enemyUnit.GetComponent<PowerCore>().GetX(),
                                                                   enemyUnit.GetComponent<PowerCore>().GetY());
                            if (path != null)
                            {
                                if (pathfinding.GetPathCost() < minpathCost)
                                {
                                    minpathCost = pathfinding.GetPathCost();
                                    minpath = path;
                                }
                            }
                        }
                    }
                    if (pathfinding.GetGrid()._Debug)
                    {
                        for (int i = 0; i < minpath.Count - 1; i++)
                        {
                            Debug.DrawLine(new Vector3(minpath[i].GetX(), minpath[i].GetY()) * 10f + Vector3.one * 5f, new Vector3(minpath[i + 1].GetX(),
                                           minpath[i + 1].GetY()) * 10f + Vector3.one * 5f, Color.yellow, 5f);
                        }
                    }
                    currentUnit.GetComponent<InfantrySmall>().SetTargetPosition(new Vector3(minpath[minpath.Count - 1].GetX(), minpath[minpath.Count - 1].GetY()) * 10f + Vector3.one * 5f);
                }
                #endregion
                #region InfantryHeavy
                foreach (GameObject currentUnit in IFTHUnits)
                {
                    minpathCost = 999999999;
                    foreach (GameObject enemyUnit in TWSUnits)
                    {
                        if(enemyUnit != null)
                        {
                            List<PathNode> path = pathfinding.FindPath(currentUnit.GetComponent<InfantryHeavy>().GetX(),
                                                                   currentUnit.GetComponent<InfantryHeavy>().GetY(),
                                                                   enemyUnit.GetComponent<TowerSmall>().GetX(),
                                                                   enemyUnit.GetComponent<TowerSmall>().GetY());
                            if (path != null)
                            {
                                if (pathfinding.GetPathCost() < minpathCost)
                                {
                                    minpathCost = pathfinding.GetPathCost();
                                    minpath = path;
                                }
                            }
                        }
                    }
                    foreach (GameObject enemyUnit in TWHUnits)
                    {
                        if(enemyUnit != null)
                        {
                            List<PathNode> path = pathfinding.FindPath(currentUnit.GetComponent<InfantryHeavy>().GetX(),
                                                                   currentUnit.GetComponent<InfantryHeavy>().GetY(),
                                                                   enemyUnit.GetComponent<TowerHeavy>().GetX(),
                                                                   enemyUnit.GetComponent<TowerHeavy>().GetY());
                            if (path != null)
                            {
                                if (pathfinding.GetPathCost() < minpathCost)
                                {
                                    minpathCost = pathfinding.GetPathCost();
                                    minpath = path;
                                }
                            }
                        }
                    }
                    foreach (GameObject enemyUnit in PWUnits)
                    {
                        if(enemyUnit != null)
                        {
                            List<PathNode> path = pathfinding.FindPath(currentUnit.GetComponent<InfantryHeavy>().GetX(),
                                                                   currentUnit.GetComponent<InfantryHeavy>().GetY(),
                                                                   enemyUnit.GetComponent<PowerCore>().GetX(),
                                                                   enemyUnit.GetComponent<PowerCore>().GetY());
                            if (path != null)
                            {
                                if (pathfinding.GetPathCost() < minpathCost)
                                {
                                    minpathCost = pathfinding.GetPathCost();
                                    minpath = path;
                                }
                            }
                        }
                    }
                    if (pathfinding.GetGrid()._Debug)
                    {
                        for (int i = 0; i < minpath.Count - 1; i++)
                        {
                            Debug.DrawLine(new Vector3(minpath[i].GetX(), minpath[i].GetY()) * 10f + Vector3.one * 5f, new Vector3(minpath[i + 1].GetX(),
                                           minpath[i + 1].GetY()) * 10f + Vector3.one * 5f, Color.blue, 5f);
                        }
                    }
                    currentUnit.GetComponent<InfantryHeavy>().SetTargetPosition(new Vector3(minpath[minpath.Count - 1].GetX(), minpath[minpath.Count - 1].GetY()) * 10f + Vector3.one * 5f);
                }
                #endregion
                #region InfantryKiller
                foreach (GameObject currentUnit in IFTKUnits)
                {
                    minpathKillerCost = 999999999;
                    foreach (GameObject enemyUnit in PWUnits)
                    {
                        if (enemyUnit != null)
                        {
                            List<PathNode> path = pathfinding.FindPath(currentUnit.GetComponent<InfantryKiller>().GetX(),
                                                                   currentUnit.GetComponent<InfantryKiller>().GetY(),
                                                                   enemyUnit.GetComponent<PowerCore>().GetX(),
                                                                   enemyUnit.GetComponent<PowerCore>().GetY());
                            if (path != null)
                            {
                                if (pathfinding.GetPathCost() < minpathKillerCost)
                                {
                                    minpathKillerCost = pathfinding.GetPathCost();
                                    minpathkiller = path;
                                }
                            }
                        }
                    }
                    #region NoPathToCore
                    /*
                    if (minpath == null)
                    {
                        minpathCost = 999999999;
                        foreach (GameObject enemyUnit in TWSUnits)
                        {
                            if (enemyUnit != null)
                            {
                                List<PathNode> path = pathfinding.FindPath(currentUnit.GetComponent<InfantryKiller>().GetX(),
                                                                       currentUnit.GetComponent<InfantryKiller>().GetY(),
                                                                       enemyUnit.GetComponent<TowerSmall>().GetX(),
                                                                       enemyUnit.GetComponent<TowerSmall>().GetY());
                                if (path != null)
                                {
                                    if (pathfinding.GetPathCost() < minpathCost)
                                    {
                                        minpathCost = pathfinding.GetPathCost();
                                        minpath = path;
                                    }
                                }
                            }
                        }
                        foreach (GameObject enemyUnit in TWHUnits)
                        {
                            if (enemyUnit != null)
                            {
                                List<PathNode> path = pathfinding.FindPath(currentUnit.GetComponent<InfantryKiller>().GetX(),
                                                                       currentUnit.GetComponent<InfantryKiller>().GetY(),
                                                                       enemyUnit.GetComponent<TowerHeavy>().GetX(),
                                                                       enemyUnit.GetComponent<TowerHeavy>().GetY());
                                if (path != null)
                                {
                                    if (pathfinding.GetPathCost() < minpathCost)
                                    {
                                        minpathCost = pathfinding.GetPathCost();
                                        minpath = path;
                                    }
                                }
                            }
                        }
                    }*/
                    #endregion
                    if(minpathkiller != null)
                    {
                        if (pathfinding.GetGrid()._Debug)
                        {
                            for (int i = 0; i < minpathkiller.Count - 1; i++)
                            {
                                Debug.DrawLine(new Vector3(minpathkiller[i].GetX(), minpathkiller[i].GetY()) * 10f + Vector3.one * 5f, new Vector3(minpathkiller[i + 1].GetX(),
                                               minpathkiller[i + 1].GetY()) * 10f + Vector3.one * 5f, Color.black, 5f);
                            }
                        }
                        currentUnit.GetComponent<InfantryKiller>().SetTargetPosition(new Vector3(minpathkiller[minpathkiller.Count - 1].GetX(), minpathkiller[minpathkiller.Count - 1].GetY()) * 10f + Vector3.one * 5f);
                    }
                }
                #endregion
                #region ClickDelete
                if (Input.GetMouseButtonDown(0))
                {
                    pathfinding.GetGrid().GetXY(GetMouseWorldPosition(), out x, out y);
                    foreach (GameObject unit in TWSUnits)
                    {
                        if(unit.GetComponent<TowerSmall>().GetX() == x && unit.GetComponent<TowerSmall>().GetY() == y)
                        {
                            TWSUnits.Remove(unit);
                            unit.GetComponent<TowerSmall>().Delete(pathfinding.GetGrid());
                        }
                    }
                    foreach (GameObject unit in TWHUnits)
                    {
                        if (unit.GetComponent<TowerHeavy>().GetX() == x && unit.GetComponent<TowerHeavy>().GetY() == y)
                        {
                            TWHUnits.Remove(unit);
                            unit.GetComponent<TowerHeavy>().Delete(pathfinding.GetGrid());
                        }
                    }
                }
                if(PWUnits.Count < 1)
                {
                    GameManager.Instance.UpdateGameState(GameManager.GameState.end);
                }
                #endregion
                if (PWUnits.Count == 0 || (IFTSUnits.Count == 0 && IFTHUnits.Count == 0 && IFTKUnits.Count == 0) || (IFTSUnits.Count == 0 && IFTHUnits.Count == 0 && CheckKillers()))
                {
                    GameManager.Instance.UpdateGameState(GameManager.GameState.end);
                }
                break;
            #endregion
            #region End
            case GameManager.GameState.end:
                if (PWUnits.Count == 0)
                {
                    resultados += "Win";
                }
                else
                {
                    resultados += "Lose";
                }
                Debug.Log(resultados);
                GameManager.Instance.AddTextToFile(resultados);
                ReStartGame();
                break;
            #endregion
        }
    }

    #region Utils
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public void SetType0()
    {
        type = 0;
        SelectionState(true);
    }

    public void SetType1()
    {
        type = 1;
        SelectionState(true);
    }

    public void SetType2()
    {
        type = 2;
        SelectionState(true);
    }

    public void SelectionState(bool State)
    {
        SelectionActive = State;
    }

    public void StartGame()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.play);
    }

    public void ReStartGame()
    {
        Debug.Log("Restarting");
        #region DESTRUCTION
        foreach (GameObject gObject in PWUnits)
        {
            gObject.GetComponent<PowerCore>().Erase(pathfinding.GetGrid());
        }
        PWUnits.Clear();
        foreach (GameObject gObject in IFTSUnits)
        {
            gObject.GetComponent<InfantrySmall>().Erase(pathfinding.GetGrid());
        }
        IFTSUnits.Clear();
        foreach (GameObject gObject in IFTHUnits)
        {
            gObject.GetComponent<InfantryHeavy>().Erase(pathfinding.GetGrid());
        }
        IFTHUnits.Clear();
        foreach (GameObject gObject in IFTKUnits)
        {
            gObject.GetComponent<InfantryKiller>().Erase(pathfinding.GetGrid());
        }
        IFTKUnits.Clear();
        foreach (GameObject gObject in TWSUnits)
        {
            gObject.GetComponent<TowerSmall>().Erase(pathfinding.GetGrid());
        }
        TWSUnits.Clear();
        foreach (GameObject gObject in TWHUnits)
        {
            gObject.GetComponent<TowerHeavy>().Erase(pathfinding.GetGrid());
        }
        TWHUnits.Clear();
        GameObject[] bullets;

        bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        GameObject[] ebullets;

        ebullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

        foreach (GameObject ebullet in ebullets)
        {
            Destroy(ebullet);
        }
        GameObject[] walls;

        walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }
        #endregion
        starting = 0;
        GameManager.Instance.UpdateGameState(GameManager.GameState.prepare);
    }

    public IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private bool CheckKillers()
    {
        if (IFTKUnits.Count > 0)
        {
            foreach (GameObject node in IFTKUnits)
            {
                if (node.GetComponent<InfantryKiller>().GetStartX() == node.GetComponent<InfantryKiller>().GetX() || node.GetComponent<InfantryKiller>().GetStartY() == node.GetComponent<InfantryKiller>().GetY())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    public void GetPositions()
    {
        //PLAYER X
        int contador = 0;
        foreach (GameObject unit in IFTSUnits)
        {
            resultados += unit.GetComponent<InfantrySmall>().GetX() + ",";
            contador += 1;
        }
        foreach (GameObject unit in IFTHUnits)
        {
            resultados += unit.GetComponent<InfantryHeavy>().GetX() + ",";
            contador += 1;
        }
        foreach (GameObject unit in IFTKUnits)
        {
            resultados += unit.GetComponent<InfantryKiller>().GetX() + ",";
            contador += 1;
        }
        while (contador < 15)
        {
            resultados += "NULL,";
            contador += 1;
        }
        //PLAYER Y
        contador = 0;
        foreach (GameObject unit in IFTSUnits)
        {
            resultados += unit.GetComponent<InfantrySmall>().GetY() + ",";
            contador += 1;
        }
        foreach (GameObject unit in IFTHUnits)
        {
            resultados += unit.GetComponent<InfantryHeavy>().GetY() + ",";
            contador += 1;
        }
        foreach (GameObject unit in IFTKUnits)
        {
            resultados += unit.GetComponent<InfantryKiller>().GetY() + ",";
            contador += 1;
        }
        while (contador < 15)
        {
            resultados += "NULL,";
            contador += 1;
        }
        //PLAYER TYPE
        contador = 0;
        foreach (GameObject unit in IFTSUnits)
        {
            resultados += unit.GetComponent<InfantrySmall>().GetUnitType() + ",";
            contador += 1;
        }
        foreach (GameObject unit in IFTHUnits)
        {
            resultados += unit.GetComponent<InfantryHeavy>().GetUnitType() + ",";
            contador += 1;
        }
        foreach (GameObject unit in IFTKUnits)
        {
            resultados += unit.GetComponent<InfantryKiller>().GetUnitType() + ",";
            contador += 1;
        }
        while (contador < 15)
        {
            resultados += "NULL,";
            contador += 1;
        }
        //ENEMY X
        contador = 0;
        foreach (GameObject unit in TWSUnits)
        {
            resultados += unit.GetComponent<TowerSmall>().GetX() + ",";
            contador += 1;
        }
        foreach (GameObject unit in TWHUnits)
        {
            resultados += unit.GetComponent<TowerHeavy>().GetX() + ",";
            contador += 1;
        }
        while (contador < 7)
        {
            resultados += "NULL,";
            contador += 1;
        }
        //ENEMY Y
        contador = 0;
        foreach (GameObject unit in TWSUnits)
        {
            resultados += unit.GetComponent<TowerSmall>().GetY() + ",";
            contador += 1;
        }
        foreach (GameObject unit in TWHUnits)
        {
            resultados += unit.GetComponent<TowerHeavy>().GetY() + ",";
            contador += 1;
        }
        while (contador < 7)
        {
            resultados += "NULL,";
            contador += 1;
        }
        //ENEMY TYPE
        contador = 0;
        foreach (GameObject unit in TWSUnits)
        {
            resultados += unit.GetComponent<TowerSmall>().GetUnitType() + ",";
            contador += 1;
        }
        foreach (GameObject unit in TWHUnits)
        {
            resultados += unit.GetComponent<TowerHeavy>().GetUnitType() + ",";
            contador += 1;
        }
        while (contador < 7)
        {
            resultados += "NULL,";
            contador += 1;
        }
    }
    #endregion
}
