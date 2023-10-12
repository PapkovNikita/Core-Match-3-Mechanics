using System;
using UnityEngine;
using Views;

namespace Settings
{
    [Serializable]
    public class TileSettings
    {
        [field: SerializeField] public TileView Tile { get; private set; }
    }
}