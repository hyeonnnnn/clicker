using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class WebGetImageTest : MonoBehaviour
{
    public RawImage MyImage;

    private async void Start()
    {
        StartCoroutine(GetTexture());

        MyImage.texture = await GetWebTexture("https://biz.chosun.com/resizer/v2/KIGXZI4TNVDWBIQC3Q7RFLN6JU.png?auth=65113d026a545411c35cf215851720560ea82225a1c1f373383e40f52aaa1f40&width=616");
    }

    private async UniTask<Texture> GetWebTexture(string url)
    {
        var texture = ((DownloadHandlerTexture)(await UnityWebRequestTexture.GetTexture(url).SendWebRequest()).downloadHandler).texture;
        return texture;
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.bbc.com/korean/articles/cn42dkdq1lwo");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}