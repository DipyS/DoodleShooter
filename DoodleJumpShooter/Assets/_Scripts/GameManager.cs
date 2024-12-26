using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    public static bool canGenerate = true;
    public static GameManager Instance {private set; get;}
    public static List<GameObject> objects = new List<GameObject>();
    public bool gameIsLoosedOrStoped {get; private set;}
    public float spawnHeight {get; private set;} //Высота до которой нужно дойти, что бы активировался спавн
    public static UnityEvent onRestartGame = new UnityEvent();
    public static UnityEvent onLoseGame = new UnityEvent();
    public static UnityEvent onGenerate = new UnityEvent();
    public static UnityEvent onBossSpawn = new UnityEvent();
    public static Slider healthBar;
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

    [SerializeField, Header("Состояние игры")] GameObject LosePanel;
    [SerializeField] TextMeshProUGUI LosePanelScoresText;
    [SerializeField] TextMeshProUGUI scoresTMP;
    [SerializeField] AudioSource sound;

    [SerializeField] int scoresPerY = 21;
    public int scores {get; private set;}
    public int highScoresGame {get; private set;}
    public Player player {get; private set; }
    [HideInInspector] public int highScoresAll;
    bool canLose = true;
    
    void Awake()
    {
        Instance = this;
        healthBar = GameObject.Find("BossBar").GetComponent<Slider>();
        healthBar.gameObject.SetActive(false);
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
        if (player.transform.position.y >= spawnHeight && canGenerate) {
            onGenerate.Invoke();
            spawnHeight = player.transform.position.y + platformStep;
        }
        
        if (canGenerate) DestroyObjectsOutScreen();
        if (canLose && player.transform.position.y < transform.position.y - height) {
            if (player.Health > 0) {
                player.rb.velocity = new Vector2(player.rb.velocity.x, 22);
                player.TakeDamage(1);
            }
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
        YandexGame.FullscreenShow();
    }
    public void CanLose() {
        canLose = true;
    }
    void UpdateScores() {
        scores = (int)player.transform.position.y * scoresPerY;
        if (scores > highScoresGame) highScoresGame = scores;
        scoresTMP.text = highScoresGame.ToString();
    }  

    public void PlaySound(AudioClip clip, float duration = 1) {
        var newSound = Instantiate(sound, transform.position, Quaternion.identity);
        newSound.clip = clip;
        newSound.pitch += Random.Range(-0.2f, 0.2f);
        Destroy(newSound.gameObject, duration);
        newSound.Play();
    }

    public void Lose() {
        onLoseGame.Invoke();
        gameIsLoosedOrStoped = true;
        LosePanel.SetActive(true);
        LosePanelScoresText.text = "Scores : " + highScoresGame.ToString();
        if (highScoresGame > highScoresAll) {
            highScoresAll = highScoresGame;
            YandexGame.NewLeaderboardScores("HighScores", highScoresAll);
        }
        player.GetComponent<BoxCollider2D>().enabled = false;
    }
    void DestroyObjectsOutScreen() {
        List<GameObject> objectsToDestroy = new List<GameObject>();

        foreach (GameObject obj in objects) {
            if (obj != null) {
                if (obj.transform.position.y <= transform.position.y - height || obj.transform.position.y >= transform.position.y + height + 7)
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
