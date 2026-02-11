using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MapleAPI.UI
{
    [Serializable]
    public class StatInfo
    {
        public string statName;      // API 응답에서 사용하는 스탯명
        public string displayName;   // UI에 표시할 이름
        public bool isEnabled;       // 토글 활성화 여부

        public StatInfo(string name, string display = null)
        {
            statName = name;
            displayName = display ?? name;
            isEnabled = false;
        }
    }

    public class StatToggleController : MonoBehaviour
    {
        [Header("프리팹")]
        [SerializeField] private GameObject statTogglePrefab;
        [SerializeField] private GameObject statDisplayPrefab;

        [Header("컨테이너")]
        [SerializeField] private Transform toggleContainer;   // 토글 목록 부모
        [SerializeField] private Transform displayContainer;  // 선택된 스탯 표시 부모

        [Header("설정")]
        [SerializeField] private bool initializeOnStart = true;

        // 기본 스탯 목록
        private readonly List<StatInfo> _defaultStats = new List<StatInfo>
        {
            new StatInfo("전투력"),
            new StatInfo("STR"),
            new StatInfo("DEX"),
            new StatInfo("INT"),
            new StatInfo("LUK"),
            new StatInfo("최대 HP", "최대 HP"),
            new StatInfo("최대 MP", "최대 MP"),
            new StatInfo("공격력"),
            new StatInfo("마력"),
            new StatInfo("방어력"),
            new StatInfo("이동속도"),
            new StatInfo("점프력"),
        };

        private List<StatInfo> _stats;
        private Dictionary<string, Toggle> _toggleMap = new Dictionary<string, Toggle>();
        private Dictionary<string, GameObject> _displayMap = new Dictionary<string, GameObject>();
        private MapleCharacterData _currentData;

        public event Action<List<StatInfo>> OnStatsChanged;

        private void Start()
        {
            if (initializeOnStart)
                Initialize();
        }

        /// <summary>
        /// 스탯 토글 초기화
        /// </summary>
        public void Initialize()
        {
            _stats = new List<StatInfo>(_defaultStats);
            CreateToggles();
        }

        /// <summary>
        /// 커스텀 스탯 목록으로 초기화
        /// </summary>
        public void Initialize(List<StatInfo> customStats)
        {
            _stats = new List<StatInfo>(customStats);
            CreateToggles();
        }

        private void CreateToggles()
        {
            if (statTogglePrefab == null || toggleContainer == null)
            {
                Debug.LogWarning("StatToggleController: 프리팹 또는 컨테이너가 설정되지 않았습니다.");
                return;
            }

            // 기존 토글 제거
            ClearContainer(toggleContainer);
            _toggleMap.Clear();

            foreach (var stat in _stats)
            {
                var toggleObj = Instantiate(statTogglePrefab, toggleContainer);
                toggleObj.name = $"Toggle_{stat.statName}";

                // 토글 컴포넌트 설정
                var toggle = toggleObj.GetComponent<Toggle>();
                if (toggle != null)
                {
                    toggle.isOn = stat.isEnabled;

                    // 클로저 캡처를 위한 로컬 변수
                    var statInfo = stat;
                    toggle.onValueChanged.AddListener(isOn => OnToggleChanged(statInfo, isOn));

                    _toggleMap[stat.statName] = toggle;
                }

                // 라벨 설정
                var label = toggleObj.GetComponentInChildren<TextMeshProUGUI>();
                if (label != null)
                    label.text = stat.displayName;
                else
                {
                    var legacyLabel = toggleObj.GetComponentInChildren<Text>();
                    if (legacyLabel != null)
                        legacyLabel.text = stat.displayName;
                }
            }
        }

        private void OnToggleChanged(StatInfo stat, bool isOn)
        {
            stat.isEnabled = isOn;
            UpdateStatDisplay();
            OnStatsChanged?.Invoke(GetEnabledStats());
        }

        /// <summary>
        /// 캐릭터 데이터로 스탯 값 업데이트
        /// </summary>
        public void UpdateStats(MapleCharacterData data)
        {
            _currentData = data;
            UpdateStatDisplay();
        }

        /// <summary>
        /// 활성화된 스탯만 표시 영역에 업데이트
        /// </summary>
        private void UpdateStatDisplay()
        {
            if (displayContainer == null || statDisplayPrefab == null) return;

            // 기존 표시 제거
            ClearContainer(displayContainer);
            _displayMap.Clear();

            if (_currentData == null || _stats == null) return;

            foreach (var stat in _stats)
            {
                if (!stat.isEnabled) continue;

                string value = _currentData.GetStatValue(stat.statName);
                if (string.IsNullOrEmpty(value) || value == "0") continue;

                var displayObj = Instantiate(statDisplayPrefab, displayContainer);
                displayObj.name = $"Stat_{stat.statName}";

                // 스탯 이름과 값 설정
                var texts = displayObj.GetComponentsInChildren<TextMeshProUGUI>();
                if (texts.Length >= 2)
                {
                    texts[0].text = stat.displayName;
                    texts[1].text = FormatStatValue(stat.statName, value);
                }
                else if (texts.Length == 1)
                {
                    texts[0].text = $"{stat.displayName}: {FormatStatValue(stat.statName, value)}";
                }

                _displayMap[stat.statName] = displayObj;
            }
        }

        /// <summary>
        /// 스탯 값 포맷팅 (%, 숫자 등)
        /// </summary>
        private string FormatStatValue(string statName, string value)
        {
            // % 스탯들
            if (statName.Contains("확률") || statName.Contains("데미지") ||
                statName.Contains("드롭") || statName.Contains("획득") ||
                statName.Contains("무시"))
            {
                return $"{value}%";
            }

            // 큰 숫자 포맷팅 (천, 만, 억)
            if (long.TryParse(value, out long numValue))
            {
                if (numValue >= 100000000)
                    return $"{numValue / 100000000.0:F1}억";
                if (numValue >= 10000)
                    return $"{numValue / 10000.0:F1}만";
                if (numValue >= 1000)
                    return $"{numValue:N0}";
            }

            return value;
        }

        /// <summary>
        /// 활성화된 스탯 목록 반환
        /// </summary>
        public List<StatInfo> GetEnabledStats()
        {
            return _stats.FindAll(s => s.isEnabled);
        }

        /// <summary>
        /// 특정 스탯 토글 상태 변경
        /// </summary>
        public void SetStatEnabled(string statName, bool enabled)
        {
            var stat = _stats.Find(s => s.statName == statName);
            if (stat != null)
            {
                stat.isEnabled = enabled;
                if (_toggleMap.TryGetValue(statName, out var toggle))
                    toggle.isOn = enabled;
                UpdateStatDisplay();
            }
        }

        /// <summary>
        /// 모든 스탯 토글 해제
        /// </summary>
        public void ClearAll()
        {
            foreach (var stat in _stats)
            {
                stat.isEnabled = false;
                if (_toggleMap.TryGetValue(stat.statName, out var toggle))
                    toggle.isOn = false;
            }
            UpdateStatDisplay();
        }

        /// <summary>
        /// 기본 스탯만 활성화 (전투력, 주스탯)
        /// </summary>
        public void SetDefaultStats()
        {
            ClearAll();
            SetStatEnabled("전투력", true);
            SetStatEnabled("STR", true);
            SetStatEnabled("DEX", true);
            SetStatEnabled("INT", true);
            SetStatEnabled("LUK", true);
        }

        private void ClearContainer(Transform container)
        {
            for (int i = container.childCount - 1; i >= 0; i--)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }
}
