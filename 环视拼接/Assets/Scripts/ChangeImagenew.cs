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
    private RenderTexture texAll;
    private List<RenderTexture> destination = new List<RenderTexture>();
    public Material[] mt;
    public UniversalMediaPlayer[] umps;
    public GameObject[] iconshow;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            destination.Add(new RenderTexture(1280, 738, 0));
        }
        Texture2D texture2D = new Texture2D(1310*2, 1310, TextureFormat.RGBA32, false);
        byte[] vs1 = texture2D.EncodeToPNG();


        string path1 = @"D:\全景拼接大图.png";
        FileStream fileStream1 = new FileStream(path1, FileMode.Create, FileAccess.Write);
        fileStream1.Write(vs1, 0, vs1.Length);
        fileStream1.Dispose();
        fileStream1.Close();
    }

    // Update is called once per frame
    void Update()
    {
        var m1 = RenderImage(0);
        var m2 = RenderImage(1);
        var m3 = RenderImage(2);
        var m4 = RenderImage(3);
        OnApplicationQuit();
    }

    private RenderTexture RenderImage(int key)
    {

        //Shader sd = AssetDatabase.LoadAssetAtPath<Shader>("Assets/shaders/ColorToGradient.shader");
        var tex2 = objs[key].transform.GetComponent<RawImage>().texture;
        mt[key].SetTexture("_MainTex", tex2);
        Graphics.Blit(tex2, destination[key], mt[key]);
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
