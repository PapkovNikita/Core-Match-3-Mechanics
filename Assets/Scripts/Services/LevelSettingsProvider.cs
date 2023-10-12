using Settings;

namespace Services
{
    public interface ILevelSettingsProvider
    {
        ILevelSettings GetCurrentLevelSettings();
    }

    public class LevelSettingsProvider : ILevelSettingsProvider
    {
        private readonly GameSettings _gameSettings;
        private int _currentLevel = 0;

        public LevelSettingsProvider(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public ILevelSettings GetCurrentLevelSettings()
        {
            return _gameSettings.Levels[_currentLevel];
        }
    }
}