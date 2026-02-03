using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    [Header("입력 - 계정 정보")]
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;

    [Header("버튼 - 로그인/이동")]
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _goToRegisterButton;

    [Header("피드백 - 메시지")]
    [SerializeField] private TextMeshProUGUI _messageTextUI;

    [Header("패널 - 회원가입 화면")]
    [SerializeField] private GameObject _registerPanel;

    private readonly AccountEmailSpecification _emailSpec = new();

    private void Start()
    {
        AddButtonEvents();
    }

    private void AddButtonEvents()
    {
        _loginButton.onClick.AddListener(Login);
        _goToRegisterButton.onClick.AddListener(GoToRegister);
        _emailInputField.onValueChanged.AddListener(OnEmailChanged);
    }

    private void OnEmailChanged(string email)
    {
        _loginButton.interactable = _emailSpec.IsSatisfiedBy(email);
    }

    private void Login()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;

        var result = AccountManager.Instance.TryLogin(email, password);

        if (result.Success)
        {
            SceneManagerEx.Instance.LoadGameScene();
        }
        else
        {
            _messageTextUI.text = result.ErrorMessage;
        }
    }

    private void GoToRegister()
    {
        _registerPanel.SetActive(true);
    }
}
