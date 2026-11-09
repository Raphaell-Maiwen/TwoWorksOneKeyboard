using UnityEngine;

[CreateAssetMenu(fileName = "Parameters", menuName = "Scriptable Objects/Parameters")]
public class Parameters : ScriptableObject
{
    [SerializeField] public int NumberOfPlayers;
}
