using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _sceneToLoadOnEnter;
    private InputAction enterAction;

    private void Awake()
    {
        enterAction = new InputAction(binding: "<Keyboard>/enter");
    }

    private void OnDestroy()
    {
        enterAction = null;
    }

    public void HandleEnter()
    {
        SceneManager.LoadScene(_sceneToLoadOnEnter);
    }
}
