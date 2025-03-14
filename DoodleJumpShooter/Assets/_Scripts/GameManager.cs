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
    [SerializeField] public float height;
    [SerializeField] public float widthSpawn;
    [SerializeField] public float platformStep = 2.5f;

    [SerializeField, Header("Состояние игры")] GameObject LosePanel;
    [SerializeField] TextMeshProUGUI LosePanelScoresText;
    [SerializeField] TextMeshProUGUI scoresTMP;
    [SerializeField] TextMeshProUGUI highScoresTMP;
    [SerializeField] AudioSource sound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] AudioClip updateScoreSound;
    [SerializeField] AudioClip newRecordSound;
    [SerializeField] GameObject highScoreLine;
    [SerializeField] ParticleSystem newHighScoreParticles;

    [SerializeField] int scoresPerY = 21;
    public int scores {get; private set;}
    public int highScoresAll;
    public int highScoresGame {get; private set;}
    public Player player {get; private set; }
    bool canLose = true;
    Animator scoresAnim;
    Animator highScoresAnim;
    GameObject currentLine;
    GameObject highScoreCongrutulationText;
    
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
        highScoreCongrutulationText = Resources.Load<GameObject>("Prefabs/floatingCrit");
        scoresAnim = scoresTMP.GetComponent<Animator>();
        highScoresAnim = highScoresTMP.GetComponent<Animator>();
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
        if (currentLine != null) Destroy(currentLine);
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
        currentLine = Instantiate(highScoreLine, new Vector2(0, highScoresAll / scoresPerY), Quaternion.identity);
        LosePanel.SetActive(false);
        YandexGame.FullscreenShow();
    }
    public void CanLose() {
        canLose = true;
    }
    void UpdateScores() {
        scores = (int)player.transform.position.y * scoresPerY;
        if (scores > highScoresGame) 
        {
            highScoresGame = scores;
            scoresTMP.text = highScoresGame.ToString();
            scoresAnim.SetTrigger("Blink" + Random.Range(1, 4));
            PlaySound(updateScoreSound);
        }
        if (highScoresGame > highScoresAll) 
        {
            highScoresAll = highScoresGame;
            highScoresTMP.text = highScoresAll.ToString();
            highScoresAnim.SetTrigger("Blink" + Random.Range(1, 4));
        }
        if (currentLine != null) 
        {
            if (player.transform.position.y > currentLine.transform.position.y) 
            {
                if (newHighScoreParticles != null) Instantiate(newHighScoreParticles, currentLine.transform.position, Quaternion.identity);
                TextMeshPro newCongrutulationText = Instantiate(highScoreCongrutulationText, currentLine.transform.position, Quaternion.identity).GetComponentInChildren<TextMeshPro>();
                PlaySound(newRecordSound, 2.5f);
                PlaySound(newRecordSound, 2.5f);

                switch (YandexGame.EnvironmentData.language) 
                {
                    case "ru": newCongrutulationText.text = "НОВЫЙ РЕКОРД, ПОЗДРАВЛЯЮ!!!!";  break;
                    case "en": newCongrutulationText.text = "A NEW RECORD, CONGRATULATIONS!!!!";  break;
                    case "tr": newCongrutulationText.text = "YENİ REKOR, TEBRİKLER!!!!";  break;
                }
                Destroy(currentLine);
            }
        }
        highScoresTMP.text = highScoresAll.ToString();
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

        if (YandexGame.EnvironmentData.language == "ru") LosePanelScoresText.text = "Очки : " + highScoresGame.ToString();
        else if (YandexGame.EnvironmentData.language == "en") LosePanelScoresText.text = "Scores : " + highScoresGame.ToString();
        else if (YandexGame.EnvironmentData.language == "tr") LosePanelScoresText.text = "Puanlar : " + highScoresGame.ToString();
        if (highScoresGame > highScoresAll) {
            highScoresAll = highScoresGame;
            YandexGame.NewLeaderboardScores("HighScoresUltraSecondVERSION", highScoresAll);
        }
        PlaySound(loseSound, 2);
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

    public void Load() {
        highScoresAll = YandexGame.savesData.HighScores;
        highScoresAll = highScoresGame;
        highScoresTMP.text = highScoresAll.ToString();
    }
    public void Save() {
        YandexGame.savesData.HighScores = highScoresAll;
    }
    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }
    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }
}
