using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private KeyCode restartKey;
    [SerializeField] private KeyCode quitKey;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainCameraPrefab;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private Volume sceneSettings;
    [SerializeField] private Volume postFX;
    [SerializeField] private Level[] _levels;
    private int _currentLevelIndex = 0;


    private bool _paused;
    private Player _player;
    private MainCamera _mainCamera;
    private Orb _orb;

    public static int CurrentLevelIndex
    {
        get => _instance._currentLevelIndex;
        private set
        {
            _instance._currentLevelIndex = Mathf.Clamp(value, 0, _instance._levels.Length - 1);
            _instance.TurnOffLevels();
            var prevLevel = GetLevel(_instance._currentLevelIndex - 1);
            var current = GetLevel(_instance._currentLevelIndex);
            var nextLevel = GetLevel(_instance._currentLevelIndex + 1);

            if (prevLevel != null)
            {
                prevLevel.Activate();
            }

            if (CurrentLevel != null)
            {
                current.Activate();
            }

            if (nextLevel != null)
            {
                nextLevel.Activate();
            }

            _instance.sceneSettings.profile = current.SceneSettings;
            _instance.postFX.profile = current.PostFx;
        }
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
        LinkLevels();
        TurnOffLevels();
        CurrentLevelIndex = 0;
        CurrentLevel.Setup(Player, MainCamera, Orb);
    }

    public static void TransitionToNextLevel()
    {
        CurrentLevelIndex++;
    }

    public static void TransitionToPreviousLevel()
    {
        CurrentLevelIndex--;
    }


    private static Level GetLevel(int level)
    {
        return level < _instance._levels.Length && level >= 0 ? _instance._levels[level] : null;
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
        CurrentLevel.Setup(Player, MainCamera, Orb);
    }
}