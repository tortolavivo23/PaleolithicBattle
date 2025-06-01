using UnityEngine;

public class Note : MonoBehaviour
{
    private Animator animator;
    public MinigameTiles minigameTiles;

    private bool visible;

    public int Id { get; set; }
    public bool Visible
    {
        get => visible;
        set
        {
            visible = value;
            if (!visible) animator.Play("Invisible");
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (minigameTiles.IsPlaying)
            transform.Translate(Vector2.down * minigameTiles.noteSpeed * Time.deltaTime);
    }

    public bool Played { get; set; }

    public void Play()
    {
        if(minigameTiles.IsPlaying)
            if (Visible)
            {
                if (!Played && minigameTiles.LastPlayedNoteId == Id - 1)
                {
                    Played = true;
                    minigameTiles.LastPlayedNoteId = Id;
                    Played = true;
                    animator.Play("Played");
                    AudioManager.Instance.RandomPitch("NotePlayed", 0.7f, 1.3f);
                    minigameTiles.tilesToWin--;
                }
            }
            else
            {
                StartCoroutine(minigameTiles.Lose());
                animator.Play("Missed");
            }
    }

    public void OutOfScreen()
    {
        if (Visible && !Played)
        {
            StartCoroutine(minigameTiles.Lose());
            animator.Play("Missed");
        }
    }
        
}
