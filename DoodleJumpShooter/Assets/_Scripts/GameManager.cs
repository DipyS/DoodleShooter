using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {private set; get;}
    public static List<GameObject> objects = new List<GameObject>();
    public bool gameIsLoosedOrStoped {get; private set;}
    public float spawnHeight {get; private set;} //Высота до которой нужно дойти, что бы активировался спавн
    public static UnityEvent onRestartGame = new UnityEvent();
    public static UnityEvent onLoseGame = new UnityEvent();
    public static UnityEvent onGenerate = new UnityEvent();
    public GameObject virtualCamera;
    public Vector2 startSpawnPos{
        get { return new Vector2(transform.position.x - widthSpawn, transform.position.y + height); }
    }
    
    public Vector2 endSpawnPos{
        get { return new Vector2(transform.position.x + widthSpawn, transform.position.y + height); }
    }
    [Header("Спавн Объектов")]
    [SerializeField] float height;
    [SerializeField] float widthSpawn;
    [SerializeField] float platformStep = 2.5f;
    [SerializeField, Header("Состояние игры")] 
    Player player;
    [SerializeField] GameObject LosePanel;
    [SerializeField] TextMeshProUGUI LosePanelScoresText;
    [SerializeField] TextMeshProUGUI scoresTMP;
    [SerializeField] int scoresPerY = 21;
    public int scores {get; private set;}
    public int highScoresGame {get; private set;}
    public int highScoresAll {get; private set;}
    bool canLose = true;
    
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        spawnHeight = transform.position.y + platformStep;
        player = player ?? GameObject.Find("Player").GetComponent<Player>();
        LosePanel.SetActive(false);
    }

    void Update()
    {
        UpdateScores();
        if (player.transform.position.y >= spawnHeight) {
            onGenerate.Invoke();
            spawnHeight += platformStep;
        }
        DestroyObjectsOutScreen();
        if (canLose && player.transform.position.y < transform.position.y - height) {
            Lose();
        }
    }

    public void RestartGame() {
        onRestartGame.Invoke();
        gameIsLoosedOrStoped = false;
        virtualCamera.transform.position = new Vector3(0,0,-10);
        player.transform.position = Vector2.zero;
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        spawnHeight = 0;
        scores = 0;
        highScoresGame = 0;
        canLose = false;
        Invoke(nameof(CanLose),0.15f);
        LosePanel.SetActive(false);
    }
    public void CanLose() {
        canLose = true;
    }
    void UpdateScores() {
        scores = (int)player.transform.position.y * scoresPerY;
        if (scores > highScoresGame) highScoresGame = scores;
        scoresTMP.text = highScoresGame.ToString();
    }    
    public void Lose() {
        onLoseGame.Invoke();
        gameIsLoosedOrStoped = true;
        LosePanel.SetActive(true);
        LosePanelScoresText.text = "Scores : " + highScoresGame.ToString();
        if (highScoresGame > highScoresAll) highScoresAll = highScoresGame;
        YandexGame.NewLeaderboardScores("High Scores", highScoresAll);
        player.GetComponent<BoxCollider2D>().enabled = false;
    }
    void DestroyObjectsOutScreen() {
        List<GameObject> objectsToDestroy = new List<GameObject>();
        foreach (GameObject obj in objects) {
            if (obj != null && obj.transform.position.y <= transform.position.y - height) {
                objectsToDestroy.Add(obj);
            }
        }
        foreach (GameObject obj in objectsToDestroy) {
            Destroy(obj.gameObject);
            objects.Remove(obj);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + height),0.3f);
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, spawnHeight),0.5f);
        Gizmos.DrawLine(new Vector2(transform.position.x + widthSpawn, transform.position.y + height),new Vector2(transform.position.x - widthSpawn, transform.position.y + height));
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y - height),0.3f);
        Gizmos.DrawLine(new Vector2(transform.position.x + widthSpawn, transform.position.y - height),new Vector2(transform.position.x - widthSpawn, transform.position.y - height));
    }
}
