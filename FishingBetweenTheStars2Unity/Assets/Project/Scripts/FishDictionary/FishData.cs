
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
  void Start()
    {
        
    }

  public void SetFishData(int id, string fishname, string description, int rarity, int tier, int Value, float BaseChance, int Power, int FavoriteFish)
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
  }
}
