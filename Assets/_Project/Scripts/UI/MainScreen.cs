using UnityEngine;

namespace OctanGames.UI
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private TrainRider _trainRider;
        [SerializeField] private ButtonHandler _runButton;

        private void Start()
        {
            _runButton.Pressed += OnButtonPressed;
            _runButton.Released += OnButtonReleased;
        }

        private void OnDestroy()
        {
            _runButton.Pressed -= OnButtonPressed;
            _runButton.Released -= OnButtonReleased;
        }

        private void OnButtonPressed()
        {
            _trainRider.StartMovement();
        }
        private void OnButtonReleased()
        {
            _trainRider.StopMovement();
        }
    }
}