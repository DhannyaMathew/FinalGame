using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private KeyCode restartKey;
    [SerializeField] private KeyCode quitKey;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainCameraPrefab;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private Level[] _levels;

    private uint _currentLevelIndex;


    private bool _paused;
    private Player _player;
    private MainCamera _mainCamera;
    private Orb _orb;

    public static uint CurrentLevelIndex
    {
        get => _instance._currentLevelIndex;
        private set => _instance.SetLevel(value);
    }

    public static bool Paused => _instance._paused;
    public static Level CurrentLevel => GetLevel(_instance._currentLevelIndex);
    public static Player Player => _instance._player;
    public static MainCamera MainCamera => _instance._mainCamera;
    public static Orb Orb => _instance._orb;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        _player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        _mainCamera = Instantiate(mainCameraPrefab, Vector3.zero, Quaternion.identity).GetComponent<MainCamera>();
        _orb = Instantiate(orbPrefab, Vector3.zero, Quaternion.identity).GetComponent<Orb>();
    }

    private void Start()
    {
        CurrentLevelIndex = 0;
        TurnOffLevels();
        LinkLevels();
    }

    public void TransitionToNextLevel()
    {
        CurrentLevelIndex++;
    }

    private void SetLevel(uint level)
    {
        _currentLevelIndex = level;
        var prevLevel = GetLevel(_currentLevelIndex - 1);
        var nextLevel = GetLevel(_currentLevelIndex + 1);

        if (prevLevel != null)
        {
            prevLevel.Activate();
        }

        if (CurrentLevel != null)
        {
            CurrentLevel.Activate();
        }

        if (nextLevel != null)
        {
            nextLevel.Activate();
        }
    }

    private static Level GetLevel(uint level)
    {
        return level < _instance._levels.Length ? _instance._levels[level] : null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;
            Cursor.visible = !Paused;
            Cursor.lockState = !Paused ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (Input.GetKeyDown(quitKey))
        {
            Quit();
        }

        if (Input.GetKeyDown(restartKey))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetLevel(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetLevel(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetLevel(2);
        }
    }

    private void TurnOffLevels()
    {
        foreach (var level in _levels)
        {
            level.gameObject.SetActive(false);
        }
    }

    private void LinkLevels()
    {
        for (var i = 0; i < _levels.Length - 1; i++)
        {
            _levels[i].Link(_levels[i + 1]);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SetLevel(_currentLevelIndex);
    }
}