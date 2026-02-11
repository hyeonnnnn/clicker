using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace MapleAPI.UI
{
    public class UI_CharacterSearch : MonoBehaviour
    {
        [Header("API 설정")]
        [SerializeField] private string apiKey = "test_cd9ed653c405777e80b06476bb8ade5b4f00a84ae28a551eb20a03cddf75c6b3efe8d04e6d233bd35cf2fabdeb93fb0d";

        [Header("UI 컴포넌트")]
        [SerializeField] private TMP_InputField characterNameInput;
        [SerializeField] private Button searchButton;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private GameObject loadingIndicator;

        [Header("결과 전달")]
        [SerializeField] private CharacterCardUI characterCardUI;

        // 이벤트
        public event Action<MapleCharacterData> OnCharacterFound;
        public event Action<string> OnSearchError;

        private MapleAPIClient _apiClient;
        private bool _isSearching;

        private void Awake()
        {
            _apiClient = new MapleAPIClient(apiKey);
        }

        private void Start()
        {
            // 버튼 클릭 이벤트
            if (searchButton != null)
                searchButton.onClick.AddListener(() => SearchCharacter().Forget());

            // Enter 키 입력 이벤트
            if (characterNameInput != null)
                characterNameInput.onSubmit.AddListener(_ => SearchCharacter().Forget());

            // 초기 상태
            SetLoadingState(false);
            SetStatus("");
        }

        /// <summary>
        /// 캐릭터 검색 실행
        /// </summary>
        public async UniTaskVoid SearchCharacter()
        {
            if (_isSearching) return;

            string characterName = characterNameInput?.text?.Trim();
            if (string.IsNullOrEmpty(characterName))
            {
                SetStatus("캐릭터명을 입력해주세요.");
                return;
            }

            await SearchCharacterAsync(characterName);
        }

        /// <summary>
        /// 특정 캐릭터명으로 검색 (외부 호출용)
        /// </summary>
        public async UniTask<MapleCharacterData> SearchCharacterAsync(string characterName)
        {
            if (_isSearching) return null;

            _isSearching = true;
            SetLoadingState(true);
            SetStatus($"'{characterName}' 검색 중...");

            try
            {
                var data = await _apiClient.GetCharacterDataAsync(characterName);

                if (data != null)
                {
                    SetStatus($"Lv.{data.Level} {data.Name} ({data.World})");

                    // 이벤트 발생
                    OnCharacterFound?.Invoke(data);

                    // CharacterCardUI에 직접 전달
                    if (characterCardUI != null)
                        characterCardUI.SetCharacterData(data);

                    return data;
                }
                else
                {
                    string errorMsg = "캐릭터를 찾을 수 없습니다.";
                    SetStatus(errorMsg);
                    OnSearchError?.Invoke(errorMsg);
                    return null;
                }
            }
            catch (Exception e)
            {
                string errorMsg = $"검색 오류: {e.Message}";
                SetStatus(errorMsg);
                OnSearchError?.Invoke(errorMsg);
                Debug.LogError(e);
                return null;
            }
            finally
            {
                _isSearching = false;
                SetLoadingState(false);
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            if (loadingIndicator != null)
                loadingIndicator.SetActive(isLoading);

            if (searchButton != null)
                searchButton.interactable = !isLoading;

            if (characterNameInput != null)
                characterNameInput.interactable = !isLoading;
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }

        private void OnDestroy()
        {
            if (searchButton != null)
                searchButton.onClick.RemoveAllListeners();

            if (characterNameInput != null)
                characterNameInput.onSubmit.RemoveAllListeners();
        }
    }
}
