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
        [SerializeField] private string _apiKey = "test_cd9ed653c405777e80b06476bb8ade5b4f00a84ae28a551eb20a03cddf75c6b3efe8d04e6d233bd35cf2fabdeb93fb0d";

        [Header("UI 컴포넌트")]
        [SerializeField] private TMP_InputField _characterNameInput;
        [SerializeField] private Button _searchButton;

        [Header("결과 전달")]
        [SerializeField] private UI_CharacterOutput _characterCardUI;
        [SerializeField] private UI_Description _description;

        // 이벤트
        public event Action<MapleCharacterData> OnCharacterFound;

        private MapleAPIClient _apiClient;
        private bool _isSearching;

        private void Awake()
        {
            _apiClient = new MapleAPIClient(_apiKey);
        }

        private void Start()
        {
            // 버튼 클릭 이벤트
            if (_searchButton != null)
                _searchButton.onClick.AddListener(() => SearchCharacter().Forget());

            // Enter 키 입력 이벤트
            if (_characterNameInput != null)
                _characterNameInput.onSubmit.AddListener(_ => SearchCharacter().Forget());

            // 초기 상태
            SetLoadingState(false);
        }

        /// <summary>
        /// 캐릭터 검색 실행
        /// </summary>
        public async UniTaskVoid SearchCharacter()
        {
            if (_isSearching) return;

            string characterName = _characterNameInput?.text?.Trim();
            if (string.IsNullOrEmpty(characterName))
            {
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
            SetLoadingState(true);;

            try
            {
                var data = await _apiClient.GetCharacterDataAsync(characterName);

                if (data != null)
                {
                    // 이벤트 발생
                    OnCharacterFound?.Invoke(data);

                    // CharacterCardUI에 직접 전달
                    if (_characterCardUI != null)
                        _characterCardUI.SetCharacterData(data);

                    if (_description != null)
                        _description.ShowSuccess();

                    return data;
                }
                else
                {
                    if (_description != null)
                        _description.ShowFail();

                    return null;
                }
            }
            catch (Exception e)
            {
                if (_description != null)
                    _description.ShowFail();

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
            if (_searchButton != null)
                _searchButton.interactable = !isLoading;

            if (_characterNameInput != null)
                _characterNameInput.interactable = !isLoading;
        }

        private void OnDestroy()
        {
            if (_searchButton != null)
                _searchButton.onClick.RemoveAllListeners();

            if (_characterNameInput != null)
                _characterNameInput.onSubmit.RemoveAllListeners();
        }
    }
}
