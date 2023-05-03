using Apitron.PDF.Rasterizer;
using Apitron.PDF.Rasterizer.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PdfNavigator : MonoBehaviour
{

    static RenderingSettings renderingSettings = new RenderingSettings();
    const int MAX_RES = 1920;

    string path = "F:/.Downloads/jov-11-5-8.pdf";
    Document doc;

    Texture2D tex;

    int currentPage = 0;

    void Start()
    {
        tex = new Texture2D(1, 1);
        GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", tex);

        Open();
    }

    void Open()
	{
        FileStream fs = new FileStream(path, FileMode.Open);
        doc = new Document(fs, EngineSettings.GlobalSettings);

        GoToPage(currentPage);
	}

    void NextPage() { GoToPage(currentPage + 1); }
    void LastPage() { GoToPage(currentPage - 1); }

    void GoToPage(int p)
	{
        p = Mathf.Clamp(p, 0, doc.Pages.Count);
        currentPage = p;

        Page page = doc.Pages[p];

        Vector2 fRes = (MAX_RES * new Vector2((float)page.Width, (float)page.Height) / Mathf.Max((float)page.Width, (float)page.Height));
        Vector2Int res = new Vector2Int((int)fRes.x, (int)fRes.y);

        float one = Time.realtimeSinceStartup;
        int[] data = page.RenderAsInts(res.x, res.y, renderingSettings);
        string time = (Time.realtimeSinceStartup - one).ToString();
        one = Time.realtimeSinceStartup;
        PdfUnityBridgeUtils.WriteTextureFromIntArray(data, res, ref tex);
        time += ", " + (Time.realtimeSinceStartup - one).ToString();
        Debug.Log(time);
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.D)) NextPage();
        if (Input.GetKeyDown(KeyCode.A)) LastPage();
    }
}

public class PdfUnityBridgeUtils
{

    public static void WriteTextureFromIntArray(int[] arr, Vector2Int res, ref Texture2D tex)
    {
        tex.Reinitialize(res.x, res.y);

        for (int i = 0; i < arr.Length; i ++)
		{
            
            tex.SetPixel(i % res.x, res.y - (i / res.x), new Color(
                        1 - ((arr[i] << 8) >> 24), //R
                        1 - ((arr[i] << 16) >> 24), //G
                        1 - ((arr[i] << 24) >> 24) //B
                        ));
		}

        tex.Apply();
    }

}