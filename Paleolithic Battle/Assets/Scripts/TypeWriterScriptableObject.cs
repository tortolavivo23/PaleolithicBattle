using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TypeWriterScriptableObject", menuName = "ScriptableObjects/TypeWriterScriptableObject", order = 1)]
public class TypeWriterScriptableObject : ScriptableObject
{
    public List<TypewriterMessage> Messages = new List<TypewriterMessage>();

    public void AddCallback(Action callback)
    {
        foreach (TypewriterMessage message in Messages)
        {
            message.SetCallback(callback);
        }
    }

}