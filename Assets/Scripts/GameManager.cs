using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WordsPool _wordsPool;
    
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private int _numberOfPlayers;
    private List<Player> _playersList = new List<Player>();
    
    private GameState _gameState = GameState.Typing;
    
    
    private float placeHolderSpaceVariable = 25;

    [SerializeField] private TextMeshProUGUI _victoryMessage;
    [SerializeField] private TextMeshProUGUI _replayMessage;
    
    private InputAction enterAction;

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
        SetUpNewGame();
        ResetGame();
    }

    void SetUpNewGame()
    {
        var currentPosition = transform.position;
        currentPosition.x -= 100;

        var newRot = transform.rotation;
        enterAction = new InputAction(binding: "<Keyboard>/enter");
        
        
        for (int i = 0; i < _numberOfPlayers; i++)
        {
            var player = Instantiate(_playerPrefab,  currentPosition, Quaternion.identity, transform);
            
            var playerComponent = player.GetComponent<Player>(); 
            _playersList.Add(playerComponent);
            playerComponent.GameWon += OnWin;
            playerComponent.SetPlayerIndex(i+1);
            
            currentPosition.x += placeHolderSpaceVariable;
        }
    }

    void ResetGame()
    {
        _victoryMessage.gameObject.SetActive(false);
        _replayMessage.gameObject.SetActive(false);
        
        AssignWords();
        _gameState = GameState.Typing;
    }

    private void AssignWords()
    {
        int wordListIndex = Random.Range(0, _wordsPool.WordListsCount);
        
        foreach (var player in _playersList)
        {
            player.AssignWordTarget(_wordsPool.RetrieveWord(wordListIndex));
        }
    }
    
    private void HandleTextInput(char character)
    {
        if (_gameState != GameState.Typing) return;
        
        if (char.IsLetterOrDigit(character))
        {
            foreach (var player in _playersList)
            {
                player.TryTypeLetter(character);
            }
            
            //TODO: check win (decouple from TryTypeLetter, in case of ties), I guess
        }
    }

    public void HandleEnter()
    {
        if (_gameState != GameState.WinScreen) return;
        
        ResetGame();

        //Escape to get back to main menu
    }

    public void OnWin(int playerIndex)
    {
        _victoryMessage.gameObject.SetActive(true);
        _victoryMessage.text = "Player " + playerIndex + " wins!";
        _replayMessage.gameObject.SetActive(true);
        
        _gameState = GameState.WinScreen;
    }
}

public enum GameState
{
    Typing,
    WinScreen
}





















