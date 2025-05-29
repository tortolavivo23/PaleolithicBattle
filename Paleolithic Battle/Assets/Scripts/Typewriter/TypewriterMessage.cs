using UnityEngine;
using System;

[Serializable]
public class MessageJson { public string Message; }
public class TypewriterJson { public MessageJson[] Messages; }

[Serializable]
public class TypewriterMessage
{
    private float timer = 0;
    private int charIndex = 0;
    private float timePerChar = 0.05f;
    [SerializeField]
    public string currentMsg = null;
    private string displayMsg = null;

    private Action onActionCallback = null;

    public TypewriterMessage(string msg, Action callback = null)
    {
        currentMsg = msg;
        onActionCallback = callback;
    }

    public void Callback()
    {
        onActionCallback?.Invoke();
    }

    public string GetFullMsgAndCallback()
    {
        onActionCallback?.Invoke();
        return currentMsg;
    }

    public string GetFullMsg()
    {
        return currentMsg;
    }

    public string GetMsg()
    {
        return displayMsg;
    }

    public void Update()
    {
        if (string.IsNullOrEmpty(currentMsg)) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timePerChar;
            charIndex++;

            displayMsg = currentMsg.Substring(0, charIndex);
            displayMsg += "<color=#00000000>" + currentMsg.Substring(charIndex) + "</color>";
            if (charIndex >= currentMsg.Length)
            {
                currentMsg = null;
            }
        }
    }

    public void SetCallback(Action callback)
    {
        onActionCallback = callback;
    }

    public bool IsActive()
    {
        if (string.IsNullOrEmpty(currentMsg)) return false;
        return charIndex < currentMsg.Length;
    }

}