using Player;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Singleton;

namespace Camera
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private float dragSpeed;
        [SerializeField] private float maxDragDistance;

        public UnityEngine.Camera MainCamera => mainCamera;

        private bool _isDrag;
        private Vector3 _startDragPosition;
        private Vector3 _lastMousePosition;

        public void Update()
        {
            if (Input.GetMouseButton(0) && PlayerController.PlayerState == PlayerState.None)
            {
                Vector3 currentMouseScreenPosition = Input.mousePosition;

                if (!_isDrag)
                {
                    _isDrag = true;
                    _startDragPosition = mainCamera.ScreenToWorldPoint(currentMouseScreenPosition);
                    _startDragPosition.z = 0f;
                    _lastMousePosition = currentMouseScreenPosition;
                }
                else
                {
                    if (currentMouseScreenPosition != _lastMousePosition)
                    {
                        Vector3 currentMouseWorldPosition = mainCamera.ScreenToWorldPoint(currentMouseScreenPosition);
                        currentMouseWorldPosition.z = 0f;

                        Vector3 dragDirection = currentMouseWorldPosition - _startDragPosition;

                        var speed = Vector3.Distance(_lastMousePosition, currentMouseWorldPosition);

                        Vector3 newCameraPosition = mainCamera.transform.position - dragDirection * dragSpeed * Time.deltaTime;

                        if (Vector2.Distance(Vector3.zero, newCameraPosition) <= maxDragDistance)
                        {
                            mainCamera.transform.position = newCameraPosition;
                        }

                        _startDragPosition = currentMouseWorldPosition;
                    }

                    _lastMousePosition = currentMouseScreenPosition;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDrag = false;
            }
        }
    }
}
