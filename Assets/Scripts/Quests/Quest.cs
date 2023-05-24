using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string name;
    public string description;
    public int reward;
    public bool isCompleted;
    // Add other properties as needed

    public Quest(string name, string description, int reward)
    {
        this.name = name;
        this.description = description;
        this.reward = reward;
        this.isCompleted = false;
    }
}
