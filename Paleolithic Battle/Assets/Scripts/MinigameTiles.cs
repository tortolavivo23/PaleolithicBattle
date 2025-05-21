using System.Collections;
using UnityEngine;

public class MinigameTiles : MonoBehaviour
{
    LevelManager levelManager;
    public GameObject InstrucionPanel;

    public float timeInstructions = 1f;
    public Transform lastSpawnedNote;
    private static float noteHeight;
    private static float noteWidth;
    public Note notePrefab;
    private Vector3 noteLocalScale;
    private float noteSpawnStartPosX;
    public float noteSpeed;
    public const int NotesToSpawn = 20;
    private int prevRandomNoteIndex;

    public Transform noteContainer;

    public Camera mainCamera;

    private int lastNoteId = 1;
    public int LastPlayedNoteId { get; set; } = 0;

    public bool IsPlaying { get; set; } = true;

    public int tilesToWin = 17;

    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        Invoke("HideInstructionPanel", timeInstructions);   
    }

    void HideInstructionPanel()
    {
        InstrucionPanel.SetActive(false);
        SetDataForNoteGeneration();
        SpawnNotes();
    }

    private void SetDataForNoteGeneration()
    {
        var topRight = new Vector3(Screen.width, Screen.height, 0);
        var topRightWorldPoint = mainCamera.ScreenToWorldPoint(topRight);
        var bottomLeftWorldPoint = mainCamera.ScreenToWorldPoint(Vector3.zero);
        var screenWidth = topRightWorldPoint.x - bottomLeftWorldPoint.x;
        var screenHeight = topRightWorldPoint.y - bottomLeftWorldPoint.y;
        noteHeight = screenHeight / 4;
        noteWidth = screenWidth / 4;
        var noteSpriteRenderer = notePrefab.GetComponent<SpriteRenderer>();
        noteLocalScale = new Vector3(
               noteWidth / noteSpriteRenderer.bounds.size.x * noteSpriteRenderer.transform.localScale.x,
               noteHeight / noteSpriteRenderer.bounds.size.y * noteSpriteRenderer.transform.localScale.y, 1);
        var leftmostPoint = mainCamera.ScreenToWorldPoint(new Vector2(0, Screen.height / 2));
        var leftmostPointPivot = leftmostPoint.x + noteWidth / 2;
        noteSpawnStartPosX = leftmostPointPivot;
    }

    public void SpawnNotes()
    {
        var noteSpawnStartPosY = lastSpawnedNote.position.y + noteHeight;
        Note note = null;
        for (int i = 0; i < NotesToSpawn; i++)
        {
            var randomIndex = GetRandomIndex();
            for (int j = 0; j < 4; j++)
            {
                note = Instantiate(notePrefab, noteContainer.transform);
                note.minigameTiles = this;
                note.transform.localScale = noteLocalScale;
                note.transform.position = new Vector2(noteSpawnStartPosX + noteWidth * j, noteSpawnStartPosY);
                note.Visible = j == randomIndex;
                if (note.Visible)
                {
                    note.Id = lastNoteId;
                    lastNoteId++;
                }
            }
            noteSpawnStartPosY += noteHeight;
            if (i == NotesToSpawn - 1) lastSpawnedNote = note.transform;
        }
    }

    private int GetRandomIndex()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, 4);
        } while (randomIndex == prevRandomNoteIndex);
        prevRandomNoteIndex = randomIndex;
        return randomIndex;
    }

    private void Update()
    {
        DetectNoteClicks();
    }

    private void DetectNoteClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var origin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(origin, Vector2.zero);
            if (hit)
            {
                var gameObject = hit.collider.gameObject;
                if (gameObject.CompareTag("Note"))
                {
                    var note = gameObject.GetComponent<Note>();
                    note.Play();
                    if (IsPlaying && tilesToWin <= 0)
                    {
                        StartCoroutine(Win());
                    }
                }
            }
        }
    }

    public IEnumerator Win()
    {
        if (!IsPlaying) yield break;
        IsPlaying = false;
        yield return new WaitForSeconds(0.5f);
        // Handle winning logic here
        levelManager.minigameResult = 1;
    }

    public IEnumerator Lose()
    {
        if (!IsPlaying) yield break;
        IsPlaying = false;
        yield return new WaitForSeconds(0.5f);
        // Handle losing logic here
        Debug.Log("You lost!");
        levelManager.minigameResult = -1;
    }
}
