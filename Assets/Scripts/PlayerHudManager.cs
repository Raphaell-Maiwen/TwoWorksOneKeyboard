using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHudManager : MonoBehaviour
{
    [SerializeField] UIDocument document;
    [SerializeField] VisualTreeAsset playerBlockTemplate; // drag PlayerBlock.uxml here

    readonly List<PlayerBlockView> blocks = new();

    void Start()
    {
        // Example call — replace with your real player list source
        // BuildLayout(playersFromLobby);
        
        BuildLayout(new List<PlayerData>
        {
            new PlayerData { Name = "Player 1", Stat = "worded", Typed = "wor"},
            new PlayerData { Name = "Player 2", Stat = "petted", Typed = "pet"},
            new PlayerData { Name = "Player 3", Stat = "petted", Typed = "pet"},
            new PlayerData { Name = "Player 4", Stat = "petted", Typed = "pet"},
            new PlayerData { Name = "Player 5", Stat = "petted", Typed = "pet"},
        });
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

    static int[] GetRowSplit(int n) => n switch
    {
        2 => new[] { 2 },
        3 => new[] { 3 },
        4 => new[] { 2, 2 },
        5 => new[] { 3, 2 },
        _ => new[] { n }
    };
}