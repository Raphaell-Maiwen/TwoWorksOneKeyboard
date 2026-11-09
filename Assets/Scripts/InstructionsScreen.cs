using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InstructionsScreen : MonoBehaviour
{
    [SerializeField] private Parameters _parameters;
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private string _sceneToLoadOnEnter;
    private InputAction enterAction;

    private void Awake()
    {
        enterAction = new InputAction(binding: "<Keyboard>/enter");
        _dropdown.onValueChanged.AddListener(delegate {OnNumberPlayersChanged ();});
    }

    private void OnDestroy()
    {
        enterAction = null;
        _dropdown.onValueChanged.RemoveListener(delegate {OnNumberPlayersChanged ();});
    }

    private void OnNumberPlayersChanged()
    {
        _parameters.NumberOfPlayers = int.Parse(_dropdown.options[_dropdown.value].text);
    }

    public void HandleEnter()
    {
        SceneManager.LoadScene(_sceneToLoadOnEnter);
    }
}
