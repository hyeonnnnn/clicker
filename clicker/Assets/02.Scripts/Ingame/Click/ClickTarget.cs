using UnityEngine;

[RequireComponent (typeof(ScaleTweeningFeedback))]
[RequireComponent(typeof(ColorFlashFeedback))]
public class ClickTarget : MonoBehaviour, Clickable
{
    [SerializeField] private string _name;
    private ScaleTweeningFeedback _scaleTweeningFeedback;
    private ColorFlashFeedback _colorFlashFeedback;

    private void Awake()
    {
        _scaleTweeningFeedback = GetComponent<ScaleTweeningFeedback>();
        _colorFlashFeedback = GetComponent<ColorFlashFeedback>();
    }

    public bool OnClick(ClickInfo clickInfo)
    {
        Debug.Log($"{_name}: 클릭됨.");
        var feedbacks = GetComponentsInChildren<IFeedback>();
        foreach ( var feedback in feedbacks)
        {
            feedback.Play();
        }

        return true;
    }
}