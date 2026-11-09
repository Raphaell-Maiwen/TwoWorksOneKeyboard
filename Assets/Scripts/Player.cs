using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Put the UXML variables here?

    private string _wordTarget;
    private int _wordIndex;
    private int _victories = 0;
    
    public event Action<int> GameWon;
    private int _playerIndex;

    private PlayerBlockView _blockView;

    public void SetBlockView(PlayerBlockView blockView)
    {
        _blockView = blockView;
    }

    public void SetPlayerIndex(int playerIndex)
    {
        _playerIndex = playerIndex;
    }

    public void AssignWordTarget(string wordTarget)
    {
        _wordTarget = wordTarget.ToLower();
        _wordIndex = 0;
        
        _blockView.SetTargetWord(wordTarget);
        _blockView.SetTyped("");
        
        Debug.Log(_wordTarget);
    }

    public void TryTypeLetter(char character)
    {
        if (character == _wordTarget[_wordIndex])
        {
            UpdateProgressTextUI(character);
            _wordIndex++;

            if (_wordIndex >= _wordTarget.Length)
            {
                Win();
            }
        }
    }

    private void UpdateProgressTextUI(char character)
    {
        _blockView.AddTypedCharacter(character);
    }

    public void Win()
    {
        _victories++;
        GameWon?.Invoke(_playerIndex);
        //Call stop game in GameManager or something; change state
    }
}














