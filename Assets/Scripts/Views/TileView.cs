using Services.Board;
using UnityEngine;

namespace Views
{
    public class TileView : MonoBehaviour
    {
        public TileType InitialType { get; private set; }
        public TileModel Model { get; private set; }

        public void Initialize(TileModel tileModel)
        {
            InitialType = tileModel.Type;
            Model = tileModel;
        }
    }
}