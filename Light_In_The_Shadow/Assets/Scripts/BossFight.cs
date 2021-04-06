using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{

    private Camera _cam;
    public float interactionDistance = 100.0f;
    public float torchStrength = 20.0f;
    private int bossLayerMask;
    public SkinnedMeshRenderer bossMeshRenderer;
    public int textureResolution = 1024;
    private Texture2D texture;
    public MeshCollider meshCollider;

    public int areaWhereDilationShouldBeCalculated = 100;
    public int kernelSize = 3;

    private Mesh _mesh;
    void Start()
    {
        texture = new Texture2D(textureResolution, textureResolution);
        Color[] colors = new Color[textureResolution * textureResolution];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.black;
        }

        texture.SetPixels(colors);
        texture.Apply();
        bossLayerMask = LayerMask.GetMask("BigBossMan");
        bossMeshRenderer.material.SetTexture("_MainTex", texture);
        _cam = Camera.main;
        //StartCoroutine(SlowUpdate());
    }

    // Update is called once per frame
    IEnumerator SlowUpdate()
    {
        while (true)
        {
            //meshCollider.sharedMesh = bossMeshRenderer.sharedMesh;
            
           // bossMeshRenderer.BakeMesh(_mesh);
            
            if (MasterManager.Instance.player.holdingTorch)
            {

                var torchRay = _cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                Debug.DrawRay(_cam.transform.position, _cam.transform.forward, Color.red);
                
                
                
                
                /*if (Physics.Raycast(torchRay, out var torchHit, interactionDistance, bossLayerMask))
                {
                    print("Hitting");

                    Vector2 pixelUV = torchHit.textureCoord;
                    pixelUV.x *= texture.width;
                    pixelUV.y *= texture.height;

                    texture.SetPixel((int) pixelUV.x, (int) pixelUV.y, Color.white);
                    texture.Apply();




                    


                    int xTextureCoordinate = (int)(torchHit.textureCoord.x * textureResolution);
                    int yTextureCoordinate = (int)(torchHit.textureCoord.y * textureResolution);
                    
                    texture.SetPixel(xTextureCoordinate, yTextureCoordinate, Color.white);*/
                    
                    /*
                    float sum = 0; // Kernel sum for this pixel
                    
                            //At the pixel hit
    
    
                            for (int y = kernelSize; y < texture.height - kernelSize; y++)
                            {
                                // Skip top and bottom edges
                                for (int x = kernelSize; x < texture.width - kernelSize; x++)
                                {
                                    // Skip left and right edges
                                    for (int ky = -kernelSize; ky <= kernelSize; ky++)
                                    {
                                        for (int kx = -kernelSize; kx <= kernelSize; kx++)
                                        {
                                            // Calculate the adjacent pixel for this kernel point
                                            int xCoordinate = xTextureCoordinate + kx;
                                            int yCoordinate = yTextureCoordinate + ky;
                                            // Multiply adjacent pixels based on the kernel values
                                            sum += texture.GetPixel(xCoordinate, yCoordinate).grayscale / 255;
                                        }
                                    }
    
                                    if (sum >= 1)
                                        texture.SetPixel(x, y, Color.white);
                                    else
                                        texture.SetPixel(x, y, Color.black);
    
                                }
                            }
                            */
    
    
                            texture.Apply();

                }
            }
            yield return new WaitForSeconds(0.1f);
           
        }
    }

