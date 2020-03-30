using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject sky, player, deathScreen;
    public GameObject[] levels;
    public Text scoreboard;
    int lastSpawn, currentSpawn, currentX, score;
    GameObject spawningLevel;
    float timeExpired, shrineChance;
    // Start is called before the first frame update
    void Start()
    {
        timeExpired = 0;
        shrineChance = Random.Range(0F, 1F);
        currentX = 160;
    }

    // Update is called once per frame
    void Update()
    {   
        if(timeExpired * 2 >= 104)
        {
            player.GetComponent<VampScript>().enabled = false;
            player.GetComponent<Rigidbody2D>().simulated = false;
            deathScreen.SetActive(true);
        } else
        {
            timeExpired += Time.deltaTime;
            score++;
            sky.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90 - (timeExpired * 2)));
            scoreboard.text = score.ToString();
        }

    }

    public void ShrineDesecrated()
    {
        timeExpired -= 10F;
    }

    public void spawnNextLevel()
    {
        while (true) {
            currentSpawn = Random.Range(0, levels.Length);
            if (currentSpawn != lastSpawn) break;
        }
        spawningLevel = GameObject.Instantiate(levels[currentSpawn], new Vector3(currentX, 0, 0), Quaternion.Euler(Vector3.zero));
        currentX += 40;
        lastSpawn = currentSpawn;
        if (spawningLevel.GetComponent<LevelSpawnOptions>().spawnShrine(shrineChance))
        {
            shrineChance = Random.Range(0F, 1F);
        }
        else
        {
            shrineChance += 0.2F;
        }
    }

    public void DayWalkerDown()
    {
        score += 300;
    }

    public void Reset()
    {
        print("I want to break free");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
