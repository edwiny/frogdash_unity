using System;
using UnityEngine;

public class ScreenBorders
{
    public Vector3 bottomLeft { get; private set; }
    public Vector3 topLeft { get; private set; }
    public Vector3 bottomRight { get; private set; }
    public Vector3 topRight { get; private set; }
    public float width { get; private set; }
    public float height { get; private set; }

    public ScreenBorders()
    {
        CalculateScreenBorders();
    }

    public void CalculateScreenBorders()
    {
        // Get the camera used to render the scene
        Camera mainCamera = Camera.main;

        // Get the distance between the camera and the near clipping plane
        float cameraDistance = mainCamera.nearClipPlane;

        // Calculate the coordinates of the screen borders
        bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, cameraDistance));
        topLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, cameraDistance));
        topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraDistance));
        bottomRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, cameraDistance));

        width = bottomRight.x - bottomLeft.x;
        height = topLeft.y - bottomLeft.y;
    }

    public bool IsOffScreen(Vector2 position, float threshold)
    {
        if (position.x > (bottomLeft.x - threshold) && position.x < (bottomRight.x + threshold) &&
            position.y > (bottomLeft.y - threshold) && position.y < (topLeft.y + threshold)) {
            return false;
        }
        return true;
    }
}