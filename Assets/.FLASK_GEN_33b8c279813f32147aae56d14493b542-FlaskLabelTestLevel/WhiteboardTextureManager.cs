#define FLASK_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WhiteboardTextureManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D _whiteboardTex;
    private Texture2D _pdfTex;
    private PdfNavigator _navigator;
    private Whiteboard _whiteboard;
    public string PDF_Location;
    public int TextureX;
    public int TextureY;
    bool whiteboardTexToggle = true;
    private Renderer r;
    void ToggleTexture()
    {
        if (whiteboardTexToggle)
        {
            whiteboardTexToggle = !whiteboardTexToggle;
            r.material.mainTexture = _pdfTex;
        }
        else
        {
            whiteboardTexToggle = !whiteboardTexToggle;
            r.material.mainTexture = _whiteboardTex;
        }
    }

    void Start()
    {
        r = GetComponent<Renderer>();
        _navigator = new PdfNavigator();
        _navigator.Open(PDF_Location, TextureX, TextureY);
        _whiteboard = GetComponent<Whiteboard>();
        _pdfTex = _navigator.tex;
        _whiteboardTex = _whiteboard.tex;
        r.material.mainTexture = _whiteboardTex;
    }

    // Update is called once per frame
    void Update()
    {
        //TEMPORARY NAVIGATION CODE
        if (Input.GetKeyDown(KeyCode.Q))
            ToggleTexture();
    }
}