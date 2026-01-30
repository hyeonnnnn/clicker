using System.Text.RegularExpressions;
using UnityEngine;
using System;

public class Account : MonoBehaviour
{
    public readonly string Email;
    public readonly string Password;

    public Account(string email, string password)
    {
        var emailSpec = new AccountEmailSpecification();
        if (!emailSpec.IsSatisfiedBy(email))
        {
            throw new ArgumentException(emailSpec.ErrorMessage);
        }

        if (string.IsNullOrEmpty(password)) throw new ArgumentException($"비밀번호는 비어있을 수 없습니다.");
        if (password.Length < 6 || password.Length > 15) throw new ArgumentException($"비밀번호는 6자 이상 16자 이하여야 합니다.");

        Email = email;
        Password = password;
    }
}
