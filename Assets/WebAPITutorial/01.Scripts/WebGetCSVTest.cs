using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class WebGetCSVTest : MonoBehaviour
{

    private async void Start()
    {
        string result = await GetWebText("https://raw.githubusercontent.com/mongilteacher/skku2_script_study/refs/heads/main/students.csv");

        List<Person> persons = new List<Person>();
        // 읽어온 CSV 파일을 파싱해서
        // Person도메인 클래스에 넣고 persons에 추가
        // List<Person> persons 순회하기

    }

    private async UniTask<string> GetWebText(string url)
    {
        var txt = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return txt;
    }
}

public struct Person
{
    public int Id;
    public string Name;
    public int Age;
}
