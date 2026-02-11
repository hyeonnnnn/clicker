using UnityEngine;
using UnityEngine.UI;
using MapleAPI.UI;

public class UI_FrameSelectToggleGroup : MonoBehaviour
{
    [SerializeField] private Toggle _toggleNone;      // 기본 (미적용)
    [SerializeField] private Toggle _toggleRare;      // 레어
    [SerializeField] private Toggle _toggleEpic;      // 에픽
    [SerializeField] private Toggle _toggleUnique;    // 유니크
    [SerializeField] private Toggle _toggleLegendary; // 레전더리
    [SerializeField] private UI_CharacterOutput _characterOutput;

    private void Start()
    {
        AddListeners();

        // 기본 프레임 선택
        if (_toggleNone != null)
            _toggleNone.isOn = true;
    }

    private void AddListeners()
    {
        if (_toggleNone != null)
            _toggleNone.onValueChanged.AddListener(isOn => { if (isOn) OnFrameChanged(FrameRarity.None); });

        if (_toggleRare != null)
            _toggleRare.onValueChanged.AddListener(isOn => { if (isOn) OnFrameChanged(FrameRarity.Rare); });

        if (_toggleEpic != null)
            _toggleEpic.onValueChanged.AddListener(isOn => { if (isOn) OnFrameChanged(FrameRarity.Epic); });

        if (_toggleUnique != null)
            _toggleUnique.onValueChanged.AddListener(isOn => { if (isOn) OnFrameChanged(FrameRarity.Unique); });

        if (_toggleLegendary != null)
            _toggleLegendary.onValueChanged.AddListener(isOn => { if (isOn) OnFrameChanged(FrameRarity.Legendary); });
    }

    private void OnFrameChanged(FrameRarity rarity)
    {
        if (_characterOutput != null)
            _characterOutput.SetFrame(rarity);
    }

    private void OnDestroy()
    {
        if (_toggleNone != null)
            _toggleNone.onValueChanged.RemoveAllListeners();

        if (_toggleRare != null)
            _toggleRare.onValueChanged.RemoveAllListeners();

        if (_toggleEpic != null)
            _toggleEpic.onValueChanged.RemoveAllListeners();

        if (_toggleUnique != null)
            _toggleUnique.onValueChanged.RemoveAllListeners();

        if (_toggleLegendary != null)
            _toggleLegendary.onValueChanged.RemoveAllListeners();
    }
}
