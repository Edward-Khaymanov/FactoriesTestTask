using UnityEngine;

public class ResourceAmountView : MonoBehaviour
{
    [SerializeField] private TextView _textView;

    public void Render(string text)
    {
        _textView.Render(text);
    }
}