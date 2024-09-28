using Camera;
using Player.ActionHandlers;
using System.Collections;
using UnityEngine;

public class CameraMovementControl : MonoBehaviour
{
    [SerializeField] private float dragSpeed;
    [SerializeField] private float maxDragDistance;

    private Vector3 _cameraDefoltPosition;

    private ClickHandler _clickHandler => ClickHandler.Instance;

    private UnityEngine.Camera _mainCamera => CameraHolder.Instance.MainCamera;

    private void Awake()
    {
        _cameraDefoltPosition = _mainCamera.transform.position;
    }

    private void OnEnable()
    {
        _clickHandler.ChangeDragPosition += TryChangePosition;
    }

    private void OnDisable()
    {
        _clickHandler.ChangeDragPosition -= TryChangePosition;
    }

    private void TryChangePosition(Vector3 inputPosition)
    {
        Vector3 newCameraPosition = _mainCamera.transform.position - inputPosition * dragSpeed * Time.deltaTime;
        newCameraPosition.z = _mainCamera.transform.position.z;

        if (Vector2.Distance(Vector3.zero, newCameraPosition) <= maxDragDistance)
        {
            _mainCamera.transform.position = newCameraPosition;
        }
    }

    public IEnumerator ReturnCameraToDefaultPosition()
    {
        while (Vector2.Distance(_cameraDefoltPosition, _mainCamera.transform.position) <= 0)
        {
            Vector3 newCameraPosition = _mainCamera.transform.position - _cameraDefoltPosition * dragSpeed * Time.deltaTime;
            newCameraPosition.z = _mainCamera.transform.position.z;

            _mainCamera.transform.position = newCameraPosition;
            yield return null;
        }
    }

}
