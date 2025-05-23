using System;
using CraftingSim.Controller;
using CraftingSim.Model;
using CraftingSim.View;


namespace CraftingSim {
    public class Program {

        //Parses command line arguments, initializes the model
        //and starts the controller loop.
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error! command should be: dotnet run --project" +
                " CraftingSim -- <materials.txt> <recipe1.txt> <recipe2.txt> ...");
                Environment.Exit(-1);
            }

            //args[0] = materials, args[1], args[2], etc = recipes
            string[] recipeFiles = new string[args.Length - 1];
            for (int i = 1; i < args.Length; i++)
                recipeFiles[i - 1] = args[i];

            Inventory inventory = new Inventory();
            inventory.LoadMaterialsFromFile(args[0]);

            ICrafter crafter = new Crafter(inventory);
            crafter.LoadRecipesFromFile(recipeFiles);

            IView view = new ConsoleView();
            CraftingController controller = new CraftingController(
                inventory, crafter, view);
            controller.Run();

        }
    }
}
