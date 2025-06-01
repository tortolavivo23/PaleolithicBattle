using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellType cellType;
    public int x, y; // Coordinates in the grid
    public bool isOccupied = false; // Indicates if the cell is occupied by a unit
    public IUnit unit; // Reference to the unit occupying the cell, if any

    public bool capturable = false; // Indicates if the cell can be captured
    public bool player = false;
    public bool enemy = false;

    public GameObject spriteNeutral;
    public GameObject spritePlayer;
    public GameObject spriteEnemy;
    

    private void Start()
    {
        // Initialize cell properties if needed
    }

    private void Update()
    {
        // Update cell properties if needed
    }

    public void SetCellType(CellType newType)
    {
        cellType = newType;
    }

    public void ChangePlayer(bool isPlayer)
    {
        if(!capturable) return;
        player = isPlayer;
        enemy = !isPlayer;
        if(player){
            spriteEnemy.SetActive(false);
            spriteNeutral.SetActive(false);
            spritePlayer.SetActive(true);
        }
        else{
            spritePlayer.SetActive(false);
            spriteNeutral.SetActive(false);
            spriteEnemy.SetActive(true);
        }
    }
}