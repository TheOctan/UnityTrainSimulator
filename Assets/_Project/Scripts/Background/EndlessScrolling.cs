using UnityEngine;

namespace OctanGames.Background
{
    public class EndlessScrolling : MonoBehaviour
    {
        private const float BORDER = 2f;

        [SerializeField] private Camera _viewCamera;
        [SerializeField] private GameObject[] _layers;

        private Vector2 _screenBounds;

        private void Start()
        {
            _screenBounds = _viewCamera.ScreenToWorldPoint(
                new Vector3(Screen.width, Screen.height, _viewCamera.transform.position.z));

            foreach (GameObject layer in _layers)
            {
                LoadChildObjects(layer);
            }
        }

        private void LateUpdate()
        {
            foreach (GameObject layer in _layers)
            {
                MoveChildObjects(layer);
            }
        }

        private void LoadChildObjects(GameObject parent)
        {
            var spriteRenderer = parent.GetComponent<SpriteRenderer>(); 
            float objectWidth = spriteRenderer.bounds.size.x;
            var childAmount = (int)Mathf.Ceil(_screenBounds.x * 2 / objectWidth);
            GameObject copyParent = Instantiate(parent);

            for (var i = 0; i <= childAmount; i++)
            {
                GameObject c = Instantiate(copyParent, parent.transform);
                Vector3 parentPosition = parent.transform.position;
                c.transform.position = new Vector3(objectWidth * i, parentPosition.y, parentPosition.z);
                c.name = parent.name + i;
            }
            Destroy(copyParent);
            Destroy(spriteRenderer);
        }

        private void MoveChildObjects(GameObject parent)
        {
            Transform[] childObjects = parent.GetComponentsInChildren<Transform>();
            if (childObjects.Length > 1)
            {
                GameObject firstChild = childObjects[1].gameObject;
                GameObject lastChild = childObjects[childObjects.Length - 1].gameObject;
                float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;

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
}