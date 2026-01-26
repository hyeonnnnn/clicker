using UnityEngine;

public class ClickTarget : MonoBehaviour, Clickable
{
    [SerializeField] private string _name;

    public bool OnClick()
    {
        Debug.Log($"{_name}: 클릭됨.");

        return true;
    }
}