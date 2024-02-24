using System.Collections.Generic;
using UnityEngine;

namespace OctanGames.Vehicle
{
    public class TrainRider : MonoBehaviour
    {
        [SerializeField] private float _startRotationSpeed = 100;
        [SerializeField] private float _maxRotationSpeed;
        [SerializeField] private float _acceleration = 10;
        [SerializeField] private bool _invertRotation = true;

        [Header("Components")]
        [SerializeField] private List<WheelJoint2D> _wheels;

        private bool _movementStarted;
        private float _currentRotationSpeed;

        private float Acceleration => _invertRotation ? -_acceleration : _acceleration;

        private void FixedUpdate()
        {
            if (_movementStarted)
            {
                _currentRotationSpeed += Acceleration;
            }
            else
            {
                _currentRotationSpeed -= Acceleration;
                _currentRotationSpeed = _invertRotation
                    ? Mathf.Min(0, _currentRotationSpeed)
                    : Mathf.Max(0, _currentRotationSpeed);
            }

            ApplyWheelRotation(_currentRotationSpeed);
        }

        public void StartMovement()
        {
            _movementStarted = true;

            if (Mathf.Approximately(_currentRotationSpeed, 0f))
            {
                ApplyWheelRotation(_invertRotation
                    ? -_startRotationSpeed
                    : _startRotationSpeed);
            }
        }

        public void StopMovement()
        {
            _movementStarted = false;
        }

        private void ApplyWheelRotation(float rotationSpeed)
        {
            foreach (WheelJoint2D wheel in _wheels)
            {
                JointMotor2D motor = wheel.motor;
                motor.motorSpeed = rotationSpeed;
                _currentRotationSpeed = rotationSpeed;
                wheel.motor = motor;
            }
        }
    }
}
