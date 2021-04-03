using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _nameField;
    [SerializeField]
    private GameObject _gameStartPanel;
    [SerializeField]
    private GameObject _inGamePanel;
    [SerializeField]
    private TextMeshProUGUI _TMPgameover;

    private void Awake()
    {
        _nameField.characterValidation = TMP_InputField.CharacterValidation.Name;
    }

    public void HidePreGameUI()
    {
        _gameStartPanel.gameObject.SetActive(false);
        ShowInGameUI();
    }
    private void ShowInGameUI()
    {
        _inGamePanel.gameObject.SetActive(true);
    }

    public void ShowEndGameText(string text)
    {
        _TMPgameover.text = text;
        _TMPgameover.gameObject.SetActive(true);
    }

    public string GetUserName()
    {
        return _nameField.text;
    }
}
