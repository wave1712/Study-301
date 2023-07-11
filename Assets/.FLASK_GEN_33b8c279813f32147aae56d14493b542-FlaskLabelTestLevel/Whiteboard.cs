#define FLASK_ONLY
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Whiteboard : MonoBehaviour
{
    public Texture2D tex;
    public Vector2 textureSize = new Vector2(2048, 2048);
    public Color Colour;
    void Start()
    {
        var r = GetComponent<Renderer>();
        tex = new Texture2D((int)textureSize.x, (int)textureSize.y);
        var whiteboardColour = Enumerable.Repeat(Colour, (int)(textureSize.x * textureSize.y)).ToArray();
        tex.SetPixels(whiteboardColour);
        tex.Apply();
    }
}