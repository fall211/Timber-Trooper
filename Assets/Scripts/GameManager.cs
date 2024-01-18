using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int score = 0;
    public int maxEnemies = 10;

    private GameObject player;
    [HideInInspector] public List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject controlsPanel;
    public GameObject resetButtonContainer;
    private void LateUpdate(){
        scoreText.text = $"Score: {score}";
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start(){
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count < maxEnemies){
            SpawnEnemy(score);
        }
        maxEnemies = 10 + score / 3;
        maxEnemies = Mathf.Min(maxEnemies, 100);

        if (Input.GetKeyDown(KeyCode.Escape)){
            controlsPanel.gameObject.SetActive(!controlsPanel.gameObject.activeSelf);
        }
    }

    private void SpawnEnemy(int score){
        // spawn enemies around the player
        Vector2 spawnLocation = Random.insideUnitCircle * 10f;
        spawnLocation += new Vector2(player.transform.position.x, player.transform.position.z);
        Vector3 spawnLocation3D;
        Ray ray = new(new Vector3(spawnLocation.x, 25f, spawnLocation.y), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f)){
            spawnLocation3D = hit.point;
            var enemy = Instantiate(EnemyPrefab, spawnLocation3D, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().SetHealth(score);
            enemies.Add(enemy);
        }
    }
    public void AddScore(int amount){
        score += amount;
    }

    public void ResetGame(){
        score = 0;
        foreach (var enemy in enemies){
            Destroy(enemy);
        }
        enemies.Clear();
        player.GetComponent<Player>().ResetPlayer();

        audioSource.Play();
    }

    public void ShowResetButton(){
        resetButtonContainer.SetActive(true);
    }
    public void HideResetButton(){
        resetButtonContainer.SetActive(false);
    }
}
