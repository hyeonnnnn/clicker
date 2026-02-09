using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }

    private Account _currentAccount;
    public bool IsLogin => _currentAccount != null;
    public string Email => _currentAccount?.Email ?? string.Empty;

    private IAccountRepository _repository;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

#if !UNITY_WEBGL || UNITY_EDITOR
        _repository = new FirebaseAccountRepository();
#else
        _repository = new LocalAccountRepository();
#endif
    }

    public async UniTask<AccountResult> TryLogin(string email, string password)
    {
        // 유효성 검사
        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception ex)
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }

        // 레포지토리를 이용하여 로그인
        AccountResult result = await _repository.Login(email, password);
        if (result.Success)
        {
            _currentAccount = result.Account;
            return new AccountResult
            {
                Success = true,
                Account = _currentAccount,
            };
        }
        else
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = result.ErrorMessage,
            };
        }
    }

    public async UniTask<AccountResult> TryRegister(string email, string password)
    {
        // 유효성 검사
        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception ex)
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }

        // 레포지토리를 이용하여 회원가입
        AccountResult result = await _repository.Register(email, password);
        if (result.Success)
        {
            return new AccountResult
            {
                Success = true
            };
        }
        else
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = result.ErrorMessage,
            };
        }
    }

    public void Logout()
    {
        _repository.Logout();
    }
}
