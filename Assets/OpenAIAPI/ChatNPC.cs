using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatNPC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultTextUI;
    [SerializeField] private TMP_InputField _promptTextField;
    [SerializeField] private Button _sendButton;

    [SerializeField] private ApiKeyConfig _config;

    private List<Message> _messages = new List<Message>();

    private void Start()
    {
        // 버튼 클릭 이벤트
        _sendButton.onClick.AddListener(Send);
    }

    private async void Send()
    {
        // 프롬프트를 읽어온다.
        string prompt = _promptTextField.text;
        if (string.IsNullOrEmpty(prompt))
        {
            return;
        }

        // 0. 버튼을 잠근다.
        _sendButton.interactable = false;

        // 1. 챗지피티 사이트에 API_Key로 로그인한다.
        var api = new OpenAIClient(_config.OpenAIKey);

        // 2. 프롬프트를 작성해서 콘텍스트에 담는다.
        _messages.Add(new Message(Role.User, prompt));

        // 3. 모델을 선택하고 요청을 보낸다.
        var chatRequest = new ChatRequest(_messages, Model.GPT4oMini, temperature: 0);

        // 4. 응답을 비동기로 받는다.
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

        // 5. 답변이 여러 개일 수 있으므로 첫번째를 선택한다. (디폴트: 1개)
        var choice = response.FirstChoice;

        // 6. 응답을 콘텍스트에 담는다.
        _messages.Add(new Message(Role.Assistant, choice.Message));

        // 결과값을 UI에 출력한다.
        _resultTextUI.text = choice.Message;

        // 버튼을 푼다. (초기화)
        _promptTextField.text = string.Empty;
        _sendButton.interactable = true;
    }
}
