using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private KeyCode restartKey;
    [SerializeField] private KeyCode quitKey;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainCameraPrefab;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private int startLevel;
    [SerializeField] private Level[] levels;

    private int _currentLevelIndex;
    private bool _paused;
    private Player _player;
    private MainCamera _mainCamera;
    private Orb _orb;

    private static int PrevLevelIndex => _instance._currentLevelIndex - 1;
    private static int NextLevelIndex => _instance._currentLevelIndex + 1;

    private static int CurrentLevelIndex
    {
        get => _instance._currentLevelIndex;
        set
        {
            var nextLevelIndex = Mathf.Clamp(value, 0, _instance.levels.Length - 1);
            var diff = nextLevelIndex - CurrentLevelIndex;
            switch (diff)
            {
                case -2:
                    _instance.TurnOffLevel(CurrentLevelIndex);
                    _instance.TurnOffLevel(NextLevelIndex);
                    break;
                case 2:
                    _instance.TurnOffLevel(CurrentLevelIndex);
                    _instance.TurnOffLevel(PrevLevelIndex);
                    break;
                case -1:
                    _instance.TurnOffLevel(NextLevelIndex);
                    break;
                case 1:
                    _instance.TurnOffLevel(PrevLevelIndex);
                    break;
                case 0:
                    break;
                default:
                    _instance.TurnOffLevels();
                    break;
            }

            _instance._currentLevelIndex = nextLevelIndex;
            var prevLevel = GetLevel(PrevLevelIndex);
            var current = GetLevel(CurrentLevelIndex);
            var nextLevel = GetLevel(NextLevelIndex);
            if (CurrentLevel != null)
            {
                current.Activate();
                current.TurnOnparticleSystems();
                current.TurnOnDirectionalLight();
                MainCamera.GetComponent<HDAdditionalCameraData>().volumeAnchorOverride = current.transform;
            }

            if (prevLevel != null)
            {
                prevLevel.Activate();
                prevLevel.TurnOffparticleSystems();
                prevLevel.TurnOffDirectionalLight();
            }

            if (nextLevel != null)
            {
                nextLevel.Activate();
                nextLevel.TurnOffparticleSystems();
                nextLevel.TurnOffDirectionalLight();
            }
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
        CurrentLevelIndex = startLevel;
        CurrentLevel.Setup(Player, MainCamera, Orb);
        EventHandler.OnDoorWalkThrough += transition =>
        {
            switch (transition)
            {
                case Level.Transition.NEXT:
                    CurrentLevelIndex++;
                    break;
                case Level.Transition.PREV:
                    CurrentLevelIndex--;
                    break;
            }
        };
    }


    private static Level GetLevel(int level)
    {
        return level < _instance.levels.Length && level >= 0 ? _instance.levels[level] : null;
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

    public static void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        CurrentLevel.Setup(Player, MainCamera, Orb);
    }

    private void LinkLevels()
    {
        for (var i = 0; i < levels.Length - 1; i++)
        {
            levels[i].Link(levels[i + 1]);
        }
    }

    private void TurnOffLevel(int index)
    {
        if (index >= 0 && index < levels.Length)
        {
            levels[index].Deactivate();
        }
    }

    private void TurnOffLevels()
    {
        foreach (var level in levels)
        {
            level.Deactivate();
        }
    }
}