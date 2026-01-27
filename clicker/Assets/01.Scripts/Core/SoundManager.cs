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
        public Bgm type;
        public AudioClip clip;
        [Range(0, 1)] public float volume;
    }

    [System.Serializable]
    public struct SfxData
    {
        public Sfx type;
        public AudioClip clip;
        [Range(0, 1)] public float volume;
    }

    [SerializeField] private List<BgmData> bgmList;
    [SerializeField] private List<SfxData> sfxList;

    [SerializeField] private AudioSource audioBgm;
    [SerializeField] private AudioSource audioSfx;

    private Dictionary<Bgm, BgmData> bgmDict = new Dictionary<Bgm, BgmData>();
    private Dictionary<Sfx, SfxData> sfxDict = new Dictionary<Sfx, SfxData>();

    private void Awake()
    {
        if (_instance != null) { Destroy(gameObject); return; }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var item in bgmList) bgmDict[item.type] = item;
        foreach (var item in sfxList) sfxDict[item.type] = item;
    }

    public void PlaySFX(Sfx type)
    {
        if (sfxDict.TryGetValue(type, out SfxData data))
        {
            float finalVolume = masterVolume * sfxGroupVolume * data.volume;

            audioSfx.PlayOneShot(data.clip, finalVolume);
        }
    }

    public void PlayBGM(Bgm type)
    {
        if (bgmDict.TryGetValue(type, out BgmData data))
        {
            if (audioBgm.clip == data.clip) return;

            audioBgm.clip = data.clip;
            audioBgm.loop = true;

            audioBgm.volume = masterVolume * bgmGroupVolume * data.volume;
            audioBgm.Play();
        }
    }

}