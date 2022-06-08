using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManagerArrows : MonoBehaviour
{
    // Class with custom getters to efficiently represent Levels
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

        public Level(float[] values){
            vals = values;
        }
    }

    public GameObject canvas;
    public GameObject winnerCanvas;
    public GameObject startCanvas;
    public Text winnerText;
    public Text levelText;
    public AudioSource audioSource;

    private int currLevel;
    private bool isRunning;
    private Vector3 WallPos;
    private Vector3 TargetPos;
    private bool gameOver;
    private Level[] levels = new Level[]{};
    private List<AgentBehaviour> readyChars  = new List<AgentBehaviour>(); 
    // Start is called before the first frame update
    public void Start()
    {
        PlayerPrefs.SetInt("displacement1", 0);
        levels = getLevels();
        audioSource = audioSource.GetComponent<AudioSource>();
        isRunning = false;
        gameOver = false;
        currLevel = 0;
        levelText.text = string.Format("Level {0} \nGravity: {1:0.##}", currLevel + 1, -levels[currLevel].gravity);
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
        List<Level> levels = new List<Level>();
        string[] csvLines = File.ReadAllLines("Assets/Scripts/Game/Levels.csv");
        foreach (string line in csvLines)
        {
            // allows commented lines using the % character
            if (line[0] != '%'){
                float[] vals =(from val in (line.Split(',')) select float.Parse(val)).ToArray();
                levels.Add(new Level(vals));
            } 
        }
        return levels.ToArray();
    }

    private void prepareLevel(){
                Level level = levels[currLevel];
                //vector3d
                Physics.gravity = new Vector3(0,0,level.gravity);

                // Communicates the start to cellulos
                GameObject.FindGameObjectWithTag("Dog").GetComponent<WallAndTarget>()
                .setLevel(false, level.activeWall, level.movingWall, level.wallPos, level.wallPosMove);

                GameObject.FindGameObjectWithTag("Sheep").GetComponent<WallAndTarget>()
                .setLevel(true, false, level.movingTarget, level.targetPos, level.targetPosMove);
                
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().prepareLevel();
                levelText.text = string.Format("Level {0} \nGravity: {1:0.##}", currLevel + 1, -levels[currLevel].gravity);
    }

    public void startGame(){
        if(!isRunning){
            isRunning = true;
            startCanvas.SetActive(false);
        }
        currLevel = 0;
        prepareLevel();
    }

    public bool isGameRunning(){
        return isRunning;
    }

    public void resetLevel(){
        prepareLevel();
    }

    public void nextLevel(){
        currLevel +=1;
        if (currLevel >= levels.Length){
            gameOverMode();
        }
        prepareLevel();
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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        winnerText.text = string.Format("You scored {0}", player.GetComponent<PlayerControl>().getScore().ToString());
        winnerCanvas.SetActive(true);
    }

    public void loadMenu(){
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    // Called by each cellulos when it is at the right place and ready to start
    public void imready(AgentBehaviour character){
        bool alreadyPresent = false;
        foreach (AgentBehaviour charact in readyChars){
            if (character == charact){
                alreadyPresent = true;
            }
        }
        if (!alreadyPresent){
            readyChars.Add(character);
        }
        if (readyChars.Count == 3){
            readyChars.Clear();
            // Start the level
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().startGame();
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
