using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseUnit : MonoBehaviour, IUnit
{
    public HealthBar healthBar; // Reference to the health bar UI element
    public float _attackPower = 10;
    public float attackPower { get => _attackPower; set => _attackPower = value; } // Attack power of the unit
    public float _defensePower = 5;
    public float defensePower { get => _defensePower; set => _defensePower = value; } // Defense power of the unit
    [SerializeField] private int _movementRange = 3;
    public int movementRange { get => _movementRange; } // Movement range of the unit

    public UnitType _unitType = UnitType.Base; // Type of the unit

    public UnitType unitType { get => _unitType; }
    public Cell currentCell { get; set; } // Reference to the cell the unit is currently occupying
    [SerializeField] private int _minAttackRange = 1; // Minimum attack range of the unit
    public int minAttackRange { get => _minAttackRange; } // Minimum attack range of the unit
    [SerializeField] private int _maxAttackRange = 1; // Maximum attack range of the unit
    public int maxAttackRange { get => _maxAttackRange; } // Maximum attack range of the unit

    GameObject IUnit.gameObject { get; set; } // Reference to the GameObject of the unit

    [SerializeField] private bool _playerUnit = true; // Indicates if the unit is a player unit
    public bool playerUnit { get => _playerUnit; set => _playerUnit = value; } // Indicates if the unit is a player unit

    public float _maxHealth = 100; // Maximum health of the unit
    public float maxHealth { get => _maxHealth; set => _maxHealth = value; } // Maximum health of the unit

    private float _health;
    public float health { get => _health; set => _health = value; }

    [SerializeField] private int _moneyCost = 100; // Cost of the unit in money
    public int moneyCost { get => _moneyCost; set => _moneyCost = value; } // Cost of the unit in money

    private int _lastMoveTurn = -1; // Last turn the unit moved
    public int lastMoveTurn { get => _lastMoveTurn; set => _lastMoveTurn = value; } // Last turn the unit moved
    private int _lastActionTurn = -1; // Last turn the unit performed an action
    public int lastActionTurn { get => _lastActionTurn; set => _lastActionTurn = value; } // Last turn the unit performed an action
    int x, y;

    Vector3 physicalPosition; // Physical position of the unit in the game world

    Vector2[] path; // Path the unit will followç
    int index = 0; // Current index in the path

    public float moveSpeed = 8f; // Speed at which the unit moves

    private void Start()
    {
        // Initialize unit properties
        health = maxHealth;
        healthBar.SetMaxHealth((int)maxHealth);
    }

    public void Move(int newX, int newY)
    {
        x = newX;
        y = newY;
        // Add logic to update the unit's position on the grid
    }

    public void Attack(IUnit target, float multiplier)
    {
        if (target == null || !target.IsAlive())
        {
            Debug.Log("Target is null or not alive.");
            return;
        }
        // Calculate damage based on attack and defense power
        float damage = Mathf.Max(0, attackPower * multiplier - target.defensePower);
        target.GetDamage(damage);
    }

    public void SetPosition(int newX, int newY)
    {
        x = newX;
        y = newY;
    }

    public (int, int) GetPosition()
    {
        return (x, y);
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth((int)health);
    }

    public void SetPhysicalPosition(Vector2 position)
    {
        physicalPosition = position; // Set the physical position of the unit
        path = null; // Clear the path when setting a new physical position
    }

    public void SetPath(List<Vector2> newPath)
    {
        if (newPath == null || newPath.Count == 0)
        {
            path = null;
            index = 0;
            return;
        }

        path = newPath.ToArray(); // Convert List to Array for easier indexing
        index = 1; // Reset index to start of the path
        physicalPosition = path[index]; // Set the initial physical position
    }

    void Update()
    {
        if (transform.position != physicalPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, physicalPosition, Time.deltaTime * moveSpeed);
            if (transform.position == physicalPosition && path != null && index < path.Length - 1)
            {
                Debug.Log($"Moving to next position in path: {physicalPosition}");
                index++;
                physicalPosition = path[index];
            }
        }
    }
}


