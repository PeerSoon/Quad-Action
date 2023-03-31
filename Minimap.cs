using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform mainCamera;
    public Camera minimapCamera;
    public float minimapHeight = 50.0f;
    public GameManager GameManager;

    private void Update()
    {
        UpdateMinimapCameraPosition();
    }

    private void UpdateMinimapCameraPosition()
    {
        Vector3 mainCameraPosition = mainCamera.position;
        minimapCamera.transform.position = new Vector3(mainCameraPosition.x, minimapHeight, mainCameraPosition.z);
    }
}