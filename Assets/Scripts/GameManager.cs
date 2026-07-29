using UnityEngine;
//using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private int _numberOfPlayers;

    private float placeHolderSpaceVariable = 25;

    private void Start()
    {
        var currentPosition = transform.position;
        currentPosition.x -= 100;

        var newRot = transform.rotation;
        
        for (int i = 0; i < _numberOfPlayers; i++)
        {
            Instantiate(_playerPrefab,  currentPosition, Quaternion.identity, transform);
            currentPosition.x += placeHolderSpaceVariable;
        }
    }
}
