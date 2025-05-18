using System;

[Serializable]
public class Minigames{
    public string[] names;

    public string RandomMinigame()
    {
        Random random = new Random();
        int index = random.Next(names.Length);
        return names[index];
    }

}