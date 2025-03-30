using UnityEngine;
using UnityEngine.UI;

public class OpenStorageButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        ServiceLocator.Get<StorageWindow>().Show();
    }
}