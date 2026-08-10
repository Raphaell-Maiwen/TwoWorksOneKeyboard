using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _wordTargetText;
    [SerializeField] private TextMeshProUGUI _wordProgressText;
    [SerializeField] private TextMeshProUGUI _victoriesText;

    private string _wordTarget;
    private int _wordIndex;
    private int _victories = 0;
    
    public event Action<int> GameWon;
    private int _playerIndex;

    public void SetPlayerIndex(int playerIndex)
    {
        _playerIndex = playerIndex;
    }

    public void AssignWordTarget(string wordTarget)
    {
        _wordTarget = wordTarget.ToLower();
        _wordIndex = 0;
        
        _wordTargetText.text = wordTarget;
        _wordProgressText.text = "";
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
        _wordProgressText.text += character;
    }

    public void Win()
    {
        _victories++;
        GameWon?.Invoke(_playerIndex);
        //Call stop game in GameManager or something; change state
    }
}














