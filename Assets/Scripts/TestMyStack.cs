using System.Collections;
using UnityEngine;

public class TestMyStack : MonoBehaviour
{
    /// The tag of the blocks to be destroyed.
    public string targetTag = "glass";

    /// <summary>
    /// Destroys the given list of glass blocks.
    /// </summary>
    /// <param name="blocksToDestroy">The ArrayList of glass blocks to destroy.</param>
    public void DestroyBlocks(ArrayList blocksToDestroy)
    {
        // Destroy each glass block
        foreach (GameObject block in blocksToDestroy)
        {
            Destroy(block.gameObject);
        }
    }

    /// <summary>
    /// Finds and returns an ArrayList of targetTag blocks in the currently focused stack.
    /// </summary>
    /// <returns>An ArrayList of targetTag blocks in the currently focused stack.</returns>
    private ArrayList FindGlassBloksInCurrentStack()
    {
        // glass objects to destroy
        ArrayList objectsToDestroy = new ArrayList();

        // Find all objects with the target tag
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        // get the parent (currently focussed stack)
        GameObject stack = GameObject.Find("Stack " + (CameraController.currentStackIndex + 1));

        // Iterate through the tagged objects
        foreach (GameObject taggedObject in taggedObjects)
        {
            // Check if the tagged object is the focussed stack
            if (taggedObject.transform.parent == stack.transform)
            {
                // The tagged object belongs to the same parent, do something with it
                // Debug.Log("Found object with tag " + targetTag + " and same parent: " + taggedObject.name);
                objectsToDestroy.Add(taggedObject);
            }
        }

        return objectsToDestroy;
    }

    /// <summary>
    /// Called when a button is clicked. Finds the blocks with tag targetTag in the currently focused stack and destroys them.
    /// </summary>
    public void ButtonClicked()
    {
        ArrayList objectsToDestroy = FindGlassBloksInCurrentStack();
        DestroyBlocks(objectsToDestroy);
    }
}
