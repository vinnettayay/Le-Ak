using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Story", menuName = "Stories/Cutscenes")]
public class Cutscene_ScriptableObjects : ScriptableObject
{
    public List<Sprite> scenes;
}