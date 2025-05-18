using Unity.VisualScripting;
using UnityEngine;

public class BaseUnit : MonoBehaviour, IUnit
{
    public float _attackPower = 10;
    public float attackPower { get => _attackPower; set => _attackPower = value; } // Attack power of the unit
    public float _defensePower = 5;
    public float defensePower { get => _defensePower; set => _defensePower = value; } // Defense power of the unit
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
    public bool playerUnit { get => _playerUnit; set => _playerUnit = value; } // Indicates if the unit is a player unit
    [SerializeField] private float _health = 100f; // Health of the unit
    public float health { get => _health; set => _health = value; }

    [SerializeField] private int _moneyCost = 100; // Cost of the unit in money
    public int moneyCost { get => _moneyCost; set => _moneyCost = value; } // Cost of the unit in money

    private int _lastMoveTurn = -1; // Last turn the unit moved
    public int lastMoveTurn { get => _lastMoveTurn; set => _lastMoveTurn = value; } // Last turn the unit moved
    private int _lastActionTurn = -1; // Last turn the unit performed an action
    public int lastActionTurn { get => _lastActionTurn; set => _lastActionTurn = value; } // Last turn the unit performed an action
    int x,y;

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


