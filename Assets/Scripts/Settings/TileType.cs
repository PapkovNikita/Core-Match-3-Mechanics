using System;
using UnityEngine;
using Views;

namespace Settings
{
    [Serializable]
    public class TileType
    {
        [field: SerializeField] public TileView ViewPrefab { get; private set; }
    }
}