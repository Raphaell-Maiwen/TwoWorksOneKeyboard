using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "WordsPool", menuName = "Scriptable Objects/WordsPool")]
public class WordsPool : ScriptableObject
{
    [SerializeField] private List<ListWrapper> _wordsLists;
    public int WordListsCount => _wordsLists.Count;
    private List<ListWrapper> _wordsCopy = new List<ListWrapper>();
    private TextAsset wordsJson;
    private Dictionary<string, List<string>> allWords;
    private List<string> wordLengths;

    void OnEnable()
    {
        wordsJson = Resources.Load("wordList/wordList") as TextAsset;
        allWords = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(wordsJson.text);
        wordLengths = allWords.Keys.ToList();
            
        for (int i = 0; i < _wordsLists.Count; i++)
        {
            FillWordsPool(i);
        }
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
        List<string> words = new List<string>(allWords[wordLengths[Random.Range(0, wordLengths.Count)]]);
        
        foreach (var player in playersList)
        {
            string word = words[Random.Range(0, words.Count)];
            player.AssignWordTarget(word);
            words = words.Where(w => WordFilter(word, w)).ToList();
            Debug.Log(word);
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















