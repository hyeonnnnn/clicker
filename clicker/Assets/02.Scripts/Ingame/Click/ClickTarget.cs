using UnityEngine;

[RequireComponent (typeof(ScaleTweeningFeedback))]
public class ClickTarget : MonoBehaviour, Clickable
{
    [SerializeField] private string _name;
    private ScaleTweeningFeedback _scaleTweeningFeedback;

    private void Awake()
    {
        _scaleTweeningFeedback = GetComponent<ScaleTweeningFeedback>();
    }

    public bool OnClick(ClickInfo clickInfo)
    {
        Debug.Log($"{_name}: 클릭됨.");
        _scaleTweeningFeedback.Play();

        return true;
    }
}