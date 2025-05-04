using UnityEngine;

public interface IUnit
{
    public GameObject gameObject { get; set; }

    public UnitType unitType { get;}
    public Cell currentCell { get; set; }
    public int movementRange { get;}

    public int moneyCost { get; set;}
    
    public int minAttackRange { get;}
    public int maxAttackRange { get;}
    public float health { get; set; }

    public int lastMoveTurn { get; set; }
    public int lastActionTurn { get; set; }

    bool playerUnit { get; set; }
    void Move(int x, int y);
    void Attack(IUnit target);
    void SetPosition(int x, int y);
    (int x, int y) GetPosition();
    bool IsAlive();
}