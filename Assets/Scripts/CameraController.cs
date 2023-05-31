using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3[] stackPositions = { new Vector3(-10.6f, 9, -11), new Vector3(7.78f, 9.029063f, -13), new Vector3(24.87f, 8.2f, -13) }; // Array to hold the 3 stack positions
    public static int currentStackIndex = 0; // Index to keep track of the current focused stack
    public float transitionSpeed = 1f; // Speed of the camera transition
    private bool isRotating = false; // Flag to indicate if camera rotation is in progress
    public float rotationSpeed = 5f; // Speed of camera rotation
    private bool isTransitioning = false; // Flag to indicate if camera is currently transitioning

    /// <summary>
    /// Updates the camera's rotation and handles input for rotating the camera and switching between stacks.
    /// </summary>
    private void Update()
    {
        // Check for input to rotate the camera
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        // Rotate the camera if rotation is in progress
        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            transform.RotateAround(stackPositions[currentStackIndex], Vector3.up, mouseX);
        }

        // Only allow moving between stacks if no transitioning is happenning at the moment
        if (isTransitioning)
            return;

        // Check for left arrow key press
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Decrement the stack index
            currentStackIndex--;
            if (currentStackIndex < 0)
                currentStackIndex = stackPositions.Length - 1;

            // Move the camera to the new stack position with a smooth transition
            Vector3 desiredPosition = stackPositions[currentStackIndex];
            StartCoroutine(MoveCamera(desiredPosition));
        }

        // Check for right arrow key press
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Increment the stack index
            currentStackIndex++;
            if (currentStackIndex >= stackPositions.Length)
                currentStackIndex = 0;

            // Move the camera to the new stack position with a smooth transition
            Vector3 desiredPosition = stackPositions[currentStackIndex];
            StartCoroutine(MoveCamera(desiredPosition));
        }
    }

    /// <summary>
    /// Moves the camera towards the desired position smoothly over time.
    /// </summary>
    /// <param name="desiredPosition">The target position to move the camera to.</param>
    /// <returns>An IEnumerator used for coroutine execution.</returns>
    private System.Collections.IEnumerator MoveCamera(Vector3 desiredPosition)
    {
        isTransitioning = true;

        Vector3 velocity = Vector3.zero; // Initial velocity for SmoothDamp
        float smoothTime = 1f; // Control the smoothness and settling time

        while (Vector3.Distance(transform.position, desiredPosition) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            yield return null;
        }

        transform.position = desiredPosition;
        isTransitioning = false;
    }

    /// <summary>
    /// Moves the camera to the specified stack position.
    /// </summary>
    /// <param name="stackIndex">The index of the stack to move the camera to.</param>
    public void MoveCameraToStack(int stackIndex)
    {
        if (isTransitioning)
            return;

        currentStackIndex = stackIndex;
        StartCoroutine(MoveCamera(stackPositions[currentStackIndex]));
    }
}
