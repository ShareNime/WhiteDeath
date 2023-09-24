using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
class Quest
{
    public bool isActive;
    public string title;
    public string description;

    public int goal;
    
    public int currProgress = 0;
}
