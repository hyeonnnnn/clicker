using UnityEngine;
using Cysharp.Threading.Tasks;

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

    public UniTask<AccountResult> Register(string email, string password)
    {
        // 이메일 중복 검사
        if (!IsEmailAvailable(email))
        {
            return UniTask.FromResult(new AccountResult
            {
                Success = false,
                ErrorMessage = "이메일을 다시 확인해주세요.",
            });
        }
        // 비밀번호 암호화
        string hashedPassword = Crypto.HashPassword(password, SALT);

        PlayerPrefs.SetString(email, hashedPassword);

        return UniTask.FromResult(new AccountResult
        {
            Success = true,
            Account = new Account(email, password)
        });
    }

    public UniTask<AccountResult> Login(string email, string password)
    {
        // 가입된 이메일인지 검사
        if (!PlayerPrefs.HasKey(email))
        {
            return UniTask.FromResult(new AccountResult
            {
                Success = false,
                ErrorMessage = "존재하는 이메일이 아닙니다.",
            });
        }

        // 비밀번호 맞는지 검사
        string myPassword = PlayerPrefs.GetString(email);
        string hashedPassword = Crypto.HashPassword(password, SALT);
        if (myPassword != hashedPassword)
        {
            return UniTask.FromResult(new AccountResult
            {
                Success = false,
                ErrorMessage = "비밀번호를 다시 확인해주세요.",
            });
        }

        return UniTask.FromResult(new AccountResult
        {
            Success = true,
            Account = new Account(email, password)
        });
    }

    public void Logout()
    {
        Debug.Log("로그아웃 됐습니다.");
    }

}
