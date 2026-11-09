using UnityEngine.UIElements;

public class PlayerBlockView
{
    readonly Label nameLabel, statLabel, statusLabel;

    public PlayerBlockView(VisualElement root)
    {
        nameLabel = root.Q<Label>("name-label");
        statLabel = root.Q<Label>("stat-label");
        statusLabel = root.Q<Label>("typed-label");
    }

    public void SetData(PlayerData data)
    {
        nameLabel.text = data.Name;
        statLabel.text = data.TargetWord;
        statusLabel.text = data.Typed;
    }

    public void SetTargetWord(string  targetWord)
    {
        statLabel.text = targetWord;
    }

    public void SetTyped(string typed)
    {
        statusLabel.text = typed;
    }

    public void AddTypedCharacter(char character)
    {
        statusLabel.text += character;
    }
}

[System.Serializable]
public class PlayerData
{
    public string Name;
    public string TargetWord;
    public string Typed;
}