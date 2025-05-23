using CraftingSim.Model;
using CraftingSim.View;

namespace CraftingSim.Controller 
{
    /// <summary>
    /// Coordinates information between View and Model and invokes 
    /// Model methods to display or craft items.
    /// </summary>
    public class CraftingController
    {
        private readonly Inventory inventory;
        private readonly ICrafter crafter;
        private readonly IView view;


        //Initializes the controller with inventory, crafter, and view.
        public CraftingController(Inventory inventory, ICrafter crafter,
                IView view)
        {
            this.inventory = inventory;
            this.crafter = crafter;
            this.view = view;
        }

        /// <summary>
        /// Contains the main loop. 
        /// Asks the View to show menus, gets information from the model given 
        /// the user choices and passes it on to the view, and exits on command.
        /// </summary>
        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                string option = view.ShowMainMenu();

                switch (option)
                {
                    case "View Materials":
                        view.DisplayMaterials(inventory);
                        break;
                    case "View Recipes":
                        view.DisplayRecipes(crafter.RecipeList);
                        break;
                    case "Craft Item":
                        string recipeName = view.AskForRecipe(crafter.RecipeList);
                        string result = crafter.CraftItem(recipeName);
                        view.ShowResults(result);
                        break;
                    case "Exit":
                        exit = true;
                        break;
                }
            }
        }
    }
}