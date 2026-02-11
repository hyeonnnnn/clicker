using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace MapleAPI.UI
{
    public enum FrameRarity
    {
        None,       // 미적용
        Rare,       // 레어
        Epic,       // 에픽
        Unique,     // 유니크
        Legendary   // 레전더리
    }

    public class UI_CharacterOutput : MonoBehaviour
    {
        [Header("캐릭터 카드 프레임")]
        [SerializeField] private GameObject _characterCardFrame;

        [Header("캐릭터 정보 프레임")]
        [SerializeField] private GameObject _characterInfoFrame;

        [Header("캐릭터 이미지")]
        [SerializeField] private RawImage _characterImage;

        [Header("기본 정보")]
        [SerializeField] private TextMeshProUGUI _characterInfoText;  // "Lv. 0 닉네임 (월드)"

        [Header("능력치 표시")]
        [SerializeField] private Transform _statContainer;
        [SerializeField] private StatToggleController _statToggleController;

        private MapleCharacterData _currentData;
        private MapleAPIClient _apiClient;

        public MapleCharacterData CurrentData => _currentData;

        private void Awake()
        {
            _apiClient = new MapleAPIClient("");
        }

        private void Start()
        {
            DeactivateCard();
        }

        private void DeactivateCard()
        {
            if (_characterCardFrame != null)
                _characterCardFrame.SetActive(false);

            if (_characterImage != null)
                _characterImage.gameObject.SetActive(false);

            if (_characterInfoFrame != null)
                _characterInfoFrame.SetActive(false);
        }

        private void ActivateCard()
        {
            if (_characterCardFrame != null)
                _characterCardFrame.SetActive(true);

            if (_characterImage != null)
                _characterImage.gameObject.SetActive(true);

            if (_characterInfoFrame != null)
                _characterInfoFrame.SetActive(true);
        }

        /// <summary>
        /// 캐릭터 데이터 설정 및 UI 업데이트
        /// </summary>
        public void SetCharacterData(MapleCharacterData data)
        {
            _currentData = data;

            if (data == null)
            {
                return;
            }

            UpdateUI();
            LoadCharacterImage().Forget();
        }

        /// <summary>
        /// UI 전체 업데이트
        /// </summary>
        private void UpdateUI()
        {
            if (_currentData == null) return;

            // 기본 정보: "Lv. 0 닉네임 (월드)"
            SetTextSafe(_characterInfoText, $"Lv. {_currentData.Level} {_currentData.Name} ({_currentData.World})");

            // 능력치 업데이트
            if (_statToggleController != null)
                _statToggleController.UpdateStats(_currentData);
        }

        /// <summary>
        /// 캐릭터 이미지 로드
        /// </summary>
        private async UniTaskVoid LoadCharacterImage()
        {
            if (_characterImage == null || _currentData == null) return;

            string imageUrl = _currentData.ImageUrl;
            if (string.IsNullOrEmpty(imageUrl)) return;

            var texture = await _apiClient.GetCharacterImageAsync(imageUrl);
            if (texture != null && _characterImage != null)
            {
                _characterImage.texture = texture;
                ActivateCard();
            }
        }

        private void SetTextSafe(TextMeshProUGUI textComponent, string value)
        {
            if (textComponent != null)
                textComponent.text = value;
        }

        /// <summary>
        /// 카드 초기화
        /// </summary>
        public void Clear()
        {
            _currentData = null;

            SetTextSafe(_characterInfoText, "");

            if (_characterImage != null)
            {
                _characterImage.texture = null;
                _characterImage.gameObject.SetActive(false);
            }
        }
    }
}
