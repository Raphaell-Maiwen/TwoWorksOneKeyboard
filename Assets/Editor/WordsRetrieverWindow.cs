using System.Text;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor tool: assign a WordsPool asset, pick how many times to call RetrieveWord(),
/// and press the button. Also surfaces the private _wordsCopy state via reflection so
/// you can watch (and force) the empty-pool/refill edge case.
///
/// IMPORTANT: Place this file inside a folder named "Editor" anywhere under Assets
/// (e.g. Assets/Editor/WordRetrieverWindow.cs). Scripts using UnityEditor must live
/// in an Editor folder or the project won't build for players.
/// </summary>
public class WordsRetrieverWindow : EditorWindow
{
    private const string WordsCopyFieldName = "_wordsCopy";
    private const string WordsFieldName = "_words";

    private WordsPool _targetPool;
    private int _retrieveCount = 1;
    private Vector2 _scrollPos;
    private readonly List<string> _results = new List<string>();

    private FieldInfo _wordsCopyField;
    private FieldInfo _wordsField;

    [MenuItem("Tools/Word Retriever")]
    public static void ShowWindow()
    {
        var window = GetWindow<WordsRetrieverWindow>("Word Retriever");
        window.minSize = new Vector2(360, 340);
    }

    private void OnEnable()
    {
        _wordsCopyField = typeof(WordsPool).GetField(WordsCopyFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        _wordsField = typeof(WordsPool).GetField(WordsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Word Retriever Tool", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox(
            "Assign a WordsPool asset. Its pool fills itself on load (OnEnable), " +
            "so this works in Edit Mode too — no need to press Play.",
            MessageType.Info);

        _targetPool = (WordsPool)EditorGUILayout.ObjectField(
            "Words Pool", _targetPool, typeof(WordsPool), false);

        EditorGUILayout.Space();

        DrawPoolStatus();

        EditorGUILayout.Space();

        _retrieveCount = EditorGUILayout.IntField("Times to Call", _retrieveCount);
        _retrieveCount = Mathf.Max(0, _retrieveCount);

        EditorGUILayout.Space();

        bool canRun = _targetPool != null && _retrieveCount > 0;

        using (new EditorGUI.DisabledScope(!canRun))
        {
            if (GUILayout.Button("Retrieve Words", GUILayout.Height(28)))
            {
                RunRetrieval();
            }
        }

        EditorGUILayout.Space();

        using (new EditorGUI.DisabledScope(_targetPool == null))
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Force Down to 1 Left"))
            {
                ForceDownToOne();
            }

            if (GUILayout.Button("Reset Pool (Refill Now)"))
            {
                ForceRefill();
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.HelpBox(
            "\"Force Down to 1 Left\" leaves exactly one word in the pool, so the very next " +
            "Retrieve call will empty it and trigger FillWordsPool() in the same call — " +
            "that's the transition to watch for below.",
            MessageType.None);

        EditorGUILayout.Space();

        if (_results.Count > 0)
        {
            EditorGUILayout.LabelField($"Results ({_results.Count}):", EditorStyles.boldLabel);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(180));
            foreach (var line in _results)
            {
                if (line.StartsWith("🔄"))
                {
                    var prevColor = GUI.color;
                    GUI.color = Color.yellow;
                    EditorGUILayout.LabelField(line, EditorStyles.boldLabel);
                    GUI.color = prevColor;
                }
                else
                {
                    EditorGUILayout.LabelField(line);
                }
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Clear Results"))
            {
                _results.Clear();
            }
        }
    }

    private void DrawPoolStatus()
    {
        if (_targetPool == null) return;

        int? remaining = GetWordsCopyCount();
        int? total = GetWordsCount();

        string text = remaining.HasValue && total.HasValue
            ? $"Remaining in pool: {remaining.Value} / {total.Value}"
            : "Remaining in pool: (unable to read via reflection)";

        var style = new GUIStyle(EditorStyles.boldLabel);
        if (remaining.HasValue && remaining.Value <= 1)
            style.normal.textColor = Color.yellow;

        EditorGUILayout.LabelField(text, style);
    }

    private int? GetWordsCopyCount()
    {
        var list = _wordsCopyField?.GetValue(_targetPool) as List<string>;
        return list?.Count;
    }

    private int? GetWordsCount()
    {
        var list = _wordsField?.GetValue(_targetPool) as List<string>;
        return list?.Count;
    }

    private void ForceDownToOne()
    {
        if (_targetPool == null || _wordsCopyField == null) return;

        var list = _wordsCopyField.GetValue(_targetPool) as List<string>;
        if (list == null || list.Count == 0) return;

        while (list.Count > 1)
        {
            list.RemoveAt(list.Count - 1);
        }

        EditorUtility.SetDirty(_targetPool);
        Repaint();
    }

    private void ForceRefill()
    {
        if (_targetPool == null || _wordsCopyField == null || _wordsField == null) return;

        var wordsCopy = _wordsCopyField.GetValue(_targetPool) as List<string>;
        var words = _wordsField.GetValue(_targetPool) as List<string>;
        if (wordsCopy == null || words == null) return;

        wordsCopy.Clear();
        wordsCopy.AddRange(words);

        EditorUtility.SetDirty(_targetPool);
        Repaint();
    }

    private void RunRetrieval()
    {
        if (_targetPool == null) return;

        var summary = new StringBuilder();
        int? prevCount = GetWordsCopyCount();

        for (int i = 0; i < _retrieveCount; i++)
        {
            string word = _targetPool.RetrieveWord();

            int? newCount = GetWordsCopyCount();

            bool refilled = prevCount.HasValue && newCount.HasValue
                             && newCount.Value > prevCount.Value - 1;

            if (refilled)
            {
                _results.Add($"🔄 Pool emptied → FillWordsPool() triggered (now {newCount.Value} words)");
            }

            _results.Add($"{i + 1}. {word}" + (newCount.HasValue ? $"   [{newCount.Value} left]" : ""));

            summary.Append(word);
            if (i < _retrieveCount - 1) summary.Append(", ");

            prevCount = newCount;
        }

        Debug.Log($"Word Retriever: called RetrieveWord() {_retrieveCount} time(s) -> {summary}");

        EditorUtility.SetDirty(_targetPool);
        Repaint();
    }
}