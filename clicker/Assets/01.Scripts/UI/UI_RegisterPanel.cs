using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RegisterPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _checkPasswordInputField;
    [SerializeField] private Button _registerButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private TextMeshProUGUI _messageTextUI;

    private readonly AccountEmailSpecification _emailSpec = new();

    private void Start()
    {
        _registerButton.onClick.AddListener(Register);
        _backButton.onClick.AddListener(Back);
        _emailInputField.onValueChanged.AddListener(OnEmailChanged);
    }

    private void OnEmailChanged(string email)
    {
        _registerButton.interactable = _emailSpec.IsSatisfiedBy(email);
    }

    private void Register()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;
        string checkPassword = _checkPasswordInputField.text;

        if (string.IsNullOrEmpty(checkPassword) || password != checkPassword)
        {
            _messageTextUI.text = "패스워드를 확인해주세요.";
            return;
        }

        var result = AccountManager.Instance.TryRegister(email, password);

        if (result.Success)
        {
            _messageTextUI.text = "가입이 완료되었습니다.";
        }
        else
        {
            _messageTextUI.text = result.ErrorMessage;
        }
    }

    private void Back()
    {
        gameObject.SetActive(false);
    }
}
