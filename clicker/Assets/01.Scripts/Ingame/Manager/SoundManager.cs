using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    [Header("Global Volume Settings")]
    [Range(0, 1)] public float masterVolume = 1.0f;
    [Range(0, 1)] public float bgmGroupVolume = 1.0f;
    [Range(0, 1)] public float sfxGroupVolume = 1.0f;

    public enum Bgm { BGM_GAME }
    public enum Sfx { CLICKPLANET, POPPLANET, COIN, SHURIKEN, ROCK }

    [System.Serializable]
    public struct BgmData
    {
        public Bgm Type;
        public AudioClip Clip;
        [Range(0, 1)] public float Volume;
    }

    [System.Serializable]
    public struct SfxData
    {
        public Sfx Type;
        public AudioClip Clip;
        [Range(0, 1)] public float Volume;
    }

    [SerializeField] private List<BgmData> _bgmList;
    [SerializeField] private List<SfxData> _sfxList;

    [SerializeField] private AudioSource _audioBgm;
    [SerializeField] private AudioSource _audioSfx;

    private Dictionary<Bgm, BgmData> _bgmDict = new Dictionary<Bgm, BgmData>();
    private Dictionary<Sfx, SfxData> _sfxDict = new Dictionary<Sfx, SfxData>();

    private void Awake()
    {
        if (_instance != null) { Destroy(gameObject); return; }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var item in _bgmList) _bgmDict[item.Type] = item;
        foreach (var item in _sfxList) _sfxDict[item.Type] = item;
    }

    public void PlaySFX(Sfx type)
    {
        if (_sfxDict.TryGetValue(type, out SfxData data))
        {
            float finalVolume = masterVolume * sfxGroupVolume * data.Volume;

            _audioSfx.PlayOneShot(data.Clip, finalVolume);
        }
    }

    public void PlayBGM(Bgm type)
    {
        if (_bgmDict.TryGetValue(type, out BgmData data))
        {
            if (_audioBgm.clip == data.Clip) return;

            _audioBgm.clip = data.Clip;
            _audioBgm.loop = true;

            _audioBgm.volume = masterVolume * bgmGroupVolume * data.Volume;
            _audioBgm.Play();
        }
    }

}