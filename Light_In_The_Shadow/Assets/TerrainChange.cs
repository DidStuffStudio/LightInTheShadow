using System;
using UnityEngine;
using System.Collections;
public class TerrainChange : MonoBehaviour
{
    private static Terrain terrain;
    public static int textureNumberFrom, textureNumberTo;
    public bool change;
    private void Start()
    {
        terrain = GetComponent<Terrain>();
    }

    private void Update()
    {
        if (!change) return;
        UpdateTerrainTexture();
        
       change = false;
    }

    public static void UpdateTerrainTexture()
    {
        
        //get current paint mask
        float[, ,] alphas = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);
        // make sure every grid on the terrain is modified
        for (int i = 0; i < terrain.terrainData.alphamapWidth; i++)
        {
            for (int j = 0; j < terrain.terrainData.alphamapHeight; j++)
            {
                //for each point of mask do:
                //paint all from old texture to new texture (saving already painted in new texture)
                alphas[i, j, textureNumberTo] = Mathf.Max(alphas[i, j, textureNumberFrom], alphas[i, j, textureNumberTo]);
                //set old texture mask to zero
                alphas[i, j, textureNumberFrom] = 0f;
            }
        }
        // apply the new alpha
        terrain.terrainData.SetAlphamaps(0, 0, alphas);
    }

    public IEnumerator SpawnInTreesAndGrass()
    {
        
        var value = 0.0f;
        while (value < 100.0f)
        {
            value++;
            terrain.detailObjectDistance = Map(value, 0, 100, 0, 200);
            terrain.detailObjectDensity = Map(value, 0, 100, 0, 0.05f);
            terrain.treeDistance = Map(value, 0, 100, 0, 200);
            yield return new WaitForSeconds(0.01f);
        }
       
    }
    
    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }

}
