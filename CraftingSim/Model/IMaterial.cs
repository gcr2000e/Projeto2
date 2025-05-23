using System;

namespace CraftingSim.Model
{
    /// <summary>
    /// Represents a Material used to craft an item
    /// Materials have an Id and a Name
    /// Materials are the same if either the Id or the Name are equal
    /// </summary>
    public interface IMaterial : IEquatable<IMaterial>
    {
        //Id of the material
        int Id { get; }
        //name of the material
        string Name { get; }
    }
}