using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LoginPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _goToRegisterButton;
    [SerializeField] private TextMeshProUGUI _messageTextUI;
    [SerializeField] private GameObject _registerPanel;

    private readonly AccountEmailSpecification _emailSpec = new();

    private void Start()
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
        var result = AccountManager.Instance.TryLogin(
            _emailInputField.text, _passwordInputField.text);

        if (result.Success)
        {
            SceneManager.LoadScene("SampleScene");
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
