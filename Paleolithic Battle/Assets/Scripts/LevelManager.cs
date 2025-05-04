using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // Reference to the main camera
    [SerializeField] private float cameraSpeed = 5f; // Speed of the camera movement
    [SerializeField] private float cameraZoomSpeed = 5f; // Speed of the camera zoom
    [SerializeField] private float minCameraSize = 5f; // Minimum camera size
    [SerializeField] private float maxCameraSize = 20f; // Maximum camera size

    public GameObject menu;

    public GameObject buttonPrefab; // Prefab for the button

    public Map map; // Reference to the map data
    public float cellSize = 1.0f; // Size of each cell in the grid
    private Cell[][] cells;
    public GameObject CellEmpytyPrefab;
    public GameObject CellMountainPrefab;
    public GameObject CellWaterPrefab;
    public GameObject CellForestPrefab;
    public GameObject CellCavePrefab;
    public GameObject CellCampPrefab;
    public GameObject CellBasePrefab;

    public GameObject UnitBasePrefab; // Prefab for the unit
    public GameObject UnitHeavyPrefab; // Prefab for the unit
    public GameObject UnitRangePrefab; // Prefab for the unit


    [HideInInspector] public ILevelState currentState;
    [HideInInspector] public PlayerTurnState playerTurnState;
    [HideInInspector] public EnemyTurnState enemyTurnState;
    [HideInInspector] public PreviewMoveState previewMoveState;
    [HideInInspector] public MenuState menuState;

    [HideInInspector] public TrainState trainState;

    [HideInInspector] public AttackState attackState;


    public float moneyPlayer = 1000;
    public float moneyEnemy = 1000;

    public UnitType[] unitTypes;

    private int currentTurn = 0; // Current turn number

    void Start()
    {
        playerTurnState = new PlayerTurnState(this);
        enemyTurnState = new EnemyTurnState(this);
        currentState = playerTurnState; // Start with player turn state
        InitializeMap();
        mainCamera.transform.position = new Vector3(map.width / 2f, map.height / 2f, -10); // Center the camera on the map
    }

    private void InitializeMap()
    {
        cells = new Cell[map.height][];
        for (int i = 0; i < map.height; i++)
        {
            cells[i] = new Cell[map.width];
            for (int j = 0; j < map.width; j++)
            {
                GameObject cellPrefab = GetCellPrefab(map.GetCellType(j, i));
                GameObject cellObject = Instantiate(cellPrefab, new Vector3(j * cellSize, i * cellSize, 0), Quaternion.identity);
                cellObject.GetComponent<SpriteRenderer>().sortingOrder = -i; // Set sorting order based on y position
                Cell cell = cellObject.GetComponent<Cell>();
                cell.x = j;
                cell.y = i;
                if(cell.capturable){
                    cell.player = map.GetPlayer(j, i) == 0; // Set player flag based on map data
                    cell.enemy = map.GetPlayer(j, i) == 1; // Set enemy flag based on map data
                    if(cell.enemy){
                        cellObject.GetComponent<SpriteRenderer>().color = Color.red; // Set enemy color to red
                    }
                }
                GameObject unitPrefab = GetUnitPrefab(map.GetUnitType(j, i));
                if (unitPrefab != null)
                {
                    GameObject unitObject = Instantiate(unitPrefab, new Vector3(j * cellSize, i * cellSize + cellSize / 2, 0), Quaternion.identity);
                    unitObject.GetComponent<SpriteRenderer>().sortingOrder = -i + 1; // Set sorting order based on y position
                    IUnit unit = unitObject.GetComponent<IUnit>();
                    unit.SetPosition(j, i);
                    unit.playerUnit = map.GetPlayer(j, i) == 0; // Set player unit flag based on map data
                    if(!unit.playerUnit){
                        unitObject.GetComponent<SpriteRenderer>().color = Color.red; // Set enemy unit color to red
                    }
                    unit.gameObject = unitObject;
                    cell.unit = unit;
                    cell.isOccupied = true;
                    unit.currentCell = cell;
                }
                cells[i][j] = cell;
            }
        }
    }

    private GameObject GetCellPrefab(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Empty:
                return CellEmpytyPrefab;
            case CellType.Mountain:
                return CellMountainPrefab;
            case CellType.Water:
                return CellWaterPrefab;
            case CellType.Forest:
                return CellForestPrefab;
            case CellType.Cave:
                return CellCavePrefab;
            case CellType.Camp:
                return CellCampPrefab;
            case CellType.Base:
                return CellBasePrefab;
            default:
                return null;
        }
    }

    private GameObject GetUnitPrefab(UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Base:
                return UnitBasePrefab;
            case UnitType.Heavy:
                return UnitHeavyPrefab;
            case UnitType.Range:
                return UnitRangePrefab;
            default:
                return null;
        }
    }

    public bool CanAffordUnit(UnitType unitType, bool canPlayerTurn)
    {
        if (canPlayerTurn)
        {
            return CanAffordUnit(unitType, moneyPlayer);
        }
        else
        {
            return CanAffordUnit(unitType, moneyEnemy);
        }
    }
    private bool CanAffordUnit(UnitType unitType, float money)
    {
        GameObject unitPrefab = GetUnitPrefab(unitType);
        if (unitPrefab != null)
        {
            IUnit unit = unitPrefab.GetComponent<IUnit>();
            return money >= unit.moneyCost; // Check if the player can afford the unit
        }
        return false; // If unit prefab is not found, return false
    }

    public int GetUnitCost(UnitType unitType)
    {
        GameObject unitPrefab = GetUnitPrefab(unitType);
        if (unitPrefab != null)
        {
            IUnit unit = unitPrefab.GetComponent<IUnit>();
            return unit.moneyCost; // Return the cost of the unit
        }
        return 0; // If unit prefab is not found, return 0
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W)) // Move camera up
        {
            mainCamera.transform.position += Vector3.up * cameraSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S)) // Move camera down
        {
            mainCamera.transform.position += Vector3.down * cameraSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A)) // Move camera left
        {
            mainCamera.transform.position += Vector3.left * cameraSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D)) // Move camera right
        {
            mainCamera.transform.position += Vector3.right * cameraSpeed * Time.deltaTime;
        }
        //Clamp camera position to stay within the map bounds
        float halfWidth = mainCamera.orthographicSize * ((float)Screen.width / Screen.height) / 2f;
        float halfHeight = mainCamera.orthographicSize / 2f;
        //mainCamera.transform.position = new Vector3(
         //   Mathf.Clamp(mainCamera.transform.position.x, halfWidth, map.width - halfWidth),
           // Mathf.Clamp(mainCamera.transform.position.y, halfHeight, map.height - halfHeight),
          //  mainCamera.transform.position.z
        //);
        if(Input.GetAxis("Mouse ScrollWheel") > 0) // Zoom in
        {
            mainCamera.orthographicSize -= cameraZoomSpeed * Time.deltaTime;
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0) // Zoom out
        {
            mainCamera.orthographicSize += cameraZoomSpeed * Time.deltaTime;
        }
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minCameraSize, maxCameraSize); // Clamp camera size

        currentState.UpdateState();
    }

    public List<Cell> GetAdjacentCells(Cell cell)
    {
        List<Cell> adjacentCells = new List<Cell>();
        int x = cell.x;
        int y = cell.y;

        // Check the four possible directions (up, down, left, right)
        if (x > 0) adjacentCells.Add(cells[y][x - 1]); // Left
        if (x < map.width - 1) adjacentCells.Add(cells[y][x + 1]); // Right
        if (y > 0) adjacentCells.Add(cells[y - 1][x]); // Down
        if (y < map.height - 1) adjacentCells.Add(cells[y + 1][x]); // Up

        return adjacentCells;
    }

    public void CreateUnit(UnitType unitType, Cell selectedCell, bool playerTurn)
    {
        GameObject unitPrefab = GetUnitPrefab(unitType);
        if (unitPrefab != null && CanAffordUnit(unitType, playerTurn)) // Check if the unit prefab is found and if the player can afford the unit
        {
            GameObject unitObject = Instantiate(unitPrefab, new Vector3(selectedCell.x * cellSize, selectedCell.y * cellSize + cellSize / 2, 0), Quaternion.identity);
            IUnit unit = unitObject.GetComponent<IUnit>();
            unit.SetPosition(selectedCell.x, selectedCell.y); // Set the unit's position
            unit.playerUnit = playerTurn; // Set the player unit flag based on the player's turn
            selectedCell.isOccupied = true; // Mark the cell as occupied
            selectedCell.unit = unit; // Assign the unit to the cell
            unit.currentCell = selectedCell; // Set the unit's current cell reference
            unit.gameObject = unitObject; // Set the unit's GameObject reference
            if(playerTurn)
            {
                moneyPlayer -= unit.moneyCost; // Deduct the cost from the player's money
            }
            else
            {
                moneyEnemy -= unit.moneyCost; // Deduct the cost from the enemy's money
            } 
        }
    }

    public void MoveUnit(IUnit unit, Cell targetCell)
    {
        if (unit.currentCell != null)
        {
            unit.currentCell.isOccupied = false; // Mark the current cell as unoccupied
            unit.currentCell.unit = null; // Remove the unit from the current cell
        }

        unit.SetPosition(targetCell.x, targetCell.y); // Update the unit's position
        targetCell.isOccupied = true; // Mark the target cell as occupied
        targetCell.unit = unit; // Assign the unit to the target cell
        unit.currentCell = targetCell; // Update the unit's current cell reference

        // Move the unit's GameObject to the new position
        unit.gameObject.transform.position = new Vector3(targetCell.x * cellSize, targetCell.y * cellSize + cellSize / 2, 0);
    }

    public void AttackUnit(IUnit attacker, IUnit target)
    {
        if (attacker != null && target != null) // Check if both units are valid and the target cell is occupied
        {
            attacker.Attack(target); // Apply damage to the target unit
            if (target.health <= 0) // Check if the target unit is dead
            {
                Destroy(target.gameObject); // Destroy the target unit's GameObject
                target.currentCell.isOccupied = false; // Mark the cell as unoccupied
                target.currentCell.unit = null; // Remove the unit from the cell
            }
        }
    }

    public void CaptureCell(Cell cell, bool playerTurn)
    {
        if (cell.capturable) // Check if the cell is capturable
        {
            cell.player = playerTurn; // Set the player flag based on the player's turn
            cell.enemy = !playerTurn; // Set the enemy flag based on the player's turn
        }
    }

    public void EndTurn()
    {
        currentTurn++; // Increment the turn number
    }
        
}
