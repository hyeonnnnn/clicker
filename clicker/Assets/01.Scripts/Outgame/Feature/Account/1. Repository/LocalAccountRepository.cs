using System;
using UnityEngine;
using UnityEngine.Windows;

public class LocalAccountRepository : IAccountRepository
{
    private const string SALT = "sh123";
    public bool IsEmailAvailable(string email)
    {
        // 이메일 중복 검사
        if (PlayerPrefs.HasKey(email))
        {
            return false;
        }
        return true;
    }

    public AuthResult Register(string email, string password)
    {
        // 이메일 중복 검사
        if (!IsEmailAvailable(email))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "비밀번호를 다시 확인해주세요.",
            };
        }
        // 비밀번호 암호화
        string hashedPassword = Crypto.HashPassword(password, SALT);

        PlayerPrefs.SetString(email, password);

        return new AuthResult()
        {
            Success = true,
            Account = new Account(email, password)
        };
    }

    public AuthResult Login(string email, string password)
    {
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

        return new AuthResult()
        {
            Success = true,
            // Account = new Account(email, password)
        };
    }

    public void Logout()
    {
        Debug.Log("로그아웃 됐습니다.");
    }

}
