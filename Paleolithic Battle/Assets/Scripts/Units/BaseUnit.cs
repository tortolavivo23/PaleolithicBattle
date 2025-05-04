using Unity.VisualScripting;
using UnityEngine;

public class BaseUnit : MonoBehaviour, IUnit
{
    public int attackPower = 10;
    public int defensePower = 5;
    [SerializeField] private int _movementRange = 3;
    public int movementRange { get => _movementRange; } // Movement range of the unit

    public UnitType unitType { get => UnitType.Base; } // Type of the unit (e.g., Warrior, Mage, etc.)
    public Cell currentCell { get; set; } // Reference to the cell the unit is currently occupying
    [SerializeField] private int _minAttackRange = 1; // Minimum attack range of the unit
    public int minAttackRange { get => _minAttackRange; } // Minimum attack range of the unit
    [SerializeField] private int _maxAttackRange = 1; // Maximum attack range of the unit
    public int maxAttackRange { get => _maxAttackRange; } // Maximum attack range of the unit

    GameObject IUnit.gameObject { get; set; } // Reference to the GameObject of the unit

    [SerializeField] private bool _playerUnit = true; // Indicates if the unit is a player unit
    public bool playerUnit { get; set; }
    [SerializeField] private float _health = 100f; // Health of the unit
    public float health { get => _health; set => _health = value; }

    [SerializeField] private int _moneyCost = 100; // Cost of the unit in money
    public int moneyCost { get => _moneyCost; set => _moneyCost = value; } // Cost of the unit in money

    // Removed the redundant gameObject property to avoid conflict with the inherited member

    int x,y;

    public void Move(int newX, int newY)
    {
        x = newX;
        y = newY;
        // Add logic to update the unit's position on the grid
    }

    public void Attack(IUnit target)
    {
        // Add logic to attack the target unit
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

    private void Start()
    {
        // Initialize unit properties if needed
    }

    private void Update()
    {
        // Update unit properties if needed
    }
}


