using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _sceneToLoadOnEnter;
    private InputAction enterAction;

    [SerializeField] private List<Transform> _shakyWords;
    [SerializeField] private List<Transform> _slowShakyWords;
    [SerializeField] private List<Transform> _horrorWords;
    [SerializeField] private List<Transform> _punchWords;
    
    private List<Tweener> _tweeners = new List<Tweener>();
    
    [SerializeField] private float tweensDuration;
    [SerializeField] private float _shakeStrength;
    [SerializeField] private int _shakeVibrato;
    
    [SerializeField] private float _slowShakeStrength;
    [SerializeField] private int _slowShakeVibrato;

    [SerializeField] private float _horrorShakeStrength;
    [SerializeField] private int _horrorShakeVibrato;
    
    [SerializeField] private float _punchStrenght;    
    [SerializeField] private int _vibrato;


    private void Awake()
    {
        enterAction = new InputAction(binding: "<Keyboard>/enter");
    }

    private void Start()
    {
        DOTween.Init(this);
        InitShakyTweens();
        InitSlowShakyTweens();
        InitHorrowTweens();
        InitPunchTweens();
    }

    private void InitPunchTweens()
    {
        foreach (var word in _punchWords)
        {
            _tweeners.Add(word.DOPunchScale(new Vector3(_punchStrenght, _punchStrenght, 0), tweensDuration, _vibrato).SetLoops(-1, LoopType.Restart));
        }
    }

    private void InitHorrowTweens()
    {
        foreach (var word in _horrorWords)
        {
            _tweeners.Add(word.DOShakePosition(tweensDuration, _horrorShakeStrength, _horrorShakeVibrato, 90, false, false).SetLoops(-1, LoopType.Restart));
        }
    }

    private void InitSlowShakyTweens()
    {
        foreach (var word in _slowShakyWords)
        {
            _tweeners.Add(word.DOShakePosition(tweensDuration, _slowShakeStrength, _slowShakeVibrato, 90, false, false).SetLoops(-1, LoopType.Restart));
        }
    }

    private void InitShakyTweens()
    {
        foreach (var word in _shakyWords)
        {
            _tweeners.Add(word.DOShakePosition(tweensDuration, _shakeStrength, _shakeVibrato, 90, false, false).SetLoops(-1, LoopType.Restart));
        }
    }

    private void OnDestroy()
    {
        enterAction = null;
    }

    public void HandleEnter()
    {
        SceneManager.LoadScene(_sceneToLoadOnEnter);
    }
}
