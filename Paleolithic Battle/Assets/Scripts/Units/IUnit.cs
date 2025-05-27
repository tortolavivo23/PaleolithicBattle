using System;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public float attackPower { get; set; }
    public float defensePower { get; set; }
    public GameObject gameObject { get; set; }

    public UnitType unitType { get; }
    public Cell currentCell { get; set; }
    public int movementRange { get; }

    public int moneyCost { get; set; }

    public int minAttackRange { get; }
    public int maxAttackRange { get; }

    public float maxHealth { get; set; }
    public float health { get; set; }

    public int lastMoveTurn { get; set; }
    public int lastActionTurn { get; set; }

    bool playerUnit { get; set; }
    public Action OnMovementFinished { get; set; }
    void Move(int x, int y);
    void Attack(IUnit target, float multiplier);
    void GetDamage(float damage);
    void SetPosition(int x, int y);
    (int x, int y) GetPosition();

    void SetPhysicalPosition(Vector2 position);

    void SetPath(List<Vector2> path);
    bool IsAlive();
    
    public bool CanKill(IUnit target)
    {
        return target.health + target.defensePower <= attackPower;
    }
}