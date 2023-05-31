using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    public GameObject toolTipObject;
    public TextMeshProUGUI toolTipText;

    /// <summary>
    /// Initializes the ToolTipManager by finding the tool tip object and setting up the text component.
    /// </summary>
    void Start()
    {
        toolTipObject = GameObject.FindGameObjectWithTag("toolTipObject");
        toolTipText = toolTipObject.GetComponentInChildren<TextMeshProUGUI>();
        HideTooltip(); // Hide the tooltip at the begining
    }

    /// <summary>
    /// Displays the tool tip with the given content.
    /// </summary>
    /// <param name="content">The content to be displayed in the tool tip.</param>
    public void ShowTooltip(string content)
    {
        toolTipObject.SetActive(true);
        //toolTipObject.transform.position = position;
        toolTipText.text = content;
    }

    /// <summary>
    /// Hides the tool tip.
    /// </summary>
    public void HideTooltip()
    {
        toolTipObject.SetActive(false);
    }
}
