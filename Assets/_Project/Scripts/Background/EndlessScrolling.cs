using UnityEngine;

namespace OctanGames.Background
{
    public class EndlessScrolling : MonoBehaviour
    {
        private const float BORDER = 2f;

        [SerializeField] private Camera _viewCamera;
        [SerializeField] private GameObject[] _layers;

        private Vector2 _screenBounds;
        private Vector3 _lastCameraPosition;
        private GameObject[] _donorLayers;

        private float[] _parallaxSpeedForLayer;
        private float[] _layersWidth;

        private void Start()
        {
            Vector3 cameraPosition = _viewCamera.transform.position;

            _screenBounds = _viewCamera
                .ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraPosition.z));
            _lastCameraPosition = cameraPosition;

            CalculateBackground();
            CalculateParallaxSpeed();
        }

        private void LateUpdate()
        {
            Vector3 cameraPosition = _viewCamera.transform.position;
            Vector3 deltaMovement = cameraPosition - _lastCameraPosition;

            for (var i = 0; i < _donorLayers.Length; i++)
            {
                GameObject layer = _donorLayers[i];
                MoveChildObjectsInLayer(layer, i);

                layer.transform.position += deltaMovement.x * _parallaxSpeedForLayer[i] * Vector3.right;
            }

            _lastCameraPosition = cameraPosition;
        }

        private void CalculateBackground()
        {
            _donorLayers = new GameObject[_layers.Length];
            _layersWidth = new float[_layers.Length];

            for (var i = 0; i < _layers.Length; i++)
            {
                var layer = _layers[i].GetComponent<SpriteRenderer>();
                _layersWidth[i] = layer.bounds.size.x;

                _donorLayers[i] = new GameObject(layer.name + " (donor)");
                _donorLayers[i].transform.SetParent(transform, true);
                _donorLayers[i].transform.position = layer.transform.position;

                LoadChildObjects(layer, _donorLayers[i], _layersWidth[i]);
            }
        }

        private void CalculateParallaxSpeed()
        {
            _parallaxSpeedForLayer = new float[_donorLayers.Length];

            Vector3 cameraPosition = _viewCamera.transform.position;
            for (var i = 0; i < _donorLayers.Length; i++)
            {
                Vector3 layerPosition = _donorLayers[i].transform.position;
                _parallaxSpeedForLayer[i] = 1 - Mathf.Clamp01(Mathf.Abs(cameraPosition.z / layerPosition.z));
            }
        }

        private void LoadChildObjects(SpriteRenderer original, GameObject parent, float objectWidth)
        {
            var childAmount = (int)Mathf.Ceil(_screenBounds.x * 2 / objectWidth);

            for (var i = 0; i <= childAmount; i++)
            {
                SpriteRenderer c = Instantiate(original, parent.transform);
                Vector3 parentPosition = parent.transform.position;
                c.transform.position = new Vector3(objectWidth * i, parentPosition.y, parentPosition.z);
                c.name = original.name + i;
            }
            original.gameObject.SetActive(false);
        }

        private void MoveChildObjectsInLayer(GameObject parent, int layerNumber)
        {
            int childCount = parent.transform.childCount;
            if (childCount <= 1)
            {
                return;
            }

            GameObject firstChild = parent.transform.GetChild(0).gameObject;
            GameObject lastChild = parent.transform.GetChild(childCount - 1).gameObject;
            float halfObjectWidth = _layersWidth[layerNumber] * 0.5f;

            Vector3 cameraPosition = _viewCamera.transform.position;
            if (cameraPosition.x + _screenBounds.x > lastChild.transform.position.x + halfObjectWidth - BORDER)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2,
                    lastChild.transform.position.y,
                    lastChild.transform.position.z);
            }
            else if (cameraPosition.x - _screenBounds.x < firstChild.transform.position.x - halfObjectWidth + BORDER)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2,
                    firstChild.transform.position.y,
                    firstChild.transform.position.z);
            }
        }
    }
}