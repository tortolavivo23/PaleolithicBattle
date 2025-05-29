using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypewriterAssistant : MonoBehaviour
{
    public TypeWriterScriptableObject scriptableObject;

    public SpriteRenderer[] characters;

    public string nextSceneName;

    int index;

    private Typewriter typewriter;

    private void Start()
    {
        typewriter = Typewriter.Instance;
        scriptableObject.AddCallback(ChangeSpeaker);
        // Add messages from the scriptable object to the typewriter
        Typewriter.Add(scriptableObject);

        // Activate the typewriter to start displaying messages
        Typewriter.Activate();

        characters[0].color = Color.white;
        for (int i = 1; i < characters.Length; i++)
        {
            characters[i].color = Color.grey;
        }
        index = 0;
    }

    private void ChangeSpeaker()
    {
        characters[index].color = Color.grey;
        index++;
        index %= characters.Length;
        characters[index].color = Color.white;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Write the next message in the queue
            typewriter.WriteNextMessageInQueue();
        }

        if(typewriter.endedLastMessage)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}