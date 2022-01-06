
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class FishData : UdonSharpBehaviour
{
  public int id;
  private int value;
  private float baseChance;
  private int power;
  private int favoriteFish;
  private string fishName;
  private int rarity;
  private int tier;
  private string description;
  private Sprite sprite;

  public void SetFishData(int id,Sprite sprite ,string fishname, string description, int rarity, int tier, int Value, float BaseChance, int Power, int FavoriteFish)
  {
    this.id = id;
    this.fishName = fishname;
    this.description = description;
    this.rarity = rarity;
    this.value = Value;
    this.baseChance = BaseChance;
    this.power = Power;
    this.favoriteFish = FavoriteFish;
    this.tier = tier;
    this.sprite = sprite;
  }
  public int getFishId()
  {
    return id;
  }
  public string getFishName()
  {
    return fishName;
  }
  public string getFishDescription()
  {
    return description;
  }
  public int getFishRarity()
  {
    return rarity;
  }
  public int getFishTier()
  {
    return tier;
  }
  public int getFishValue()
  {
    return value;
  }
  public int getFishPower()
  {
    return power;
  }
  public float getFishBaseChance()
  {
    return baseChance;
  }
  public int getFavoriteFish()
  {
    return favoriteFish;
  }

  public Sprite getFishSprite()
  {
    return sprite;
  }

}
