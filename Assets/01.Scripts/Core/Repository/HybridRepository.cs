using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class HybridRepository<T> where T : class, ISaveData, new()
{
    private readonly IRepository<T> _localRepository;
    private readonly IRepository<T> _firebaseRepository;

    private int _localSaveCount;
    private const int REMOTE_SAVE_INTERVAL = 5;
    private const float DEBOUNCE_SECONDS = 0.6f;

    private CancellationTokenSource _debounceCts; // 디바운스 타이머 취소용
    private T _pendingData; // 저장 대기 중인 데이터를 임시 보관하는 버퍼

    public HybridRepository(IRepository<T> local, IRepository<T> firebase)
    {
        _localRepository = local;
        _firebaseRepository = firebase;
    }

    public void Save(T data)
    {
        data.LastSavedAt = DateTime.UtcNow.ToString("o");   // 지금 저장할게
        _pendingData = data;                                // 최신 데이터로 덮어쓰고

        _debounceCts?.Cancel();                             // 이전에 돌고 있던 저장 타이머가 있으면 취소
        _debounceCts = new CancellationTokenSource();       // 새로운 취소 토큰

        DebounceSaveAsync(_debounceCts.Token).Forget();     // 새로운 저장 타이머 시작
    }

    private async UniTaskVoid DebounceSaveAsync(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DEBOUNCE_SECONDS), cancellationToken: token);

            if (_pendingData == null) return;

            // 로컬에는 무조건 저장
            await _localRepository.Save(_pendingData);
            _localSaveCount++;

            // 파이어베이스에는 5번 중 1번만 저장
            if (_firebaseRepository != null && _localSaveCount >= REMOTE_SAVE_INTERVAL)
            {
                await _firebaseRepository.Save(_pendingData);
                _localSaveCount = 0;
            }

            _pendingData = null;
        }
        catch (OperationCanceledException)
        {
            // 디바운스로 인한 취소
        }
    }

    public async UniTask<T> Load()
    {
        if (_firebaseRepository == null)
        {
            return await _localRepository.Load();
        }

        var (local, firebase) = await UniTask.WhenAll(
            _localRepository.Load(),
            _firebaseRepository.Load()
        );

        var localTime = ParseTime(local.LastSavedAt);
        var firebaseTime = ParseTime(firebase.LastSavedAt);

        return localTime >= firebaseTime ? local : firebase;
    }

    public async UniTask ForceRemoteSave()
    {
        _debounceCts?.Cancel();

        var dataToSave = _pendingData ?? await _localRepository.Load();

        if (dataToSave != null)
        {
            dataToSave.LastSavedAt = DateTime.UtcNow.ToString("o");

            if (_firebaseRepository != null)
            {
                await UniTask.WhenAll(
                    _localRepository.Save(dataToSave),
                    _firebaseRepository.Save(dataToSave)
                );
            }
            else
            {
                await _localRepository.Save(dataToSave);
            }

            _pendingData = null;
            _localSaveCount = 0;
        }
    }

    private static DateTime ParseTime(string iso8601)
    {
        if (string.IsNullOrEmpty(iso8601)) return DateTime.MinValue;
        return DateTime.TryParse(iso8601, out var result) ? result : DateTime.MinValue;
    }
}
