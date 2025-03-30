using TMPro;
using UnityEngine;

public class TextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textComponent;

    public void Render(string text)
    {
        _textComponent.text = text;
    }
}
