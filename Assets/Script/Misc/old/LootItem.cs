using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Items")]

public class LootItem : ScriptableObject
{
    public enum PickUpType
    {
        ScorePoint,
        HealthGlobe,
    }

    public Sprite loopSprite;
    public string lootName;
    public int dropChance;
    [SerializeField] private PickUpType pickUpType;
    public int score;
    public int healpoint;


    private void OnValidate()
    {
        if(pickUpType == PickUpType.ScorePoint)
        {
            healpoint = 0;
            if(score <=0)
            {
                score = 10;
            }
        }
        else if(pickUpType == PickUpType.HealthGlobe)
        {
            score = 0;
            if (healpoint <= 0)
            {
                healpoint = 10;
            }
        }
    }
    public PickUpType GetPickUpType()
    {
        return pickUpType;
    }
}
