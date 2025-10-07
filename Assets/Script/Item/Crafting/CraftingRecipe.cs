using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public ItemData inputA;
    public ItemData inputB;
    public ItemData result;
}
