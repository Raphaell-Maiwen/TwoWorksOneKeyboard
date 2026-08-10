using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordsPool", menuName = "Scriptable Objects/WordsPool")]
public class WordsPool : ScriptableObject
{
    [SerializeField] private List<string> _words;
    private List<string> _wordsCopy = new List<string>();
    

    void OnEnable()
    {
        FillWordsPool();
    }

    public string RetrieveWord()
    {
        string word = _wordsCopy[Random.Range(0, _wordsCopy.Count)];
        _wordsCopy.Remove(word);

        if (_wordsCopy.Count == 0)
        {
            FillWordsPool();
        }
        
        Debug.Log(word);

        return word;
    }
    
    private void FillWordsPool()
    {
        _wordsCopy.Clear();
        foreach (string word in _words)
        {
            _wordsCopy.Add(word);
        }
    }
}















