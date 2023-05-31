using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;

public class JengaGameManager : MonoBehaviour
{

    public List<JengaBlockData> jengaBlocks = new List<JengaBlockData>();
    public static List<JengaBlockData>[] blocksGradeSorted;
    public GameObject jengaBlockPrefab;
    public Material glassMaterial;
    public Material woodMaterial;
    public Material stoneMaterial;
    public Vector3[] stackPositions;

    string path = Application.streamingAssetsPath + "/data.json"; // The dummy data file for testing
    string apiUrl = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack"; // The API URL

    /// <summary>
    /// Fetches the Jenga block data from an API and places the Jenga blocks.
    /// </summary>
    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string responseJson = www.downloadHandler.text;

            // replace starting and ending [] to {} to easily parse into a json
            responseJson = responseJson.Replace("[", "{\"blocks\": [");
            responseJson = responseJson.Replace("]", "] }");

            // For testing: read json from the file
            // responseJson = File.ReadAllText(path);
            // Debug.Log(responseJson);

            // TestData testData = JsonUtility.FromJson<TestData>(responseJson);
            JengaBlockDataWrapper wrapper = JsonUtility.FromJson<JengaBlockDataWrapper>(responseJson);
            jengaBlocks = wrapper.blocks;

            // Debug.Log("Total Blocks: " + jengaBlocks.Count);
            PlaceJengaBlocks(); // Call the method to place the Jenga blocks after fetching the data
        }
        else
        {
            Debug.LogError("Failed to fetch Jenga block data from the API: " + www.error);

        }
    }

    /// <summary>
    /// Places the Jenga blocks in the specified positions.
    /// </summary>
    void PlaceJengaBlocks()
    {
        int stackSpacing = 15;
        Vector3 startPosition = new Vector3(0, 0.5f, 0);

        // Determine the positions for the stacks
        stackPositions = new Vector3[3];
        stackPositions[0] = startPosition;
        stackPositions[1] = startPosition + new Vector3(stackSpacing, 0f, 0f);
        stackPositions[2] = startPosition + new Vector3(stackSpacing * 2f, 0f, 0f);

        // CameraController.stackPositions = stackPositions;

        // Sort the blocks based on the specified criteria
        List<JengaBlockData> sortedBlocks = jengaBlocks.OrderBy(block =>
            block.grade).ThenBy(block =>
            block.domain).ThenBy(block =>
            block.cluster).ThenBy(block =>
            block.standardid).ToList();

        // Separate blocks into lists based on grade (for the 3 stacks)
        blocksGradeSorted = new List<JengaBlockData>[3];
        for (int i = 0; i < 3; i++)
        {
            blocksGradeSorted[i] = new List<JengaBlockData>();
        }

        foreach (var blockData in sortedBlocks)
        {
            int gradeIndex = blockData.grade[0] - '0' - 6; // Convert the grade 6 to 8 into an intger from 0 to 2
            // Debug.Log("gradeIndex: " + blockData.id + " " + blockData.grade + " " + (blockData.grade[0] - '0') + " " + gradeIndex);

            // Error handling: skip unexpected grades
            if ((gradeIndex < 0) || (gradeIndex > 2))
            {
                Debug.Log("Found an errorneous grade: " + gradeIndex + " block id: " + blockData.id);
                continue;
            }
            blocksGradeSorted[gradeIndex].Add(blockData);
        }

        // Place the blocks in the stack
        for (int stackIndex = 0; stackIndex < 3; stackIndex++)
        {
            PlaceStack(stackIndex);
        }
    }

    /// <summary>
    /// Places the Jenga blocks in a stack at the specified index.
    /// </summary>
    /// <param name="stackIndex">The index of the stack.</param>
    public void PlaceStack(int stackIndex)
    {
        float blockSpacing = 0.2f; // Spacing between the blocks within the same layer
        int blocksInLayer = 3; // Number of blocks in a single layer of the stack
        int currentBlockCount = 0; // Counter for blocks in the current layer
        int currentLayer = 0; // Current layer number

        // Create a stack object (blocks are added as children of this object)
        GameObject stackObject = new GameObject();
        stackObject.name = "Stack " + (stackIndex + 1);
        stackObject.transform.position = stackPositions[stackIndex];

        // Place the blocks in the stack
        Vector3 blockPosition = stackObject.transform.position;
        Quaternion blockRotation = stackObject.transform.rotation;

        for (int i = 0; i < blocksGradeSorted[stackIndex].Count; i++)
        {

            // Debug.Log("stack: " + stackIndex + " layer: " + currentLayer + "block count " + currentBlockCount + " blockPosition: " + blockPosition);

            JengaBlockData blockData = blocksGradeSorted[stackIndex][i];
            Material blockMaterial = null;
            string blockTag = null;

            // Instantiate the block object
            GameObject blockObject = Instantiate(jengaBlockPrefab, blockPosition, blockRotation, stackObject.transform);
            JengaBlock block = blockObject.GetComponent<JengaBlock>();
            block.blockData = blockData; // Assign the block data
            Renderer blockRenderer = blockObject.GetComponent<Renderer>();

            // Determine the block type based on the mastery level
            switch (blockData.mastery)
            {
                case 0: // Glass
                    blockMaterial = glassMaterial;
                    blockTag = "glass";
                    break;
                case 1: // Wood
                    blockMaterial = woodMaterial;
                    blockTag = "wood";
                    break;
                case 2: // Stone
                    blockMaterial = stoneMaterial;
                    blockTag = "stone";
                    break;
            }
            blockRenderer.material = blockMaterial;
            block.tag = blockTag;

            // Adjust the position for the next block
            currentBlockCount++;

            if (currentBlockCount >= blocksInLayer)
            {
                currentBlockCount = 0;
                currentLayer++;
            }

            // Every other layer should be y rotated 90 degrees and z should be 1
            if (currentLayer % 2 == 0) // Even layers
            {
                blockPosition = new Vector3(stackPositions[stackIndex].x, stackPositions[stackIndex].y + currentLayer, currentBlockCount + blockSpacing * currentBlockCount);
                blockRotation = Quaternion.identity;
            }
            else // Odd layers
            {
                blockPosition = new Vector3(stackPositions[stackIndex].x + (currentBlockCount - 1) + (blockSpacing * (currentBlockCount - 1)), stackPositions[stackIndex].y + currentLayer, 1 + blockSpacing);
                blockRotation = Quaternion.Euler(0f, 90f, 0f);
            }

        }

    }


}
