using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHudManager : MonoBehaviour
{
    [SerializeField] UIDocument document;
    [SerializeField] VisualTreeAsset playerBlockTemplate; // drag PlayerBlock.uxml here

    readonly List<PlayerBlockView> blocks = new();

    public void OnGameStart(int numberOfPlayers)
    {
        List<PlayerData> playerDatas = new List<PlayerData>();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerDatas.Add(new PlayerData{ Name = "Player " + (i+1), TargetWord = "test", Typed = "" });
        }
        
        BuildLayout(playerDatas);
    }

    public void BuildLayout(List<PlayerData> players)
    {
        var root = document.rootVisualElement.Q<VisualElement>("hud-root");
        root.Clear();
        blocks.Clear();

        int index = 0;
        foreach (var rowCount in GetRowSplit(players.Count))
        {
            var row = new VisualElement();
            row.AddToClassList("row");

            for (int i = 0; i < rowCount; i++)
            {
                var blockRoot = playerBlockTemplate.CloneTree();
                blockRoot.AddToClassList("player-block");
                var view = new PlayerBlockView(blockRoot);
                view.SetData(players[index]);
                row.Add(blockRoot);
                blocks.Add(view);
                index++;
            }
            root.Add(row);
        }
    }
    
    public void SetTypedWordLabel(int playerIndex, string word)
    {
        blocks[playerIndex].SetTyped(word);
    }

    public PlayerBlockView GetPlayerBlockView(int index)
    {
        return blocks[index];
    }

    static int[] GetRowSplit(int n) => n switch
    {
        2 => new[] { 2 },
        3 => new[] { 3 },
        4 => new[] { 2, 2 },
        5 => new[] { 3, 2 },
        _ => new[] { n }
    };
}