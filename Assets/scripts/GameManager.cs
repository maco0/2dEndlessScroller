using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
   //gameplay logic variables
    float Timer;
    float DelayTimer = 4f;
    public int point;
    public Text HighScore;
    public Text DeployScore;
    public Text YourScore;
    int startPoint = 0;
    int prevScore = 0;
    //panels and gameobject transforms
    public static GameManager instance;
    public Transform[] SpawnPos;
    public GameObject[] ObstacleCoin;
    public GameObject StartCanvas;
    public GameObject playCanvas;
    public GameObject EndCanvas;
    public GameObject car;
    public GameObject StartTeaching;
    //gamemodes
    public enum GameMode { Start, Playing, End };
    public GameMode mode;


    //audio Components
    public AudioSource CoinCollect;
    public AudioSource GameDifficultyAudio;
    public AudioSource ObstacleCollide;
    public AudioSource GameSound;
    public AudioSource buttonCLick;

    public Animator SoundButton;
    public Animator musicButton;
    bool SongMuted=false;
    bool SoundMuted=false;
    
    void awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
       
        mode = GameMode.Start;

      HighScore.text= "Best Score: "+ PlayerPrefs.GetInt("HighScore").ToString();
        Timer = DelayTimer;

    }


    void Update()
    {
        //switching between the game states
       
        switch (mode)
        {
            //only setting active StartCanvas, toggling between sound and music button
            case GameMode.Start:
                EndCanvas.SetActive(false);
                car.SetActive(false);

                StartCanvas.SetActive(true);
                ToggleButtonAnimation(SongMuted, SoundButton);

                ToggleButtonAnimation(SoundMuted, musicButton);
                break;

            // only setting playcanvas active
            case GameMode.Playing:

                StartCanvas.SetActive(false);
                EndCanvas.SetActive(false);

                playCanvas.SetActive(true);
               //starting the spawn delay
                spawnDelay();
                //enabling gameplayUpdate
                GameplayUpdate();
                //proving logic for endgame
                EndGame();
                break;
            case GameMode.End:
                YourScore.text = "Your Score: " + point.ToString();
               //comparing current score with previous score, if current score is higher, then changing best score with current score
                if(point> prevScore)
                {
                    prevScore = point;
                   
                    PlayerPrefs.SetInt("Highscore", point);
                    HighScore.text = "Best Score: " + PlayerPrefs.GetInt("HighScore", point).ToString();
                    //saving current best score
                    PlayerPrefs.Save();
                }
                

                EndCanvas.SetActive(true);
                playCanvas.SetActive(false);
                break; 
        }   
    }

    #region GAMEPLAY LOGIC

    //delay of the obscatle/coins spawned
    void spawnDelay() {
        Timer -= Time.deltaTime;
       
        if (Timer <= 0)
        {
            Spawn();
            
            Timer = DelayTimer;
            
        }
    }

    //updating the difficulty 
    public void GameplayUpdate()
    {
        
        if (startPoint == 5)
        {
            GameDifficultyAudio.Play();
           
            backgroundRepeat.instance.speed+=2;
            
            startPoint = 0;
        }
    }
   
    //spawning coin/obstacle at random position with random method
    void Spawn()
    {
        int index = Random.Range(0, ObstacleCoin.Length);
        int SpawnIndex = Random.Range(0, SpawnPos.Length);
        Instantiate(ObstacleCoin[index], SpawnPos[SpawnIndex].position, Quaternion.identity);
        
    }

    //setting game mode on End, disabling car and stopping the road
    public void EndGame()
    {
        if (CarTouch.instance.Hit)
        {
            mode = GameMode.End;
            car.SetActive(false);
            backgroundRepeat.instance.Canrepeat = false;
            ObstacleCollide.Play();
        }
    }

    //deploying and updating the score
    public void UpdateScore(int amount)
    {
        CoinCollect.Play();
        point += amount;
        //startPoint counts the number of points we are getting while playing to update game difficulty
        startPoint++;
        DeployScore.text = "SCORE " + point.ToString();
    }

    #endregion


    #region Button Behaviours
    //BUTTON CLICK BEHAVIOURS

    //button click on starting a game
    public void startGame()
    {
        buttonCLick.Play();
        //seting game mode on playing
        mode = GameMode.Playing;
        //starting coroutine of teching animation and activating the car
        StartCoroutine(StartTeachingDelay());
        //resetting the points aswell as car/road speed
            point = 0;
            startPoint = 0;
            DeployScore.text = "SCORE " + point.ToString();
           
            car.transform.position = new Vector3(0, -1, 0);
            backgroundRepeat.instance.Canrepeat = true;
            backgroundRepeat.instance.speed = backgroundRepeat.instance.BaseSpeed;
            CarTouch.instance.speed = 2;
            CarTouch.instance.Hit = false;
        

    }

    //switching game mode on start
    public void MainMenu()
    {
        buttonCLick.Play();
        mode = GameMode.Start;

    }

    #endregion
    //disabling the volume sounds 
    public void DisableGameSounds()
    {
        SoundMuted = !SoundMuted;
        buttonCLick.Play();
        GameSound.enabled = !GameSound.enabled;
       
        ToggleButtonAnimation(SoundMuted, musicButton);
    }
   //disabling the InGameSounds
    public void DisableInGameSounds()
    {
        buttonCLick.Play();
        buttonCLick.enabled = !buttonCLick.enabled;
        CoinCollect.enabled = !CoinCollect.enabled;
        GameDifficultyAudio.enabled = !GameDifficultyAudio.enabled;
        ObstacleCollide.enabled = !ObstacleCollide.enabled;

        SongMuted = !SongMuted;
        ToggleButtonAnimation(SongMuted, SoundButton);
    }

    //switching between activebutton and inactive button
    public void ToggleButtonAnimation(bool muteState, Animator anim)
    {
        if (muteState)
        {
            anim.SetBool("pressed", true);
           
        }
        else
        {
            anim.SetBool("pressed", false);

        }

    }

    //ienumerator of the startAnimation
    IEnumerator StartTeachingDelay()
    {
        
        StartTeaching.SetActive(true);
        yield return new WaitForSeconds(1);
        StartTeaching.SetActive(false);
        car.SetActive(true);

    }
   
}
