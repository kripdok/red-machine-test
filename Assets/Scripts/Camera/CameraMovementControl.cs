using Camera;
using Player.ActionHandlers;
using System.Collections;
using UnityEngine;
using Utils.Scenes;
using Utils.Singleton;

public class CameraMovementControl : DontDestroyMonoBehaviourSingleton<CameraMovementControl>
{
    [SerializeField] private float dragSpeed;

    private float maxDragDistance = 0;

    private Vector3 _cameraDefoltPosition;
    private Coroutine _coroutin;
    private ClickHandler _clickHandler;

    private void Start()
    {
        _clickHandler = ClickHandler.Instance;

        _clickHandler.ChangeDragPosition += TryChangePosition;
        ScenesChanger.SceneLoadedEvent += ReturnCameraPosition;
    }

    private void OnDestroy()
    {
        _clickHandler.ChangeDragPosition -= TryChangePosition;
        ScenesChanger.SceneLoadedEvent -= ReturnCameraPosition;
    }

    public void ChangeMaxDragDistance(float newDistance)
    {
        maxDragDistance = newDistance;
    }

    public void ReturnCameraPosition()
    {
        _coroutin = StartCoroutine(ReturnCameraToDefaultPosition());
    }

    private IEnumerator ReturnCameraToDefaultPosition()
    {
        while (Vector2.Distance(_cameraDefoltPosition, CameraHolder.Instance.MainCamera.transform.position) > 0.01f)
        {
            Vector3 newCameraPosition = Vector3.Lerp(CameraHolder.Instance.MainCamera.transform.position, _cameraDefoltPosition, dragSpeed * Time.deltaTime);
            newCameraPosition.z = CameraHolder.Instance.MainCamera.transform.position.z;
            CameraHolder.Instance.MainCamera.transform.position = newCameraPosition;
            yield return null;
        }

        CameraHolder.Instance.MainCamera.transform.position = new Vector3(_cameraDefoltPosition.x, _cameraDefoltPosition.y, CameraHolder.Instance.MainCamera.transform.position.z);
    }

    private void TryChangePosition(Vector3 inputPosition)
    {
        if(_coroutin!=null) 
            StopCoroutine(_coroutin);

        Vector3 newCameraPosition = CameraHolder.Instance.MainCamera.transform.position - inputPosition * dragSpeed * Time.deltaTime;
        newCameraPosition.z = CameraHolder.Instance.MainCamera.transform.position.z;

        if (Vector2.Distance(Vector3.zero, newCameraPosition) <= maxDragDistance)
        {
            CameraHolder.Instance.MainCamera.transform.position = newCameraPosition;
        }
    }
}
