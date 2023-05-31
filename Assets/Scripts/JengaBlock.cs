using UnityEngine;
public class JengaBlock : MonoBehaviour
{
    public JengaBlockData blockData;
    public ToolTipManager toolTipManager;

    /// <summary>
    /// Called when the Jenga block is first created.
    /// Finds the ToolTipManager object in the scene.
    /// </summary>
    void Start()
    {
        toolTipManager = GameObject.FindGameObjectWithTag("ToolTipManager").GetComponent<ToolTipManager>(); ;
    }

    /// <summary>
    /// Called when the mouse cursor is over the Jenga block.
    /// Checks for right-click input and shows the block details.
    /// </summary>
    private void OnMouseOver()
    {
        // Check for right-click input
        if (Input.GetMouseButtonDown(1))
        {
            // Show details of the Jenga block
            ShowBlockDetails();
        }
    }

    /// <summary>
    /// Shows the details of the Jenga block in the tooltip object.
    /// </summary>
    private void ShowBlockDetails()
    {
        // Create the info string to display
        string content = blockData.grade + ": " + blockData.domain + "\n" +
                         blockData.cluster + "\n" +
                         blockData.standardid + ": " + blockData.standarddescription + "\n";

        // Display the info
        toolTipManager.ShowTooltip(content);
    }

    /// <summary>
    /// Called when the mouse is no longer over the collider of this object.
    /// Hides the tooltip.
    /// </summary>
    private void OnMouseExit()
    {
        // Hide the tooltip when the mouse is exited from the block
        toolTipManager.HideTooltip();
    }


}
