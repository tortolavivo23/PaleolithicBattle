using UnityEngine;

public class OutOfScreenTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered" + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Note"))
        {
            Debug.Log("Note out of screen");
            var note = collision.gameObject.GetComponent<Note>();
            note.OutOfScreen();
        }
    }
}
