using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerArrows : MonoBehaviour
{
    private class Level{
        // cf. https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties
        private float[] vals;

        public float gravity{get{
            return vals[0];
        }}
        public bool movingTarget{get{
            return vals[1] > 0; 
        }}
        public Vector3 targetPos{get{
            return new Vector3(vals[2], 0, vals[3]);
        }}
        public Vector3 targetPosMove{get{
            return new Vector3(vals[4], 0, vals[5]);
        }}
        public bool activeWall{get{
            return vals[6] > 0;
        }}
        public bool movingWall{get{
            return vals[7] > 0;
        }}
        public Vector3 wallPos{get{
            return new Vector3(vals[8], 0, vals[9]);
        }}
        public Vector3 wallPosMove{get{
            return new Vector3(vals[10], 0, vals[11]);
        }}

        public Level(float values){
            vals = values;
        }
    }

    public GameObject canvas;
    public GameObject winnerCanvas;
    
    public GameObject startCanvas;

    public Text text;

    private int currLevel;
    public AudioSource audioSource;
    private bool isRunning;
    private Vector3 WallPos;
    private Vector3 TargetPos;
    private bool gameOver;
    private Level[] levels = new Level[]{};
    private AgentBehaviour[] readyChars  = new AgentBehaviour[]{}; 
    // Start is called before the first frame update
    public void Start()
    {
        levels = getLevels();
        audioSource = audioSource.GetComponent<AudioSource>();
        isRunning = false;
        gameOver = false;
        currLevel = 1;
        if (PlayerPrefs.GetInt("MusicEnable", 1) == 1){
            audioSource.enabled = true;
            audioSource.volume = PlayerPrefs.GetFloat("Volume", audioSource.volume);
        } else {
            audioSource.enabled = false;
        }
        
        winnerCanvas.SetActive(false);
        canvas.SetActive(true);
        startCanvas.SetActive(true);
    }

    /* Reads the CSV file and add corresponding levels
        for Boolean values, positive = true
    */
    private Level[] getLevels(){
        string[] csvLines = File.ReadAllLines("Levels.csv");
        foreach (string line in lines)
        {
            // allows commented lines using the % character
            if (line[0] != '%'){
                float[] vals = from val in line.Split(",") select float.Parse(val);
                Level level = new Level(vals);
                levels.Add(vals);
            }
        }


    }

// TODO: not finished
    public void prepareLevel(){
                Level level = levels[currLevel];
                Physics.gravity.z = vals.gravity;
                FindGameObjectsWithTag("Dog").GetComponent<WallAndTarget>()
                .setLevel();
                FindGameObjectsWithTag("Sheep").GetComponent<WallAndTarget>()
                .setLevel(false, vals)

    }

    public void startGame(){
        if(!isRunning){
            isRunning = true;
            startCanvas.SetActive(false);
        }
        currLevel = 1;
        prepareLevel(currLevel);
    }

    public bool isGameRunning(){
        return isRunning;
    }

    public void nextLevel(){
        currLevel +=1;
        setLevels(currLevel);
        // update the targets and movement of the cellulos
    }
    public int getLevel(){
        return currLevel;
    }

    public bool gameIsOver(){
        return gameOver;
    }
    public void gameOverMode(){
        gameOver = true;
        isRunning = false;
        canvas.SetActive(false);

        GameObject[] dogs = GameObject.FindGameObjectsWithTag("Dog");
        GameObject winner = dogs[0];

        foreach(GameObject dog in dogs){
            if(dog.GetComponent<MoveWithKeyboardBehavior>().getScore() > winner.GetComponent<MoveWithKeyboardBehavior>().getScore()){
                winner = dog;
            }
        }

        text.text = string.Format("{0} won the game", winner.GetComponent<MoveWithKeyboardBehavior>().name);
        winnerCanvas.SetActive(true);
    }

    public void loadMenu(){
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void setLevels(int levelNumber){
       PlayerPrefs.SetString("Level1", "level"+levelNumber);

    }

    // Called by each cellulos when it is at the right place and ready to start
    public void imready(AgentBehaviour character){
        bool alreadyPresent = false;
        foreach (AgentBehaviour char in readyChars){
            if (character == char){
                alreadyPresent = true;
            }
        }
        if (!alreadyPresent) {
            readyChars.Add(character);
        }
        if (readyChars.Length == 3){
            readyChars.Clear();
            startGame();
        }
    }

    public Vector3 nextWallPosition(){

        //Default before debug
        return WallPos;
    }

    
    public Vector3 nextTargetPosition(){

        //Default before debug
        return TargetPos;
    }
}
