using Services.Board;
using UnityEngine;

namespace Views
{
    public class TileView : MonoBehaviour
    {
        public TileModel Model { get; private set; }

        public void Initialize(TileModel model)
        {
            Model = model;
        }
    }
}