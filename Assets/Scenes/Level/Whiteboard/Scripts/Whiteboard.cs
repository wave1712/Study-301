using System.Linq;
using UnityEngine;
[Elixir]
public class Whiteboard : MonoBehaviour
{
    [SerializeField]private bool isRunningUnity = false;
    public Texture2D tex;
    public Texture2D placeholder;
    public Material placeholderTest;

    public Vector2 textureSize = new Vector2(2048, 2048);
    public Color Colour;

    void Start()
    {
        var r = GetComponent<Renderer>();
        var m = new Material(placeholderTest);
        if(isRunningUnity){
            Debug.Log("Created a new material:", m);
            Debug.Log("Renderer is:", r);
        }else{
            MelonLoader.MelonLogger.Msg(System.ConsoleColor.Green, "Created a new material:", m);
            MelonLoader.MelonLogger.Msg(System.ConsoleColor.Green, "Renderer is:", r);
        }

        if(r != null)
        {
            
            
            // // Color32[] cols = new Color32[tex.width * tex.height];
            // // cols = tex.GetPixels32();
            tex = new Texture2D((int)textureSize.x, (int)textureSize.y);
            var color = Enumerable.Repeat(Color.white,(int)(textureSize.x * textureSize.y)).ToArray();
            tex.SetPixels(color);
            if(isRunningUnity)
                Debug.Log(string.Format("Creating a texture ({0},{1}). Its colour is: {2}, {3} {4}", tex.width,tex.height, tex.GetPixel(0,0).r,tex.GetPixel(0,0).g,tex.GetPixel(0,0).b));
            else
                MelonLoader.MelonLogger.Msg(System.ConsoleColor.Green, string.Format("Creating a texture ({0},{1}). Its colour is: {2}, {3} {4}", tex.width,tex.height, tex.GetPixel(0,0).r,tex.GetPixel(0,0).g,tex.GetPixel(0,0).b));
            
            // var cornerChange = Enumerable.Repeat(Color.green, (int)((tex.width/8) * (tex.height/8))).ToArray();
            // Debug.Log("Testing in Whiteboard.cs");

            // //tex.SetPixels(whiteboardColour);
            
            // tex.SetPixels(0,0,(int)tex.width/8, (int)tex.height/8, cornerChange);
            // tex.SetPixels(0,(int)(tex.height - tex.height/8),(int)tex.width/8, (int)tex.height/8, cornerChange);
            // tex.SetPixels((int)(tex.width - tex.width/8),0,(int)tex.width/8, (int)tex.height/8, cornerChange);
            //tex.SetPixels((int)(tex.width - tex.width/8),(int)(tex.height - tex.height/8),(int)tex.width/8, (int)tex.height/8, cornerChange);
            tex.Apply();
            r.material.SetTexture("_BaseMap", tex); 
            if(isRunningUnity)
                Debug.Log("Set Texture:"+tex.ToString());
            else
                MelonLoader.MelonLogger.Msg(System.ConsoleColor.Green, "Set Texture:"+tex.ToString());
            
            //tex.Apply();
            //r.material.SetTexture("_BaseMap", tex);
        }
        else{
            Debug.Log("Something has gone wrong");
        }

    }

}
