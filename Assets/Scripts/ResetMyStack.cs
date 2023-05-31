using System.Collections;
using UnityEngine;

public class ResetMyStack : MonoBehaviour
{
    /// <summary>
    /// Called when the reset button is clicked.
    /// Destroys the currently focused stack, then rebuilds it using the Jenga game manager.
    /// </summary>
    public void ButtonClicked()
    {
        // Get the currently focussed stack
        GameObject focussedStack = GameObject.Find("Stack " + (CameraController.currentStackIndex + 1));
        // Destroy it
        Destroy(focussedStack);
        // Rebuild it
        JengaGameManager gameManager = GameObject.Find("GameManager").GetComponent<JengaGameManager>();
        gameManager.PlaceStack(CameraController.currentStackIndex);

    }
}
