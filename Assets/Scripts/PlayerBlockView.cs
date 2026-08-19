using UnityEngine.UIElements;

public class PlayerBlockView
{
    readonly Label nameLabel, statLabel, statusLabel;

    public PlayerBlockView(VisualElement root)
    {
        nameLabel = root.Q<Label>("name-label");
        statLabel = root.Q<Label>("stat-label");
        statusLabel = root.Q<Label>("status-label");
    }

    public void SetData(PlayerData data)
    {
        nameLabel.text = data.Name;
        statLabel.text = data.Stat;
        statusLabel.text = data.Status;
    }
}

[System.Serializable]
public class PlayerData
{
    public string Name;
    public string Stat;
    public string Status;
}