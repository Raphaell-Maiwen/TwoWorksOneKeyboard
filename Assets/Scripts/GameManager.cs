using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Parameters _parameters;
    [SerializeField] private PlayerHudManager _playerHudManager;
    [SerializeField] private WordsPool _wordsPool;
    
    [SerializeField] private GameObject _playerPrefab;
    
    private List<Player> _playersList = new List<Player>();
    
    private GameState _gameState = GameState.Typing;
    
    
    private float placeHolderSpaceVariable = 25;

    [SerializeField] private TextMeshProUGUI _victoryMessage;
    [SerializeField] private List<TextMeshProUGUI> _endScreenMessages;
    
    private InputAction enterAction;

    public Action<int> OnGameStarted;
    public Action<int, string> OnWordChanged;

    private void OnEnable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput += HandleTextInput;
        }

        OnGameStarted += _playerHudManager.OnGameStart;
    }

    private void OnDisable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= HandleTextInput;
        }
        
        OnGameStarted -= _playerHudManager.OnGameStart;
    }
    
    private void Start()
    {
        OnGameStarted?.Invoke(_parameters.NumberOfPlayers);
        
        SetUpNewGame();
        ResetGame();
    }

    void SetUpNewGame()
    {
        var currentPosition = transform.position;
        currentPosition.x -= 100;

        var newRot = transform.rotation;
        enterAction = new InputAction(binding: "<Keyboard>/enter");
        
        
        for (int i = 0; i < _parameters.NumberOfPlayers; i++)
        {
            var player = Instantiate(_playerPrefab,  currentPosition, Quaternion.identity, transform);
            
            var playerComponent = player.GetComponent<Player>(); 
            _playersList.Add(playerComponent);
            playerComponent.GameWon += OnWin;
            playerComponent.SetPlayerIndex(i+1);
            
            currentPosition.x += placeHolderSpaceVariable;
            
            playerComponent.SetBlockView(_playerHudManager.GetPlayerBlockView(i));
        }
    }

    void ResetGame()
    {
        _victoryMessage.gameObject.SetActive(false);
        foreach (var msg in _endScreenMessages)
        {
            msg.gameObject.SetActive(false);
        }
        
        AssignWords(_playersList);
        _gameState = GameState.Typing;
    }
    
    public bool WordFilter(string pickedWord, string listWord)
    {
        foreach (var character in pickedWord)
        {
            if (listWord.Contains(character.ToString()))
            {
                return false;
            }
        }
        return true;
    }

    public void AssignWords(List<Player> playersList)
    {
        List<string> words;
        
        bool valid;
        int maxTries = 0;
        do
        {
            if (_parameters.NumberOfPlayers > 2)
            {
                words = new List<string>(_wordsPool.AllWords[1]);
            
            }
            else
            {
                words = new List<string>(_wordsPool.AllWords[Random.Range(0, 3)]);
            }
            
            valid = true;
            maxTries += 1;
            for (int i = 0; i < playersList.Count; i++)
            {
                if (words.Count == 0)
                {
                    valid = false;
                    Debug.Log(maxTries);
                    break;
                }

                string word = words[Random.Range(0, words.Count)];
                playersList[i].AssignWordTarget(word);
                words = words.Where(w => WordFilter(word, w)).ToList();

                OnWordChanged?.Invoke(i, word);
            }
        } while (!valid && maxTries <= 100000);
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
    }

    public void HandleEscape()
    {
        if (_gameState != GameState.WinScreen) return;
        
        GetBackToMainMenu();
    }

    public void OnWin(int playerIndex)
    {
        _victoryMessage.gameObject.SetActive(true);
        _victoryMessage.text = "Joueur " + playerIndex + " gagne!";
        foreach (var msg in _endScreenMessages)
        {
            msg.gameObject.SetActive(true);
        }
        
        _gameState = GameState.WinScreen;
    }

    private void GetBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

public enum GameState
{
    Typing,
    WinScreen
}





















