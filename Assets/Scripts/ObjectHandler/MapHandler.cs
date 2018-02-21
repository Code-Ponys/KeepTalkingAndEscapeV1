    public class MapHandler {
        public void RegisterButton(MapButton button) {
            _buttons.Add(button);
            _gameManager = GameManager.GetGameManager();
        }
