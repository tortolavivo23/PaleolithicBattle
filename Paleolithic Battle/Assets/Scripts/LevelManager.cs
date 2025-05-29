using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject CellEmptyPrefab;
    public GameObject CellMountainPrefab;
    public GameObject CellWaterPrefab;
    public GameObject CellForestPrefab;
    public GameObject CellCavePrefab;
    public GameObject CellCampPrefab;
    public GameObject CellBasePrefab;


    public GameObject UnitBasePrefab; // Prefab for the unit
    public GameObject UnitEnemyBasePrefab; // Prefab for the unit
    public GameObject UnitHeavyPrefab; // Prefab for the unit
    public GameObject UnitEnemyHeavyPrefab; // Prefab for the unit
    public GameObject UnitRangePrefab; // Prefab for the unit
    public GameObject UnitEnemyRangePrefab; // Prefab for the unit


    private ILevelState currentState;
    [HideInInspector] public PlayerTurnState playerTurnState;
    [HideInInspector] public EnemyTurnState enemyTurnState;
    [HideInInspector] public PreviewMoveState previewMoveState;
    [HideInInspector] public MenuState menuState;

    [HideInInspector] public TrainState trainState;

    [HideInInspector] public PreviewAttackState previewAttackState;
    [HideInInspector] public AttackState attackState;


    public int moneyPlayer = 200;
    public int moneyEnemy = 200;

    public int moneyTurn = 50; // Money added each turn

    public int moneyPerCave = 50; // Money added for each cave owned

    public UnitType[] unitTypes;

    [HideInInspector] public int currentTurn = -1; // Current turn number

    public Button endTurnButton; // Reference to the end turn button

    [HideInInspector] public Cell selectedCell; // The cell selected by the player
    public int minigameResult = 0; // Result of the minigame (0: not played, 1: win, -1: lose)
    public bool playingMinigame = false; // Flag to indicate if a minigame is being played

    [HideInInspector] public Minigames minigames; // Reference to the minigames data

    [HideInInspector] public List<IUnit> enemyUnits = new List<IUnit>();
    [HideInInspector] public List<Cell> enemyCamps = new List<Cell>();

    [HideInInspector] public List<IUnit> playerUnits = new List<IUnit>();

    private int cavePlayer = 0;
    private int caveEnemy = 0;

    public TextMeshProUGUI moneyText; // Reference to the money text UI element

    [HideInInspector] public Cell playerBase;
    [HideInInspector] public Cell enemyBase;

    public GameObject cellParent;


    public int moneyToGetStar = 1000;
    public int turnsToGetStar = 10;

    public String levelMusic = "LevelMusic"; // Name of the level music to play

    void Awake()
    {
        currentTurn = -1;
        InitializeMap();
        mainCamera.transform.position = new Vector3(map.width / 2 * cellSize, map.height / 2 * cellSize, -10); // Center the camera on the map
        mainCamera.orthographicSize = maxCameraSize; // Set the camera size to maximum
    }
    void Start()
    {
        playerTurnState = new PlayerTurnState(this);
        enemyTurnState = new EnemyTurnState(this);
        previewMoveState = new PreviewMoveState(this);
        menuState = new MenuState(this);
        trainState = new TrainState(this);
        previewAttackState = new PreviewAttackState(this);
        attackState = new AttackState(this);
        currentState = playerTurnState; // Start with player turn state
        minigames = JsonUtility.FromJson<Minigames>(Resources.Load<TextAsset>("minigames").text); // Load minigames data from JSON file
        currentTurn++;
        moneyText.text = "Money: " + moneyPlayer; // Update the money text UI element
        AudioManager.Instance.Play(levelMusic); // Play the level music
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
                cellObject.transform.SetParent(cellParent.transform); // Set the parent of the cell object
                cellObject.GetComponent<SpriteRenderer>().sortingOrder = -i; // Set sorting order based on y position
                Cell cell = cellObject.GetComponent<Cell>();
                cell.x = j;
                cell.y = i;
                if (cell.capturable)
                {
                    cell.player = map.GetPlayer(j, i) == 0; // Set player flag based on map data
                    cell.enemy = map.GetPlayer(j, i) == 1; // Set enemy flag based on map data
                    if (cell.player)
                    {
                        cell.spriteEnemy.SetActive(false); // Hide enemy sprite
                        cell.spriteNeutral.SetActive(false); // Hide neutral sprite
                        cell.spritePlayer.SetActive(true); // Show player sprite
                        if (cell.cellType == CellType.Cave) cavePlayer++; // Increment player cave count
                        else if (cell.cellType == CellType.Base) playerBase = cell; // Set player base cell
                    }
                    else if (cell.enemy)
                    {
                        cell.spritePlayer.SetActive(false); // Hide player sprite
                        cell.spriteNeutral.SetActive(false); // Hide neutral sprite
                        cell.spriteEnemy.SetActive(true); // Show enemy sprite
                        if (cell.cellType == CellType.Camp) enemyCamps.Add(cell);
                        else if (cell.cellType == CellType.Cave) caveEnemy++; // Increment player cave count
                        else if (cell.cellType == CellType.Base) enemyBase = cell; // Set enemy base cell
                    }
                    else
                    {
                        cell.spritePlayer.SetActive(false); // Hide player sprite
                        cell.spriteEnemy.SetActive(false); // Hide enemy sprite
                        cell.spriteNeutral.SetActive(true); // Show neutral sprite
                    }
                }
                CreateUnit(map.GetUnitType(j, i), cell, map.GetPlayer(j, i) == 0, false); // Create unit based on map data
                cells[i][j] = cell;
            }
        }
    }

    private GameObject GetCellPrefab(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Empty:
                return CellEmptyPrefab;
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

    private GameObject GetUnitPrefab(UnitType unitType, bool player = true)
    {
        switch (unitType)
        {
            case UnitType.Base:
                if (player) return UnitBasePrefab;
                else return UnitEnemyBasePrefab;
            case UnitType.Heavy:
                {
                    if (player) return UnitHeavyPrefab;
                    else return UnitEnemyHeavyPrefab;
                }
            case UnitType.Range:
                if (player) return UnitRangePrefab;
                else return UnitEnemyRangePrefab;
            default:
                return null;
        }
    }

    public bool CanAffordUnit(UnitType unitType, bool playerTurn)
    {
        if (playerTurn)
        {
            return CanAffordUnit(unitType, moneyPlayer);
        }
        else
        {
            return CanAffordUnit(unitType, moneyEnemy, playerTurn);
        }
    }
    private bool CanAffordUnit(UnitType unitType, float money, bool playerTurn = true)
    {
        GameObject unitPrefab = GetUnitPrefab(unitType, playerTurn);
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
        ControlCamera(); // Control camera movement and zoom
        currentState.UpdateState();
        // If we are in debug mode, allow end game using numbers
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            AudioManager.Instance.Stop(levelMusic); // Stop the level music
            GameManager.Instance.EndGame(0); // End the game with a loss for the player
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.Instance.Stop(levelMusic); // Stop the level music
            GameManager.Instance.EndGame(1); // End the game with a win for the player with 1 star
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.Instance.Stop(levelMusic); // Stop the level music
            GameManager.Instance.EndGame(2); // End the game with a win for the player with 2 stars
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioManager.Instance.Stop(levelMusic); // Stop the level music
            GameManager.Instance.EndGame(3); // End the game with a win for the player with 3 stars
        }
#endif
    }

    void ControlCamera()
    {
        HandleMovement();
        HandleZoom();
        ClampCamera();
    }

    void HandleMovement()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) direction += Vector3.up;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.down;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

        mainCamera.transform.position += direction * cameraSpeed * Time.deltaTime;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            mainCamera.orthographicSize -= scroll * cameraZoomSpeed * Time.deltaTime;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minCameraSize, maxCameraSize);
        }
    }

    void ClampCamera()
    {
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * ((float)Screen.width / Screen.height);

        float minX = -cellSize / 2f;
        float maxX = (map.width - 0.5f) * cellSize;
        float minY = 0;
        float maxY = map.height * cellSize;

        float mapWidthWorld = maxX - minX;
        float mapHeightWorld = maxY - minY;

        Vector3 pos = mainCamera.transform.position;

        if (mapWidthWorld <= halfWidth * 2f)
        {
            pos.x = (minX + maxX) / 2f;
        }
        else
        {
            pos.x = Mathf.Clamp(pos.x, minX + halfWidth, maxX - halfWidth);
        }

        if (mapHeightWorld <= halfHeight * 2f)
        {
            pos.y = (minY + maxY) / 2f;
        }
        else
        {
            pos.y = Mathf.Clamp(pos.y, minY + halfHeight, maxY - halfHeight);
        }

        mainCamera.transform.position = new Vector3(pos.x, pos.y, mainCamera.transform.position.z);
    }

    void OnDrawGizmos()
    {
        float minX = -cellSize / 2f;
        float maxX = (map.width - 0.5f) * cellSize;
        float minY = 0;
        float maxY = map.height * cellSize;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minX, minY), new Vector3(maxX, minY));
        Gizmos.DrawLine(new Vector3(maxX, minY), new Vector3(maxX, maxY));
        Gizmos.DrawLine(new Vector3(maxX, maxY), new Vector3(minX, maxY));
        Gizmos.DrawLine(new Vector3(minX, maxY), new Vector3(minX, minY));
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

    public void CreateUnit(UnitType unitType, Cell selectedCell, bool playerTurn, bool moneyCost = true)
    {
        if (!(!moneyCost || CanAffordUnit(unitType, playerTurn))) // Check if the player can afford the unit
        {
            Debug.Log("Not enough money to create unit!"); // Log a message if the player cannot afford the unit
            return; // Exit the method if the player cannot afford the unit
        }

        if (selectedCell.isOccupied) // Check if the cell is already occupied
        {
            Debug.Log("Cell is already occupied!"); // Log a message if the cell is occupied
            return; // Exit the method if the cell is occupied
        }
        GameObject unitPrefab = GetUnitPrefab(unitType, playerTurn); // Get the unit prefab based on the unit type and player turn
        if (unitPrefab != null) // Check if the unit prefab is found and if the player can afford the unit
        {
            GameObject unitObject = Instantiate(unitPrefab, new Vector3(selectedCell.x * cellSize, selectedCell.y * cellSize + cellSize / 2, 0), Quaternion.identity);
            IUnit unit = unitObject.GetComponent<IUnit>();
            unit.SetPosition(selectedCell.x, selectedCell.y); // Set the unit's position
            unit.SetPhysicalPosition(new Vector2(selectedCell.x * cellSize, selectedCell.y * cellSize + cellSize / 2)); // Set the unit's physical position
            unit.playerUnit = playerTurn; // Set the player unit flag based on the player's turn
            selectedCell.isOccupied = true; // Mark the cell as occupied
            selectedCell.unit = unit; // Assign the unit to the cell
            unit.currentCell = selectedCell; // Set the unit's current cell reference
            unit.gameObject = unitObject; // Set the unit's GameObject reference
            unit.lastMoveTurn = currentTurn; // Set the last move turn for the unit
            unit.lastActionTurn = currentTurn; // Set the last action turn for the unit
            if (playerTurn)
            {
                moneyPlayer -= moneyCost ? unit.moneyCost : 0; // Deduct the cost from the player's money
                moneyText.text = "Money: " + moneyPlayer; // Update the money text UI element
                playerUnits.Add(unit); // Add the unit to the player's units list
            }
            else
            {
                moneyEnemy -= moneyCost ? unit.moneyCost : 0; // Deduct the cost from the enemy's money
                enemyUnits.Add(unit); // Add the unit to the enemy units list
            }
        }
    }

    public void MoveUnit(IUnit unit, Cell targetCell, Dictionary<Cell, Cell> availableCells = null)
    {
        if (targetCell == null || targetCell.isOccupied || targetCell.unit != null) // Check if the target cell is valid and not occupied by another unit
        {
            Debug.Log("Invalid target cell for movement!"); // Log a message if the target cell is invalid
            return; // Exit the method if the target cell is invalid
        }
        if (unit.currentCell != null)
        {
            unit.currentCell.isOccupied = false; // Mark the current cell as unoccupied
            unit.currentCell.unit = null; // Remove the unit from the current cell
        }

        unit.SetPosition(targetCell.x, targetCell.y); // Update the unit's position
        targetCell.isOccupied = true; // Mark the target cell as occupied
        targetCell.unit = unit; // Assign the unit to the target cell
        unit.currentCell = targetCell; // Update the unit's current cell reference

        if (availableCells != null) // If available cells are provided, update the path
        {
            if (availableCells.TryGetValue(targetCell, out Cell previousCell))
            {
                LinkedList<Vector2> path = new LinkedList<Vector2>();
                Cell currentCell = targetCell;
                while (currentCell != null)
                {
                    path.AddFirst(new Vector2(currentCell.x * cellSize, currentCell.y * cellSize + cellSize / 2));
                    if (availableCells.TryGetValue(currentCell, out Cell prevCell))
                    {
                        currentCell = prevCell; // Move to the previous cell in the path
                    }
                    else
                    {
                        currentCell = null; // No previous cell found, end the path
                    }
                }
                unit.SetPath(new List<Vector2>(path)); // Set the unit's path
            }
        }
        else
        {
            // Move the unit's GameObject to the new position
            unit.SetPhysicalPosition(new Vector2(targetCell.x * cellSize, targetCell.y * cellSize + cellSize / 2));
        }
    }

    public void AttackUnit(IUnit attacker, IUnit target, float multiplier = 1f)
    {
        if (attacker != null && target != null) // Check if both units are valid and the target cell is occupied
        {
            if (target.playerUnit == attacker.playerUnit) // Check if the target unit is an enemy
            {
                Debug.Log("Cannot attack your own unit!"); // Log a message if the target is an ally
                return; // Exit the method if the target is an ally
            }
            attacker.lastActionTurn = currentTurn; // Update the attacker's last action turn
            attacker.Attack(target, multiplier); // Apply damage to the target unit
            if (target.health <= 0) // Check if the target unit is dead
            {
                if (target.playerUnit)
                {
                    playerUnits.Remove(target); // Remove the target unit from the player's units list
                    if (playerUnits.Count == 0) // Check if the player has no units left
                    {
                        Debug.Log("Player has no units left!"); // Log a message if the player has no units left
                        EndGame(false); // End the game with a loss for the player
                    }
                }
                else
                {
                    enemyUnits.Remove(target); // Remove the target unit from the enemy units list
                    if (enemyUnits.Count == 0) // Check if the enemy has no units left
                    {
                        Debug.Log("Enemy has no units left!"); // Log a message if the enemy has no units left
                        EndGame(true); // End the game with a win for the player
                    }
                }
                Destroy(target.gameObject); // Destroy the target unit's GameObject
                target.currentCell.isOccupied = false; // Mark the cell as unoccupied
                target.currentCell.unit = null; // Remove the unit from the cell
            }
        }
    }

    public void CaptureCell(Cell cell, bool playerTurn)
    {
        if (!cell.capturable) return; // Exit if the cell is not capturable
        AudioManager.Instance.Play("CaptureCell"); // Play the capture sound effect
        switch (cell.cellType)
        {
            case CellType.Cave:
                if (playerTurn)
                {
                    cavePlayer++; // Increment player cave count
                    if (cell.enemy) caveEnemy--; // Decrement enemy cave count if the cell was owned by the enemy
                }
                else
                {
                    caveEnemy++; // Increment enemy cave count
                    if (cell.player) cavePlayer--; // Decrement player cave count if the cell was owned by the player
                }
                break;
            case CellType.Camp:
                if (playerTurn) enemyCamps.Remove(cell); // Remove the camp from the enemy camps list
                else enemyCamps.Add(cell); // Add the camp to the enemy camps list
                break;
            case CellType.Base:
                if (playerTurn) // Check if the player is capturing the base
                {
                    Debug.Log("Player captured the enemy base!"); // Log a message if the player captures the enemy base
                    EndGame(true); // End the game with a win for the player
                }
                else
                {
                    Debug.Log("Enemy captured the player base!"); // Log a message if the enemy captures the player's base
                    EndGame(false); // End the game with a loss for the player
                }
                break;

        }
        cell.ChangePlayer(playerTurn); // Change the cell's ownership
    }

    public void EndTurn()
    {
        currentTurn++; // Increment the turn number
    }

    public void AddMoney(bool playerTurn)
    {
        if (playerTurn)
        {
            moneyPlayer += moneyTurn + (cavePlayer * moneyPerCave); // Add money to the player's account
            moneyText.text = "Money: " + moneyPlayer; // Update the money text UI element
        }
        else moneyEnemy += moneyTurn + (caveEnemy * moneyPerCave); // Add money to the enemy's account
    }

    public void ChangeState(ILevelState newState)
    {
        currentState.ExitState(); // Call the exit method of the current state
        currentState = newState; // Change the current state to the new state
        currentState.EnterState(); // Call the enter method of the new state
    }

    public Dictionary<Cell, Cell> GetAvailableMoveCells(Cell cell)
    {
        if (!cell.isOccupied)
            return new Dictionary<Cell, Cell>();

        Dictionary<Cell, Cell> availableCells = new Dictionary<Cell, Cell>();
        bool[,] visited = new bool[map.height, map.width]; // Create a 2D array to track visited cells
        IUnit unit = cell.unit; // Get the unit in the cell
        Queue<(Cell, Cell)> queue = new Queue<(Cell, Cell)>(); // Create a queue for BFS
        queue.Enqueue((cell, cell)); // Enqueue the starting cell
        queue.Enqueue((null, null)); // Enqueue a null marker for the next level
        int range = unit.movementRange; // Get the movement range of the unit
        int steps = 0; // Initialize the step counter
        while (queue.Count > 0)
        {
            (Cell, Cell) cellTuplex = queue.Dequeue();
            Cell currentCell = cellTuplex.Item1; // Get the current cell from the tuple
            if (currentCell == null)
            {
                steps++;
                if (steps > range) break; // Si hemos alcanzado el rango de movimiento, salimos del bucle
                queue.Enqueue((null, null)); // Añadimos un marcador para el siguiente nivel
                continue;
            }

            if (visited[currentCell.y, currentCell.x]) continue; // Si ya hemos visitado esta celda, la ignoramos
            visited[currentCell.y, currentCell.x] = true; // Marcamos la celda como visitada

            if (unit.unitType == UnitType.Heavy && currentCell.cellType == CellType.Mountain) continue; // Si la unidad es pesada y la celda es montaña, la ignoramos
            if (steps != 0) availableCells.Add(currentCell, cellTuplex.Item2); // Añadimos la celda a las celdas disponibles
            if (currentCell.cellType == CellType.Mountain && steps != 0) continue; // Si la celda es montaña, no añadimos las celdas adyacentes

            // Añadimos las celdas adyacentes a la cola
            foreach (var adjacentCell in GetAdjacentCells(currentCell))
            {
                if (!visited[adjacentCell.y, adjacentCell.x] && !(adjacentCell.isOccupied && adjacentCell.unit.playerUnit != unit.playerUnit) &&
                 (unit.unitType == UnitType.Water && adjacentCell.cellType == CellType.Water ||
                 (unit.unitType != UnitType.Water && adjacentCell.cellType != CellType.Water))) // Solo añadimos celdas no ocupadas y que no sean agua si la unidad no es de tipo acuática
                {
                    queue.Enqueue((adjacentCell, currentCell));
                }
            }
        }
        return availableCells;
    }

    public List<Cell> GetAvailableAttackCells(Cell cell)
    {
        if (!cell.isOccupied)
            return new List<Cell>(); // Return an empty list if the cell is empty

        List<Cell> availableCells = new List<Cell>();
        IUnit unit = cell.unit; // Get the unit in the cell
        bool[,] visited = new bool[map.height, map.width]; // Create a 2D array to track visited cells
        Queue<Cell> queue = new Queue<Cell>(); // Create a queue for BFS
        queue.Enqueue(cell); // Enqueue the starting cell
        queue.Enqueue(null); // Enqueue a null marker for the next level
        int rangeMin = unit.minAttackRange; // Get the minimum attack range of the unit
        int rangeMax = unit.maxAttackRange; // Get the maximum attack range of the unit
        int steps = 0; // Initialize the step counter
        while (queue.Count > 0)
        {
            Cell currentCell = queue.Dequeue();
            if (currentCell == null)
            {
                steps++;
                if (steps > rangeMax) break; // Si hemos alcanzado el rango de ataque, salimos del bucle
                queue.Enqueue(null); // Añadimos un marcador para el siguiente nivel
                continue;
            }

            if (visited[currentCell.y, currentCell.x]) continue; // Si ya hemos visitado esta celda, la ignoramos
            visited[currentCell.y, currentCell.x] = true; // Marcamos la celda como visitada

            if (steps >= rangeMin && currentCell.isOccupied && currentCell.unit.playerUnit != unit.playerUnit)
                availableCells.Add(currentCell); // Añadimos la celda a las celdas disponibles

            // Añadimos las celdas adyacentes a la cola
            foreach (var adjacentCell in GetAdjacentCells(currentCell))
            {
                if (!visited[adjacentCell.y, adjacentCell.x])
                    queue.Enqueue(adjacentCell);
            }
        }
        return availableCells;
    }

    public int GetDistance(Cell cell1, Cell cell2)
    {
        return Mathf.Abs(cell1.x - cell2.x) + Mathf.Abs(cell1.y - cell2.y); // Distancia Manhattan
    }

    public int RealDistance(Cell origin, Cell target, IUnit unit)
    {
        if (origin == null || target == null || unit.unitType == UnitType.Heavy && target.cellType == CellType.Mountain)
            return int.MaxValue;

        bool[,] visited = new bool[map.height, map.width]; // Create a 2D array to track visited cells
        Queue<Cell> queue = new Queue<Cell>(); // Create a queue for BFS
        queue.Enqueue(origin); // Enqueue the starting cell
        queue.Enqueue(null); // Enqueue a null marker for the next level
        int distance = 0; // Initialize the distance counter
        while (queue.Count > 0)
        {
            Cell currentCell = queue.Dequeue();
            if (currentCell == null)
            {
                distance++;
                queue.Enqueue(null); // Add a marker for the next level
                continue;
            }

            if (visited[currentCell.y, currentCell.x]) continue; // If we have already visited this cell, skip it
            visited[currentCell.y, currentCell.x] = true; // Mark the cell as visited

            if (unit.unitType == UnitType.Heavy && currentCell.cellType == CellType.Mountain) continue; // Si la unidad es pesada y la celda es montaña, la ignoramos
            if (currentCell == target) return distance; // If we reached the target cell, return the distance 
            // Add adjacent cells to the queue
            foreach (var adjacentCell in GetAdjacentCells(currentCell))
            {
                if (!visited[adjacentCell.y, adjacentCell.x] &&
                 (unit.unitType == UnitType.Water && adjacentCell.cellType == CellType.Water ||
                 (unit.unitType != UnitType.Water && adjacentCell.cellType != CellType.Water))) // Only add unoccupied cells and not water cells if the unit is not water type
                {
                    queue.Enqueue(adjacentCell);
                }
            }
        }
        return int.MaxValue; // If we exhaust the queue without finding the target, return max value
    }

    public List<IUnit> GetUnitsInRange(Cell cell, int range, bool playerTurn = false)
    {
        List<IUnit> unitsInRange = new List<IUnit>(); // Create a list to store units in range
        foreach (var unit in playerTurn ? playerUnits : enemyUnits) // Iterate through the player's or enemy's units
        {
            if (unit.currentCell != null && GetDistance(cell, unit.currentCell) <= range) // Check if the unit is within range
            {
                unitsInRange.Add(unit); // Add the unit to the list if it is within range
            }
        }
        return unitsInRange; // Return the list of units in range
    }

    public List<Cell> GetAllCells()
    {
        List<Cell> allCells = new List<Cell>();
        foreach (var row in cells)
        {
            allCells.AddRange(row); // Add all cells from each row to the list
        }
        return allCells; // Return the list of all cells
    }


    private void EndGame(bool playerWon)
    {
        AudioManager.Instance.Stop(levelMusic); // Stop the level music
        Debug.Log(playerWon ? "Player won!" : "Player lost!"); // Log the game result
        if (!playerWon)
        {
            AudioManager.Instance.Play("Lose"); // Play lose sound
            GameManager.Instance.EndGame(0);
        }
        else
        {
            int stars = 1;
            if (moneyPlayer >= moneyToGetStar) stars++;
            if (currentTurn <= turnsToGetStar) stars++;
            AudioManager.Instance.Play("Victory"); // Play win sound
            GameManager.Instance.EndGame(stars); // End the game with the specified number of stars
        }
    }
}
