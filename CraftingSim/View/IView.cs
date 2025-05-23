using System.Collections.Generic;
using CraftingSim.Model;

namespace CraftingSim.View
{
    public interface IView
    {
        string ShowMainMenu();
        void DisplayMaterials(Inventory inventory);
        void DisplayRecipes(IEnumerable<IRecipe> recipes);
        string AskForRecipe(IEnumerable<IRecipe> recipes);
        void ShowResults(string message);
    }
}