using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Items")]

public class LootItem : ScriptableObject
{
    public Sprite loopSprite;
    public string lootName;
    public int dropChance;

    
}
