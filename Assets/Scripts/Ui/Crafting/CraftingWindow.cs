using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
  public List<RecipeObject> recipes;
  [SerializeField]
  GameObject recipeHolder;
  [SerializeField]
  GameObject recipeTemplate;
  // Start is called before the first frame update
  void Start()
  {
    foreach (var item in recipes)
    {
      if (item == null)
        return;
      var go = Instantiate(recipeTemplate, Vector2.zero, Quaternion.identity);
      go.transform.SetParent(recipeHolder.transform);
      go.GetComponent<RecipeSlot>().recipe = item;
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
