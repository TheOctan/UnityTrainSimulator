using System;
using UnityEngine;
using UnityEngine.UI;

namespace OctanGames.UI
{
    public class MainScreen : MonoBehaviour
    {
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
            
        }
        private void OnButtonReleased()
        {
            
        }
    }
}