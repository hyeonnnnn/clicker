using System;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace MapleAPI
{
    public class MapleAPIClient
    {
        private const string BASE_URL = "https://open.api.nexon.com/maplestory/v1";
        private readonly string _apiKey;

        public MapleAPIClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// 캐릭터명으로 전체 정보 조회 (ocid + 기본정보 + 스탯)
        /// </summary>
        public async UniTask<MapleCharacterData> GetCharacterDataAsync(string characterName)
        {
            try
            {
                // 1. ocid 조회
                var ocidResponse = await GetOcidAsync(characterName);
                if (string.IsNullOrEmpty(ocidResponse?.ocid))
                {
                    Debug.LogError($"캐릭터를 찾을 수 없습니다: {characterName}");
                    return null;
                }

                string ocid = ocidResponse.ocid;

                // 2. 기본 정보 + 스탯 정보 병렬 조회
                var (basic, stat) = await UniTask.WhenAll(
                    GetCharacterBasicAsync(ocid),
                    GetCharacterStatAsync(ocid)
                );

                return new MapleCharacterData
                {
                    Ocid = ocid,
                    Basic = basic,
                    Stat = stat
                };
            }
            catch (Exception e)
            {
                Debug.Log($"캐릭터 정보 조회 실패: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 캐릭터명으로 ocid 조회
        /// </summary>
        public async UniTask<OcidResponse> GetOcidAsync(string characterName)
        {
            string url = $"{BASE_URL}/id?character_name={UnityWebRequest.EscapeURL(characterName)}";
            return await SendRequestAsync<OcidResponse>(url);
        }

        /// <summary>
        /// 캐릭터 기본 정보 조회
        /// </summary>
        public async UniTask<CharacterBasicResponse> GetCharacterBasicAsync(string ocid, string date = null)
        {
            string url = $"{BASE_URL}/character/basic?ocid={ocid}";
            if (!string.IsNullOrEmpty(date))
                url += $"&date={date}";

            return await SendRequestAsync<CharacterBasicResponse>(url);
        }

        /// <summary>
        /// 캐릭터 능력치 정보 조회
        /// </summary>
        public async UniTask<CharacterStatResponse> GetCharacterStatAsync(string ocid, string date = null)
        {
            string url = $"{BASE_URL}/character/stat?ocid={ocid}";
            if (!string.IsNullOrEmpty(date))
                url += $"&date={date}";

            return await SendRequestAsync<CharacterStatResponse>(url);
        }

        /// <summary>
        /// 캐릭터 이미지 텍스처 로드
        /// </summary>
        public async UniTask<Texture2D> GetCharacterImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return null;

            try
            {
                using var request = UnityWebRequestTexture.GetTexture(imageUrl);
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"이미지 로드 실패: {request.error}");
                    return null;
                }

                return DownloadHandlerTexture.GetContent(request);
            }
            catch (Exception e)
            {
                Debug.LogError($"이미지 로드 예외: {e.Message}");
                return null;
            }
        }

        private async UniTask<T> SendRequestAsync<T>(string url) where T : class
        {
            using var request = UnityWebRequest.Get(url);
            request.SetRequestHeader("x-nxopen-api-key", _apiKey);

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"API 요청 실패: {request.error}\nURL: {url}\nResponse: {request.downloadHandler.text}");
                return null;
            }

            string json = request.downloadHandler.text;
            Debug.Log($"API 응답: {json}");

            return JsonUtility.FromJson<T>(json);
        }
    }
}
