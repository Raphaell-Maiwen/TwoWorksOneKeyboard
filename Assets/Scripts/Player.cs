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

    public void AssignWordTarget(string wordTarget)
    {
        _wordTarget = wordTarget.ToLower();
        _wordIndex = 0;
    }

    public void TryTypeLetter(char character)
    {
        if (character == _wordTarget[_wordIndex])
        {
            Debug.Log(character);
            //TODO: Update UI ; _wordProgressText
            _wordIndex++;

            if (_wordIndex >= _wordTarget.Length)
            {
                Win();
            }
        }
    }

    public void Win()
    {
        _victories++;
        Debug.Log("Win");
        //Call stop game in GameManager or something; change state
    }
}














