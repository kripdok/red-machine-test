using UnityEngine;

public class MaximumDragCameraManager : MonoBehaviour
{
    [SerializeField] private float maxDragDistance = 5;

    private void Start()
    {
        CameraMovementControl.Instance.ChangeMaxDragDistance(maxDragDistance);
    }

    public void ResetCamera()
    {
        CameraMovementControl.Instance.ReturnCameraPosition();
    }
}
