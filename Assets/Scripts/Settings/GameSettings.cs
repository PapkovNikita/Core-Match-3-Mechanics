using UnityEngine;

namespace Settings
{
    
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/Game Settings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public LevelSettings[] Levels { get; private set; }
    }
}