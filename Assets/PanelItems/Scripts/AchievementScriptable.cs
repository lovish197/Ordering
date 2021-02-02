using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement/CreateNewAchievement")]
public class AchievementScriptable : ScriptableObject
{
    //string title, string description, int points, int spriteIndex, int progress, string[] dependencies = null
    public string Title;
    public string Description;
    public int Points;
    public int SpriteIndex;
    public int Progress;
    public string[] DependenciesTitles;

}

