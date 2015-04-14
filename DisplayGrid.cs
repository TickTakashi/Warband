using UnityEngine;
using System.Collections;

public class DisplayGrid : MonoBehaviour {

  public bool showMain = true;

  public int gridSizeX;
  public int gridSizeY;
  public int gridSizeZ;

  public float step;

  public float startX;
  public float startY;
  public float startZ;

  private Material lineMaterial;

  public Color mainColor = new Color(0f, 1f, 0f, 1f);

  void Start() { }
  
  void CreateLineMaterial() {

    if (!lineMaterial) {
      lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
          "SubShader { Pass { " +
          "    Blend SrcAlpha OneMinusSrcAlpha " +
          "    ZWrite Off Cull Off Fog { Mode Off } " +
          "    BindChannels {" +
          "      Bind \"vertex\", vertex Bind \"color\", color }" +
          "} } }");
      lineMaterial.hideFlags = HideFlags.HideAndDontSave;
      lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
    }
  }

  void OnPostRender() {
    CreateLineMaterial();
    // set the current material
    lineMaterial.SetPass(0);

    GL.Begin(GL.LINES);

    if (showMain) {
      GL.Color(mainColor);

      //Layers
      for (float j = startY; j <= gridSizeY + startY; j += step) {
        //X axis lines
        for (float i = 0; i <= gridSizeZ; i += step) {
          GL.Vertex3(startX, j, startZ + i);
          GL.Vertex3(startX + gridSizeX, j, startZ + i);
        }

        //Z axis lines
        for (float i = 0; i <= gridSizeX; i += step) {
          GL.Vertex3(startX + i, j, startZ);
          GL.Vertex3(startX + i, j, startZ + gridSizeZ);
        }
      }

      //Y axis lines
      for (float i = 0; i <= gridSizeZ; i += step) {
        for (float k = 0; k <= gridSizeX; k += step) {
          GL.Vertex3(startX + k, startY, startZ + i);
          GL.Vertex3(startX + k, startY + gridSizeY, startZ + i);
        }
      }
    }

    GL.End();
  }
}
 
