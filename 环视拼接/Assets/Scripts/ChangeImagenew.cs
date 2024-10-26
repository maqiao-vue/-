using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Xml.Linq;
using TMPro;
using UMP;

public class ChangeImagenew : MonoBehaviour
{
    public List<GameObject> objs = new List<GameObject>();
    public List<GameObject> outObjs = new List<GameObject>();
    public ComputeShader shader;
    public ComputeShader shader1;
    private int kernel;
    private List<RenderTexture> tex = new List<RenderTexture>();
    private RenderTexture texAll;
    private List<RenderTexture> destination = new List<RenderTexture>();
    public Material mt;
    public UniversalMediaPlayer[] umps;
    public GameObject[] iconshow;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            destination.Add(new RenderTexture(1310 * 2, 1310, 0));
        }

        kernel = shader.FindKernel("CSMain");
        for (int i = 0; i < 2; i++)
        {
            tex.Add(new RenderTexture(1310 * 2, 1310, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB));
            tex[i].enableRandomWrite = true;
            tex[i].Create();

        }

        for (int i = 0; i < outObjs.Count; i++)
        {
            texAll = new RenderTexture(1310 * 2, 1310, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            texAll.enableRandomWrite = true;
            texAll.Create();
            outObjs[i].transform.GetComponent<Renderer>().material.mainTexture = texAll;
        }

    }

    // Update is called once per frame
    void Update()
    {
        int num = 0;
        for (int i = 0; i < objs.Count; i = i + 2)
        {
            var obj = objs[i].transform.GetComponent<RawImage>().texture;
            var obj1 = objs[i + 1].transform.GetComponent<RawImage>().texture;
            if (obj != null && obj1 != null)
            {
                shader.SetTexture(kernel, "inputTexture", obj);
                shader.SetTexture(kernel, "characterTex", obj1);
                shader.SetTexture(kernel, "outputTexture", tex[num]);
                shader.Dispatch(kernel, 1310 * 2, 1310, 1);
                StateStrAndColor(iconshow[i].transform, "在线", new Color32(32, 127, 32, 255));
                StateStrAndColor(iconshow[i + 1].transform, "在线", new Color32(32, 127, 32, 255));
            }
            else
            {
                if (obj == null)
                {
                    umps[i].Play();
                    StateStrAndColor(iconshow[i].transform, "离线", new Color32(96, 96, 96, 255));
                }

                if (obj1 == null)
                {
                    umps[i + 1].Play();
                    StateStrAndColor(iconshow[i + 1].transform, "离线", new Color32(96, 96, 96, 255));
                }

            }


            num++;
        }

        var m1 = RenderImage(tex[0], 0);
        var m2 = RenderImage(tex[1], 1);

        shader1.SetTexture(kernel, "inputTexture", m1);
        shader1.SetTexture(kernel, "characterTex", m2);
        shader1.SetTexture(kernel, "outputTexture", texAll);
        shader1.Dispatch(kernel, 1310 * 2, 1310, 1);
        OnApplicationQuit();
    }

    private RenderTexture RenderImage(RenderTexture tex2, int key)
    {

        //Shader sd = AssetDatabase.LoadAssetAtPath<Shader>("Assets/shaders/ColorToGradient.shader");

        mt.SetTexture("_MainTex", tex2);
        Graphics.Blit(tex2, destination[key], mt);
        return destination[key];
    }

    /// 运行模式下Texture转换成Texture2D
    private Texture2D TextureToTexture2D(Texture texture)
    {
        var texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        var currentRT = RenderTexture.active;
        var renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }

    void OnApplicationQuit()
    {
        // 当应用程序即将关闭时释放内存
        Resources.UnloadUnusedAssets();
        System.GC.Collect(); // 强制进行垃圾回收
    }

    private void StateStrAndColor(Transform obj, string str, Color color)
    {
        obj.transform.Find("Text").GetComponent<Text>().text = str;
        obj.GetComponent<Image>().color = color;
    }

}
