using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WordsPool _wordsPool;
    
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private int _numberOfPlayers;
    private List<Player> _playersList = new List<Player>();
    
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
            var player = Instantiate(_playerPrefab,  currentPosition, Quaternion.identity, transform);
            _playersList.Add(player.GetComponent<Player>());
            
            currentPosition.x += placeHolderSpaceVariable;
        }
        
        AssignWords();
    }

    private void AssignWords()
    {
        foreach (var player in _playersList)
        {
            player.AssignWordTarget(_wordsPool.RetrieveWord());
        }
    }
    
    private void HandleTextInput(char character)
    {
        if (char.IsLetterOrDigit(character))
        {
            foreach (var player in _playersList)
            {
                player.TryTypeLetter(character);
            }
            
            //TODO: check win (decouple from TryTypeLetter, in case of ties), I guess
        }
    }
}






















