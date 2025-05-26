using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    public TextMeshProUGUI TextComponent;
    private static Typewriter instance;
    private List<TypewriterMessage> messages = new List<TypewriterMessage>();

    private TypewriterMessage currentMessage = null;
    private int msgIndex = 0;

    [HideInInspector] public bool endedLastMessage = false;

    public static Typewriter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<Typewriter>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("Typewriter");
                    instance = obj.AddComponent<Typewriter>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (messages.Count > 0 && currentMessage != null)
        {
            currentMessage.Update();
            TextComponent.text = currentMessage.GetMsg();
        }
    }

    public static void Add(string msg, Action callback = null)
    {
        TypewriterMessage message = new TypewriterMessage(msg, callback);
        instance.messages.Add(message);
    }

    public static void Add(TypeWriterScriptableObject scrObj)
    {
        foreach (var message in scrObj.Messages)
        {
            TypewriterMessage msg = new TypewriterMessage(message.GetFullMsg(), message.Callback);
            instance.messages.Add(msg);
        }
    }

    public static void Add(TypewriterJson json)
    {
        foreach (var message in json.Messages)
        {
            TypewriterMessage msg = new TypewriterMessage(message.Message);
            instance.messages.Add(msg);
        }
    }

    public static void Activate()
    {
        instance.currentMessage = instance.messages[0];
    }

    public void WriteNextMessageInQueue()
    {
        if (currentMessage != null && currentMessage.IsActive())
        {
            TextComponent.text = currentMessage.GetFullMsg();
            currentMessage = null;
            return;
        }

        msgIndex++;


        if (msgIndex >= messages.Count)
        {
            currentMessage = null;
            TextComponent.text = "";
            endedLastMessage = true;
            return;
        }

        currentMessage = messages[msgIndex];
        currentMessage.Callback();
    }

    
}
