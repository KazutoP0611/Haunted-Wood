using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplaySceneController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera[] cameras;

    [Header("UI Settings")]
    [SerializeField] private Image keyIcon;
    [SerializeField] private Image fullHeartImage;
    [SerializeField] private InteractText interactText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;

    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Room Settings")]
    [SerializeField] private GameObject map;
    [SerializeField] private AreaFieldController areaFieldController;

    [Header("Box Randomer")]
    [SerializeField] private BoxRandomer boxRandomer;

    [Header("King Room Settings")]
    [SerializeField] private GameObject kingRoomText;
    [SerializeField] private float showKingRoomTextForSecs;
    [SerializeField] private Interactable frontDoorInteractable;
    [SerializeField] private Interactable backDoorInteractable;
    [SerializeField] private AreaBoss bossRoom;
    [SerializeField] private float waitForSecsAfterEnterBossRoom;
    [SerializeField] private float waitForSecsAfterBossDied;

    [Header("King Appearance Settings")]
    [SerializeField] private GameObject bossAppearance;
    [SerializeField] private GameObject bossEnemy;
    [SerializeField] private float waitForSecsUntilSpawnKing;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject EnemySkeletonPrefab;
    [SerializeField] private GameObject EnemyGrimreaperPrefab;
    public GameObject EnemySkeleton { get { return EnemySkeletonPrefab; } }
    public GameObject EnemyGrimreaper { get { return EnemyGrimreaperPrefab; } }

    public static GameplaySceneController instance;

    public Player player { get; private set; }

    private Coroutine kingRoomTextCoroutine;
    private Coroutine waitAfterEnterBossRoomCoroutine;
    private Coroutine bossAppearSequence;
    private Dictionary<int, Camera> cameraDict = new Dictionary<int, Camera>();
    private int score = 0;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
        areaFieldController.InitializeAreaField(ChangeRoom);
        interactText.Initialize(player);

        gameOverPanel.SetActive(false);
        gameClearPanel.SetActive(false);

        for (int i = 0; i < cameras.Length; i++)
            cameraDict.Add(i, cameras[i]);

        //Debug.Log(cameraDict.Count);

        bossRoom.InitAreaBoss(OnBossRoomInteract);
        frontDoorInteractable.InitializeInteractable(OnInteractFrontDoor, OnFrontDoorOpened);
        backDoorInteractable.InitializeInteractable(OnEnterBossRoom, OnEnterBossRoomFinished);
    }

    private void Start()
    {
        OpenCamera(0);
        if (instance != null)
            instance = null;

        instance = this;

        SetActiveKeyIcon(false);
        kingRoomText.SetActive(false);
        boxRandomer.SpawnItems();

        BGMManager.instance.ChangeSongTo(BGMSong.gameSceneSong);
    }

    public void SetActiveKeyIcon(bool active)
    {
        keyIcon.color = new Color(1, 1, 1, active ? 1.0f : 0.2f);
    }

    public void UpdateHealthPoint(float healthPoint)
    {
        fullHeartImage.fillAmount = healthPoint;
    }

    public void ShowKingRoomText()
    {
        if (kingRoomText.activeSelf)
            return;

        kingRoomText.SetActive(true);

        if (kingRoomTextCoroutine != null)
            StopCoroutine(kingRoomTextCoroutine);

        kingRoomTextCoroutine = StartCoroutine(ShowKingRoomTextCoroutine());
    }

    IEnumerator ShowKingRoomTextCoroutine()
    {
        yield return new WaitForSeconds(showKingRoomTextForSecs);
        kingRoomText.SetActive(false);
    }

    private void ChangeRoom(Area area)
    {
        map.transform.position = area.GetRoomPosition;
    }

    public void SetActiveInteractText(bool active)
    {
        interactText.gameObject.SetActive(active);
    }

    private void OnInteractFrontDoor()
    {
        OpenCamera(1);
        //disable player movement
        player.SetEnableInput(false);
    }

    private void OnFrontDoorOpened()
    {
        frontDoorInteractable.gameObject.SetActive(false);
        OpenCamera(0);
        player.SetEnableInput(true);
    }

    private void OnBossRoomInteract(AreaBoss bossRoom)
    {
        ChangeRoom(bossRoom);
        areaFieldController.ExitAreaField();
        player.SetEnableInput(false);

        if (waitAfterEnterBossRoomCoroutine != null)
            StopCoroutine(waitAfterEnterBossRoomCoroutine);

        waitAfterEnterBossRoomCoroutine = StartCoroutine(WaitAfterEnteredBossRoom());
    }

    IEnumerator WaitAfterEnteredBossRoom()
    {
        yield return new WaitForSeconds(waitForSecsAfterEnterBossRoom);

        backDoorInteractable.Interacted();
    }

    private void OnEnterBossRoom()
    {
        BGMManager.instance.ChangeSongTo(BGMSong.bossSong);
        OpenCamera(2);
    }

    private void OnEnterBossRoomFinished()
    {
        OpenCamera(0);

        if (bossAppearSequence != null)
            StopCoroutine(bossAppearSequence);

        bossAppearSequence = StartCoroutine(SpawnBossSequence());
    }

    IEnumerator SpawnBossSequence()
    {
        yield return new WaitForSeconds(waitForSecsUntilSpawnKing);
        bossAppearance.SetActive(true);
    }

    private void OpenCamera(int cameraIndex)
    {
        for (int i = 0; i < cameraDict.Count; i++)
        {
            cameraDict[i].enabled = (i == cameraIndex);
        }
    }

    public void OnGameOver()
    {
        areaFieldController.ClearRoom();
        gameOverPanel.SetActive(true);
    }

    public void SpawnBossEnemy()
    {
        Enemy boss = Instantiate(bossEnemy, bossRoom.GetEnemySpawnPoint.position, Quaternion.identity, bossRoom.GetEnemyParent).GetComponent<Enemy>();
        boss.InitEnemy(BossDead, waitForSecsAfterBossDied);
    }

    public void BossAppearanceEventFinished()
    {
        player.SetEnableInput(true);
        bossAppearance.SetActive(false);
    }

    public void ReloadScene()
    {
        GameManager.instance.ChangeLevelTo(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        GameManager.instance.ChangeLevelTo(0);
    }

    public void BossDead()
    {
        gameClearPanel.SetActive(true);
        player.SetEnableInput(false);
    }

    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString("000000");
    }
}
