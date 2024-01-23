using BowloutModManager.BowloutMod.Interfaces;
using UnityEngine;

namespace BowloutHierarchyMod
{
    public class BowloutHierarchyModConfiguration : IBowloutConfiguration
    {
        public int Version { get => 1; set => _ = value; }
    }
}
