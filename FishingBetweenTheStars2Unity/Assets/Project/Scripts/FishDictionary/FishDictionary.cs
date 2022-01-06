
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
public class FishDictionary : UdonSharpBehaviour
{
  private FishData[] FishArray;
  [SerializeField] private Sprite[] spriteArray;

  public GameObject fishTemplate;
  void Start()
  {
    FishArray = new FishData[34];
    BuildFishDictionary();
  }

  void BuildFishDictionary()
  {
    //Rarities ( 0 = Common , 1 = Uncommon, 2 = Rare, 3 = Legendary)
    //Tiers ( 0 --> 5)
    //If Favorite Fishh is 9999 , that means it has no favorite
    GameObject temp;
    for (int i = 0; i < FishArray.Length; i++)
    {
      temp = VRCInstantiate(fishTemplate);
      temp.transform.SetParent(transform);
      FishArray[i] = temp.GetComponent<FishData>();
    }
    //Location A (Different Locations have different pools of fish)
    //Tier 0 (ID , Name, Description, Rarity, Tier, Value, Base Chance, Power, Favorite Fish)
   FishArray[0].SetFishData(0,spriteArray[0],"Action Figure", "Very Pog Pepe Action Figure",1, 0, 75, .2f, 0,9999);
   FishArray[1].SetFishData(1,spriteArray[1], "Tin Can", "Wow! They really have it! Canned Bread!", 0, 0, 50, .25f, 0, 9999);
   FishArray[2].SetFishData(2,spriteArray[2], "Squeaky Boots", "Who knew fish had such great taste in fashion?!", 0, 0, 50, .3f, 0, 9999);
   FishArray[3].SetFishData(3,spriteArray[3], "Worm Bundle", "A Bundle of 3 Worms", 1, 0, 0, .2f, 0, 9999);
   FishArray[4].SetFishData(4,spriteArray[4], "Wooden Crate", "A crate found in poor condition. Holds various trinkets that are worth a pretty penny!", 3, 0, 200, .05f, 0, 9999);
    
   //Tier 1 (ID , Name, Description, Rarity, Tier, Value, Base Chance, Power, Favorite Fish)
   FishArray[5].SetFishData(5, spriteArray[5], "Bloat Belly GoldFish", "A gold fish whose belly looks as if it will burst at any moment!", 0, 1, 100, .35f, 110, 9999);
   FishArray[6].SetFishData(6, spriteArray[6], "Cyclops Anchovie", "A tiny fish with a large bulbous eye black eye. ", 0, 1, 100, .35f, 110, 9999);
   FishArray[7].SetFishData(7, spriteArray[7], "Curved Fin Sardine", "Its fins curve just like the moon...", 2, 1, 125, .15f, 155, 9999);
   FishArray[8].SetFishData(8, spriteArray[8], "Void Riddled Carp", "A fish ladent with energy of the endless void...", 2, 1, 150, .10f, 175, 9999);
   FishArray[9].SetFishData(9, spriteArray[9], "Silver Crate", "A heavy metal box filled with ores and metal scrap.", 3, 1, 350, .05f, 0, 9999);

   //Tier 2 (ID , Name, Description, Rarity, Tier, Value, Base Chance, Power, Favorite Fish)
   FishArray[10].SetFishData(10, spriteArray[10], "Rainbow Trout", "A fish whose scales shine all colors of the rainbow!", 1, 2, 275, .25f, 160, 9999);
   FishArray[11].SetFishData(11, spriteArray[11], "Pink Bucky", "A fish with horns resembling that of a deer!", 0, 2, 250, .35f, 155, 9999);
   FishArray[12].SetFishData(12, spriteArray[12], "Sunfish", "It's scales are sharp and emit blinding light...It's hot to the touch..", 3, 2, 425, .10f, 155, 9999);
   FishArray[13].SetFishData(13, spriteArray[13], "Thunder-Cloud Shark", "Don't touch it or it'll shock you!", 2, 2, 375, .15f, 175, 9999);
   FishArray[14].SetFishData(14, spriteArray[14], "Ghostfish", "It remains still if stared directly at...", 2, 2, 425, .10f, 155, 9999);
   FishArray[15].SetFishData(15, spriteArray[15], "Gold Crate", "Large crate filled with many gems and gold ", 3, 2, 500, .05f, 0, 9999);


   //Tier 3 (ID , Name, Description, Rarity, Tier, Value, Base Chance, Power, Favorite Fish)
   FishArray[16].SetFishData(16, spriteArray[16], "Lion Trout", "A gold fish whose belly looks as if it will burst at any moment!", 2, 3, 550, .15f, 210, 9999);
   FishArray[17].SetFishData(17, spriteArray[17], "Void Fish", "Its scales Ooze with energy from another dimension...Is there really an endless void..?", 0, 3, 500, .30f, 210, 9999);
   FishArray[18].SetFishData(18, spriteArray[18], "Cosmic Discus", "A fish whose body resmbles the ring of a planet.. ", 2, 3, 600, .10f, 210, 9999);
   FishArray[19].SetFishData(19, spriteArray[19], "Quantum Pufferfish", "Don't take your eyes off it or else it'll move!", 0, 3, 750, .30f, 210, 9999);
   FishArray[20].SetFishData(20, spriteArray[20], "LamBass", "A fish that does not easily fall for bait", 2, 3, 500, .10f, 210, 9999);
   FishArray[21].SetFishData(21, spriteArray[21], "Platinum Crate", "This will sell for a nice price.", 3, 3, 1000, .05f, 0, 9999);


   //Tier 4 (ID , Name, Description, Rarity, Tier, Value, Base Chance, Power, Favorite Fish)
   FishArray[22].SetFishData(22, spriteArray[22], "Golden Blob Fish", "Why did he do it...?", 2, 4, 1200, .15f, 270, 9999);
   FishArray[23].SetFishData(23, spriteArray[23], "Ashen Octopus", "Its 8 tentacles are chipped with lava and obsidian", 0, 4, 1000, .30f, 270, 9999);
   FishArray[24].SetFishData(24, spriteArray[24], "Volcanic Sting Ray", "Its body is hardened with rock and metal...", 0, 4, 1000, .10f, 270, 9999);
   FishArray[25].SetFishData(25, spriteArray[25], "Planetary Parasite", "A worm who feast on the power emitted from a planet...", 3, 4, 2000, .10f, 270, 9999);
   FishArray[26].SetFishData(26, spriteArray[26], "Lava Eel", "Lava worm go Brrrr..", 2, 4, 1250, .30f, 270, 9999);
   FishArray[27].SetFishData(27, spriteArray[27], "Diamond Crate", "Its less of a crate and more of a large BLOCK!", 3, 4, 5000, .05f, 0, 9999);

   //Tier 5 (ID , Name, Description, Rarity, Tier, Value, Base Chance, Power, Favorite Fish)
   FishArray[28].SetFishData(28, spriteArray[28], "Forbidden Eye", "Why are we trapped on this rock...?", 2, 5, 0, .15f, 0, 9999);
   FishArray[29].SetFishData(29, spriteArray[29], "Hand of an Old God", "Is there a reason I must fish..?", 1, 5, 0, .30f, 0, 9999);
   FishArray[30].SetFishData(30, spriteArray[30], "Eater of Worlds", "A creature that comes from a dimension outside of our own...", 1, 5, 6000, .10f, 0, 9999);
   FishArray[31].SetFishData(31, spriteArray[31], "Jar of Light", "Its light is enough to drive a man mad...", 3, 5, 0, .10f, 0, 9999);
   FishArray[32].SetFishData(32, spriteArray[32], "Statue of Blinding Faith", "A statue whose power is felt through even the most unholy of nights..", 0, 5, 5000, .30f, 0, 9999);
   FishArray[33].SetFishData(33, spriteArray[33], "Void Crate", "A crate with treasure beyond ones comprehension", 3, 5, 10000, .05f, 0, 9999);
   
  }

  public FishData getFishData(int ID)
  {
    return FishArray[ID];
  }

}
