using System.Collections.Generic;
using Spectre.Console;
using CraftingSim.Model;

namespace CraftingSim.View
{
    /// <summary>
    /// View for console that handle the user interaction.
    /// </summary>
    public class ConsoleView : IView
    {
        /// <summary>
        /// Shows the main menu and asks the user to choose an option
        /// </summary>
        /// <returns>The selected option</returns>
        public string ShowMainMenu()
        {
            AnsiConsole.MarkupLine("[bold cyan]#     Andre's Forge     #[/]");
            AnsiConsole.MarkupLine("[italic cyan]If you require smithing, " +
                                "speak to me.[/]\n");

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an option")
                    .AddChoices(new[] { "View Materials",
                        "View Recipes",
                        "Craft Item",
                        "Exit" }));
        }

        /// <summary>
        /// Displays all materials and their quantities in a table.
        /// </summary>
        /// <param name="materials">Read only dictionary of materials and 
        /// their quantities</param>
        public void DisplayMaterials(Inventory inventory)
        {
            Table table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Quantity");

            foreach (IMaterial material in inventory.Materials)
            {
                table.AddRow(
                    material.Id.ToString(),
                    material.Name,
                    inventory.GetQuantity(material).ToString()
                );
            }
            AnsiConsole.Write(table);
        }

        /// <summary>
        /// Displays all recipes and the required materials in a table
        /// </summary>
        /// <param name="recipeList">Read only list of recipes to display</param>
        public void DisplayRecipes(IEnumerable<IRecipe> recipeList)
        {

            Table table = new Table();
            table.AddColumn("Recipe");
            table.AddColumn("Materials Required");

            foreach (IRecipe recipe in recipeList)
            {
                string list = "";
                foreach (KeyValuePair<IMaterial, int> entry in recipe.RequiredMaterials)
                {
                    if (list != "")
                        list += ", ";
                    list += entry.Value + "x " + entry.Key.Name;
                }

                table.AddRow(recipe.Name, list);
            }

            AnsiConsole.Write(table);
        }

        /// <summary>
        /// Asks the user to select a recipe to craft from the provided list.
        /// </summary>
        /// <param name="recipeList">Read only list of available recipes</param>
        /// <returns>The name of the selected recipe</returns>
        public string AskForRecipe(IEnumerable<IRecipe> recipeList)
        {
            List<string> names = new List<string>();
            foreach (IRecipe recipe in recipeList)
                names.Add(recipe.Name);

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a recipe to craft")
                    .AddChoices(names));
        }
        /// <summary>
        /// Displays the result of the crafting attempt.
        /// </summary>
        /// <param name="message">The result message to show</param>
        public void ShowResults(string message)
        {
            if (message.Contains("succeeded"))
                AnsiConsole.MarkupLine("[green]" + message + "[/]\n\n");
            else
                AnsiConsole.MarkupLine("[red]" + message + "[/]\n\n");
        }

    }
}