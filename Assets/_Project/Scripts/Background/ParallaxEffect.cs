using UnityEngine;

namespace OctanGames.Background
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private ParallaxComponent[] _parallaxComponents;

        private Vector3 _lastCameraPosition;

        private void LateUpdate()
        {
            Vector3 cameraPosition = _camera.position;

            Vector3 deltaMovement = cameraPosition - _lastCameraPosition;

            foreach (ParallaxComponent parallaxComponent in _parallaxComponents)
            {
                Vector2 parallaxWeight = parallaxComponent.ParallaxWeight;
                Transform spriteTransform = parallaxComponent.SpriteRenderer.transform;

                spriteTransform.position += new Vector3(
                    deltaMovement.x * parallaxWeight.x,
                    deltaMovement.y * parallaxWeight.y,
                    0);

                if (Mathf.Abs(cameraPosition.x - spriteTransform.position.x) >= parallaxComponent.TextureUnitSizeX)
                {
                    float offsetPositionX = (cameraPosition.x - spriteTransform.position.x) %
                                            parallaxComponent.TextureUnitSizeX;
                    spriteTransform.position = new Vector3(cameraPosition.x + offsetPositionX, spriteTransform.position.y);
                }
            }

            _lastCameraPosition = cameraPosition;
        }
    }
}