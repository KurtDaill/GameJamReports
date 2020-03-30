using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateHandler : MonoBehaviour
{
    //Time is measured in seconds
    float timer, endTimer;
    //Speeds up spawn chance, min spawn time, and max spawn time
    public float uptickSpeed, minTimeAccel, maxTimeAccel, spawnChance, minSpawnTime, maxSpawnTime;
    float spawnChanceD, minSpawnTimeD, maxSpawnTimeD;
    int score;
    public GameObject dopplePrefab;
    GameObject currentSpawn;
    bool activeGame, endingGame;
    public CanvasGroup ui;
    public Text scoreboard;
    public AudioSource bump, amb;
    // Start is called before the first frame update
    void Start()
    {
        activeGame = false;
        amb.Play();
        endTimer = 0;
        spawnChanceD = spawnChance;
        minSpawnTimeD = minSpawnTime;
        maxSpawnTimeD = maxSpawnTime;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeGame == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Dopple") Destroy(hit.collider.gameObject);
                    bump.Play();
                    score++;
                }
            }

            timer += Time.deltaTime;
            spawnChance *= 1 + (uptickSpeed * Time.deltaTime);
            if (spawnChance >= 1) spawnChance = 1;
            minSpawnTime -= minTimeAccel * Time.deltaTime;
            if (minSpawnTime <= 0.1) minSpawnTime = 0.05F;
            maxSpawnTime -= maxTimeAccel * Time.deltaTime;
            if (maxSpawnTime <= 1) maxSpawnTime = 0.5F;

            if (timer >= minSpawnTime)
            {
                if (timer >= maxSpawnTime)
                {
                    SpawnDopple();
                }
                else if (Random.Range(0, 1) > spawnChance)
                {
                    SpawnDopple();
                }

            }
        }
        else if (endingGame == true)
        {
            endTimer += Time.deltaTime;
            if (endTimer > 1F)
            {
                foreach (GameObject dopple in GameObject.FindGameObjectsWithTag("Dopple")) Destroy(dopple);
                ui.blocksRaycasts = true;
                endTimer = 0;
                endingGame = false;
                maxSpawnTime = maxSpawnTimeD;
                minSpawnTime = minSpawnTimeD;
                spawnChance = spawnChanceD;

            }
        }
    }

    private void SpawnDopple()
    {
        currentSpawn = Instantiate(dopplePrefab, new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), -2), Quaternion.Euler(Vector3.zero));
        currentSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0, 1, 0.2F, 1, 0.2F, 1);
        bump.Play();
        timer = 0;
    }

    public void StartGame()
    {
        score = 0;
        scoreboard.text = "";
        activeGame = true;
        amb.Stop();
        ui.alpha = 0;
        ui.blocksRaycasts = false;
        SpawnDopple();
    }

    public void EndGame()
    {
        if (endTimer == 0)
        {
            endingGame = true;
            activeGame = false;
            ui.alpha = 1;
            scoreboard.text += score;
        }
    }

    public void GoStupidGoCrazy()
    {
        StartGame();
        minSpawnTime = 0;
        maxSpawnTime = 0;
        spawnChance = 1;
        print("Go Stupid, Go Crazy");
    }
}
