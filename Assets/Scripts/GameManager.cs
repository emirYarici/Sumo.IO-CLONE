using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timer;
    public GameObject enemyPrefab;
    public List<GameObject> enemyList;
    public List<GameObject> powerUpObjectsList;
    public GameObject scaleUpPrefab;
    public Transform leftBound;
    public Transform rightBound;
    public Transform topBound;
    public Transform bottomBound;
    public UIManager UIScript;
    public static GameManager Instance;
    private int _score;
    public GameObject speedUpObjectPrefab;
    public bool gameStarted = false;
    public GameObject playerObject;
    public List<GameObject> spawnPossesList;
    public List<GameObject> powerUpSpawnPossesList;
    //update the score everytime score changes by using get and set methods
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            UIScript.ScoreText.gameObject.transform.DOPunchScale(Vector3.one, 0.5f, 1);
            UIScript.ScoreText.text = _score.ToString();
        }
    }
    private int _peopleLeft;
    public int peopleLeft
    {
        get
        {
            return _peopleLeft;
        }
        set
        {
            _peopleLeft = value;
            UIScript.peopleLeftText.gameObject.transform.DOPunchScale(Vector3.one, 0.5f, 1);
            UIScript.peopleLeftText.text = _peopleLeft.ToString();
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //generate level 
        SpawnEnemy();
        SpawnPowerUp();
        //spawn speed up every 15 sec
        InvokeRepeating("SpawnSpeedUp", 0f, 15);

    }

    // Update is called once per frame
    void Update()
    {
        //count time,and update timer text
        if (gameStarted == true)
        {
            timer -= Time.deltaTime;
            UIScript.timerText.text = "0:" + ((int)(timer)).ToString();
            if (timer < 0)
            {
                timer = 0;
                GameLose();
            }
        }
    }
    public void SpawnPowerUp()
    {
        //spawn and add these objects to a list
        int scaleUpAmount = Random.Range(5, 7);
        for (int counter = 0; counter < scaleUpAmount; counter++)
        {
            GameObject temp = Instantiate(scaleUpPrefab, powerUpSpawnPossesList[counter].transform.position, scaleUpPrefab.transform.rotation);
            powerUpObjectsList.Add(temp);
        }
    }
    public void SpawnEnemy()
    {
        //spawn and add these objects to a list
        peopleLeft = 6;
        for (int counter = 0; counter < peopleLeft; counter++)
        {
           GameObject temp =  Instantiate(enemyPrefab, spawnPossesList[counter].transform.position, Quaternion.identity);
            enemyList.Add(temp);
        }
    }
    public void GameEnd()
    {
        if(enemyList.Count == 0)
        {
            GameWin();
        }
    }
    public void SpawnSpeedUp()
    {
        GameObject temp = Instantiate(speedUpObjectPrefab, GetRandomPosInArea(), speedUpObjectPrefab.transform.rotation);
        powerUpObjectsList.Add(temp);
    }

    //get Random position between the bounds of the game area
    public Vector3 GetRandomPosInArea()
    {
        float randomXPos = Random.Range(leftBound.transform.position.x + 1, rightBound.transform.position.x - 1);
        float randomZPos = Random.Range(topBound.transform.position.z - 1, bottomBound.transform.position.y + 1);
        return new Vector3(randomXPos, 0, randomZPos);
    }
    //Instantiate power ups if there is no left 
    public void ControlScaleUpAmount()
    {
        if (powerUpObjectsList.Count == 0)
        {
            SpawnPowerUp();
        }
    }

    public void GameWin()
    {
        gameStarted = false;
        UIScript.InGameScreen.SetActive(false);
        UIScript.winScreen.SetActive(true); 
    }
    public void GameLose()
    {
        gameStarted = false;
        UIScript.InGameScreen.SetActive(false);
        UIScript.loseScreen.SetActive(true);
    }
   
}
