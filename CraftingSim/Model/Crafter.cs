using System;
using System.Collections.Generic;
using System.IO;


namespace CraftingSim.Model
{
    /// <summary>
    /// Implementation of ICrafter. 
    /// </summary>
    public class Crafter : ICrafter
    {
        private readonly Inventory inventory;
        private readonly List<IRecipe> recipeList;

        public Crafter(Inventory inventory)
        {
            this.inventory = inventory;
            recipeList = new List<IRecipe>();
        }

        /// <summary>
        /// returns a read only list of loaded recipes.
        /// </summary>
        public IEnumerable<IRecipe> RecipeList => recipeList;

        /// <summary>
        /// Loads recipes from the files.
        /// Must parse the name, success rate, required materials and
        /// necessary quantities.
        /// </summary>
        /// <param name="recipeFiles">Array of file paths</param>
        public void LoadRecipesFromFile(string[] recipeFiles)
        {
            using StreamReader reader = new StreamReader("recipes.txt");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                string name = parts[0];
                double successRate = double.Parse(parts[1]);
                Dictionary<IMaterial, int> requiredMaterials = new Dictionary<IMaterial, int>();

                for (int i = 2; i < parts.Length; i += 2)
                {
                    IMaterial material = new Material(parts[i]);
                    int quantity = int.Parse(parts[i + 1]);
                    requiredMaterials.Add(material, quantity);
                }

                IRecipe recipe = new Recipe(name, successRate, requiredMaterials);
                recipeList.Add(recipe);
            }
            //TODO Implement Me
        }

        /// <summary>
        /// Attempts to craft an item from a given recipe. Consumes inventory 
        /// materials and returns the result message.
        /// </summary>
        /// <param name="recipeName">Name of the recipe to craft</param>
        /// <returns>A message indicating success, failure, or error</returns>
        public string CraftItem(string recipeName)
        {
            IRecipe selected = null;

            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i].Name.Equals(recipeName,
                        StringComparison.OrdinalIgnoreCase))
                {
                    selected = recipeList[i];
                    break;
                }
            }
            
            if (selected == null)
                return "Recipe not found.";

            foreach (KeyValuePair<IMaterial, int> required in selected.RequiredMaterials)
            {
                IMaterial material = required.Key;
                int need = required.Value;
                int have = inventory.GetQuantity(material);

                if (have < need)
                {
                    if (have == 0)
                    {
                        return "Missing material: " + material.Name;
                    }
                    return "Not enough " + material.Name +
                           " (need " + need +
                           ", have " + have + ")";
                }
            }

            foreach (KeyValuePair<IMaterial, int> required in selected.RequiredMaterials)
                if (!inventory.RemoveMaterial(required.Key, required.Value))
                    return "Not enough materials";

            Random rng = new Random();
            if (rng.NextDouble() < selected.SuccessRate)
                return "Crafting '" + selected.Name + "' succeeded!";
            else
                return "Crafting '" + selected.Name + "' failed. Materials lost.";

        }
    }

    public class Recipe : IRecipe
    {
        public string Name { get; }
        public double SuccessRate { get; }
        public Dictionary<IMaterial, int> RequiredMaterials { get; }

        IReadOnlyDictionary<IMaterial, int> IRecipe.RequiredMaterials => RequiredMaterials;

        public Recipe(string name, double successRate,
                Dictionary<IMaterial, int> requiredMaterials)
        {
            Name = name;
            SuccessRate = successRate;
            RequiredMaterials = requiredMaterials;
        }

        public int CompareTo(IRecipe other)
        {
            if (other == null) return 1;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }

    public class Material : IMaterial
    {
        public int Id { get; }
        public string Name { get; }

        public Material(string name)
        {
            //Name = name;
            Id = new Random().Next(1, 1000); // Generate a random ID for the material
        }

        public Material(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public bool Equals(IMaterial other)
        {
            return other != null && (Id == other.Id || Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IMaterial);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}