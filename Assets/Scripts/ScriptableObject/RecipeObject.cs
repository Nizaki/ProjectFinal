using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Game/Recipe")]
public class RecipeObject : ScriptableObject
{

    public ItemStack[] Input;
    public ItemStack Output;
}