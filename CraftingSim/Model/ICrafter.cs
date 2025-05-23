using System.Collections.Generic;

namespace CraftingSim.Model
{
    /// <summary>
    /// Defines methods for loading recipes and crafting items.
    /// </summary>
    public interface ICrafter
    {
        //list of recipes
        IEnumerable<IRecipe> RecipeList { get; }
        void LoadRecipesFromFile(string[] recipeFiles);
        string CraftItem(string recipeName);
    }
}
