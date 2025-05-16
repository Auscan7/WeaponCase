using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public RectTransform crosshairUI;

    public float rotationAngle = -15f; // Left lean
    public float rotationSpeed = 10f;  // Higher = snappier

    public float scaleSpeed = 10f; // Higher = snappier
    public Vector3 scaleAmount = new Vector3(1.2f, 1.2f, 1.2f); // Scale up amount

    private Quaternion defaultRotation;
    private Quaternion targetRotation;

    private Vector3 defaultScale;
    private Vector3 targetScale;

    [SerializeField]private bool rotateOnClick = false;
    [SerializeField]private bool scaleOnClick = false;

    void Start()
    {
        defaultRotation = Quaternion.identity;
        targetRotation = defaultRotation;

        defaultScale = crosshairUI.localScale;
        targetScale = defaultScale;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        if (crosshairUI != null)
        {
            crosshairUI.position = mousePos;
            Cursor.visible = false;

            if (rotateOnClick)
            {
                RotateOnClick();
            }
            if (scaleOnClick)
            {
                ScaleOnClick();
            }

        }
    }

    private void RotateOnClick()
    {
        if (crosshairUI != null)
        {
            // Handle rotation on click
            if (Input.GetMouseButton(0)) // Left click held
            {
                targetRotation = Quaternion.Euler(0f, 0f, rotationAngle);
            }
            else
            {
                targetRotation = defaultRotation;
            }

            // Smooth rotation
            crosshairUI.rotation = Quaternion.Lerp(crosshairUI.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void ScaleOnClick()
    {
        if (crosshairUI != null)
        {
            if (Input.GetMouseButton(0))
            {
                targetScale = scaleAmount; // Scale up
            }
            else
            {
                targetScale = defaultScale; // Reset to default scale
            }

            // Smooth scaling
            crosshairUI.localScale = Vector3.Lerp(crosshairUI.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
    }
}
