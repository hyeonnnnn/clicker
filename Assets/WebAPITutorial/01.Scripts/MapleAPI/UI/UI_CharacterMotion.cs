using UnityEngine;
using TMPro;
using MapleAPI.UI;

public class UI_CharacterMotion : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _actionDropdown;
    [SerializeField] private TMP_Dropdown _emotionDropdown;
    [SerializeField] private UI_CharacterOutput _characterOutput;

    // Action (액션)
    private readonly string[] _actionNames = {
        "기본 서기1", "기본 서기2", "걷기1", "걷기2", "엎드리기", "날기",
        "점프", "앉기", "사다리", "로프", "힐", "경계"
    };
    private readonly string[] _actionCodes = {
        "A00", "A01", "A02", "A03", "A04", "A05",
        "A06", "A07", "A08", "A09", "A10", "A11"
    };

    // Emotion (감정표현)
    private readonly string[] _emotionNames = {
        "기본 표정", "윙크", "웃음", "울음", "화남", "당황",
        "깜빡임", "불꽃", "인사", "환호", "뽀뽀"
    };
    private readonly string[] _emotionCodes = {
        "E00", "E01", "E02", "E03", "E04", "E05",
        "E06", "E07", "E08", "E09", "E10"
    };

    private void Start()
    {
        InitializeDropdowns();
        AddListeners();
    }

    private void InitializeDropdowns()
    {
        if (_actionDropdown != null)
        {
            _actionDropdown.ClearOptions();
            _actionDropdown.AddOptions(new System.Collections.Generic.List<string>(_actionNames));
        }

        if (_emotionDropdown != null)
        {
            _emotionDropdown.ClearOptions();
            _emotionDropdown.AddOptions(new System.Collections.Generic.List<string>(_emotionNames));
        }
    }

    private void AddListeners()
    {
        if (_actionDropdown != null)
            _actionDropdown.onValueChanged.AddListener(_ => OnMotionChanged());

        if (_emotionDropdown != null)
            _emotionDropdown.onValueChanged.AddListener(_ => OnMotionChanged());
    }

    private void OnMotionChanged()
    {
        if (_characterOutput == null) return;

        string action = GetSelectedCode(_actionDropdown, _actionCodes);
        string emotion = GetSelectedCode(_emotionDropdown, _emotionCodes);

        _characterOutput.UpdateCharacterImage(action, emotion);
    }

    private string GetSelectedCode(TMP_Dropdown dropdown, string[] codes)
    {
        if (dropdown == null || codes == null || codes.Length == 0)
            return null;

        int index = dropdown.value;
        if (index >= 0 && index < codes.Length)
            return codes[index];

        return codes[0];
    }

    private void OnDestroy()
    {
        if (_actionDropdown != null)
            _actionDropdown.onValueChanged.RemoveAllListeners();

        if (_emotionDropdown != null)
            _emotionDropdown.onValueChanged.RemoveAllListeners();
    }
}
