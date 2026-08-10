using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WordsPool _wordsPool;
    
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private int _numberOfPlayers;

    private float placeHolderSpaceVariable = 25;

    private void OnEnable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput += HandleTextInput;
        }
    }

    private void OnDisable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= HandleTextInput;
        }
    }
    
    private void Start()
    {
        var currentPosition = transform.position;
        currentPosition.x -= 100;

        var newRot = transform.rotation;
        
        for (int i = 0; i < _numberOfPlayers; i++)
        {
            Instantiate(_playerPrefab,  currentPosition, Quaternion.identity, transform);
            currentPosition.x += placeHolderSpaceVariable;
        }
        
        AssignWords();
    }

    private void AssignWords()
    {
        for (int i = 0; i < _numberOfPlayers; i++)
        {
            string word = _wordsPool.RetrieveWord();
        }
    }
    
    private void HandleTextInput(char character)
    {
        if (char.IsLetterOrDigit(character))
        {
            Debug.Log($"Alphanumeric key pressed: {character}");
        }
    }
}






















