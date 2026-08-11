using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "WordsPool", menuName = "Scriptable Objects/WordsPool")]
public class WordsPool : ScriptableObject
{
    [SerializeField] private List<ListWrapper> _wordsLists;
    public int WordListsCount => _wordsLists.Count;
    private List<ListWrapper> _wordsCopy = new List<ListWrapper>();
    

    void OnEnable()
    {
        for (int i = 0; i < _wordsLists.Count; i++)
        {
            FillWordsPool(i);
        }
    }

    public string RetrieveWord(int wordListIndex)
    {
        string word = _wordsCopy[wordListIndex].words[Random.Range(0, _wordsCopy.Count)];
        _wordsCopy[wordListIndex].words.Remove(word);

        if (_wordsCopy[wordListIndex].words.Count == 0)
        {
            FillWordsPool(wordListIndex);
        }
        
        Debug.Log(word);
        return word;
    }
    
    //Refactor to multiple lists
    private void FillWordsPool(int listIndex)
    {
        if (_wordsCopy.Count <= listIndex)
        {
            _wordsCopy.Add(new ListWrapper());
        }
        else
        {
            _wordsCopy[listIndex].words.Clear();
        }
        
        foreach (string word in _wordsLists[listIndex].words)
        {
            _wordsCopy[listIndex].words.Add(word);
        }
    }
}

[System.Serializable]
public class ListWrapper
{
    public List<string> words = new List<string>();
}















