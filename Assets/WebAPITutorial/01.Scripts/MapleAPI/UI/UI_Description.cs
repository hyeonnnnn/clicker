using TMPro;
using UnityEngine;

public class UI_Description : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionDisplay;
    [SerializeField] private string _defaultDescription = "닉네임을 입력해주세요.";
    [SerializeField] private string _successDescription = "캐릭터를 불러왔습니다.";
    [SerializeField] private string _failDescription = "캐릭터가 존재하지 않습니다.";

    private void Start()
    {
        ShowDefault();
    }

    public void ShowDefault() => SetDescription(_defaultDescription);
    public void ShowSuccess() => SetDescription(_successDescription);
    public void ShowFail() => SetDescription(_failDescription);

    private void SetDescription(string text)
    {
        if (_descriptionDisplay != null)
            _descriptionDisplay.text = text;
    }
}
