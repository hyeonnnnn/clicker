using UnityEngine;
using System;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }
    private Account _currentAccount;
    public bool IsLogin => _currentAccount != null;
    public string Email => _currentAccount?.Email ?? string.Empty;

    private void Awake()
    {
        Instance = this;
    }

    public AuthResult TryLogin(string email, string password)
    {
        Account account = null;

        // 유효성 검사
        try
        {
            account = new Account(email, password);
        }
        catch (Exception ex)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }

        // 가입된 이메일인지 검사
        if (!PlayerPrefs.HasKey(email))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "존재하는 이메일이 아닙니다.",
            };
        }

        // 비밀번호 맞는지 검사
        string myPassword = PlayerPrefs.GetString(email);
        if (myPassword != password)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "비밀번호를 다시 확인해주세요.",
            };
        }

        return new AuthResult
        {
            Success = true,
        };
    }

    public AuthResult TryRegister(string email, string password)
    {
        // 이메일 중복 검사
        if (PlayerPrefs.HasKey(email))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "비밀번호를 다시 확인해주세요.",
            };
        }

        // 유효성 검사
        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception ex)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }

        PlayerPrefs.SetString(email, password);
        return new AuthResult
        {
            Success = true,
        };
    }

    public void Logout()
    {

    }
}
