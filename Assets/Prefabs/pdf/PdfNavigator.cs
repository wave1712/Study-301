using Apitron.PDF.Rasterizer;
using Apitron.PDF.Rasterizer.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class PdfNavigator : MonoBehaviour
{

    static RenderingSettings renderingSettings = new RenderingSettings();
    const int MAX_RES = 1920;

    string path;
    Document doc;

    int currentPage = 0;

    public void Open(string p)
	{
        path = p;

        FileStream fs = new FileStream(path, FileMode.Open);
        doc = new Document(fs, EngineSettings.GlobalSettings);

        GoToPage(currentPage);
	}

    void NextPage() { GoToPage(currentPage + 1); }
    void LastPage() { GoToPage(currentPage - 1); }

    void GoToPage(int p)
	{
        //Stop multiple changes at the same time
        if (changeTextureSemaphore) return;
        changeTextureSemaphore = true;

        p = Mathf.Clamp(p, 0, doc.Pages.Count);
        currentPage = p;

        transform.GetChild(0).GetComponent<TextMeshPro>().text = $"{p+1}/{doc.Pages.Count}";

        Page page = doc.Pages[p];

        StartCoroutine(nameof(ChangeTexture), page);
    }

    bool changeTextureSemaphore = false;
    const int MAX_TEXTURE_FILL_PER_FRAME = 100000;
    IEnumerator ChangeTexture(Page page)
	{
        //Calculate resolution
        Vector2 fRes = (MAX_RES * new Vector2((float)page.Width, (float)page.Height) / Mathf.Max((float)page.Width, (float)page.Height));
        Vector2Int res = new Vector2Int((int)fRes.x, (int)fRes.y);

        //Open a new thread to rasterize the pdf
        int[] arr = null;
        Thread t = new Thread(() => { arr = page.RenderAsInts(res.x, res.y, renderingSettings); });
        t.Start();
        
        //wait until the thread finishes
        while (t.ThreadState == ThreadState.Running || t.ThreadState == ThreadState.WaitSleepJoin)
		{
            yield return new WaitForEndOfFrame();
		}

        if (t.ThreadState != ThreadState.Stopped)
        {
            Debug.LogError($"Thread for Changing pdf Page stopped early! Thread state {t.ThreadState}");
            changeTextureSemaphore = false;
            yield break;
        }

        //Make a new texture and fill it based on the rasterized pdf
        Texture2D tex = new Texture2D(1920, 1920);
        Vector2Int pageCornerCoords = (Vector2Int.one*1920 - res)/2;

        for (int i = 0; i < arr.Length; i++)
        {
            tex.SetPixel(pageCornerCoords.x + (i % res.x), pageCornerCoords.y + (res.y - (i / res.x)), new Color(
                        1 - ((arr[i] << 8) >> 24), //R
                        1 - ((arr[i] << 16) >> 24), //G
                        1 - ((arr[i] << 24) >> 24) //B
                        ));
            if ((i+1)%MAX_TEXTURE_FILL_PER_FRAME == 0) yield return new WaitForEndOfFrame();
        }

        tex.Apply();

        GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", tex);

        //Open the semaphores again
        changeTextureSemaphore = false;
    }

	private void Update()
	{
        //TEMPORARY NAVIGATION CODE
        if (Input.GetKeyDown(KeyCode.D)) NextPage();
        if (Input.GetKeyDown(KeyCode.A)) LastPage();
    }
}