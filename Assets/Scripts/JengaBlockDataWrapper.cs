using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JengaBlockDataWrapper
{
    public List<JengaBlockData> blocks;

    /// <summary>
    /// Initializes a new instance of the JengaBlockDataWrapper class with the specified list of blocks.
    /// </summary>
    /// <param name="blocks">The list of Jenga block data.</param>s
    public JengaBlockDataWrapper(List<JengaBlockData> blocks)
    {
        this.blocks = blocks;
    }
}
