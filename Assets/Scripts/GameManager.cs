using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.iOS;

public class GameManager : MonoBehaviour
{

    // Debug
    [SerializeField] bool debugPause = false;
    public AbilityHolder DEBUGAbility;
    
    // Components
    [field: SerializeField] public GameObject player { get; private set; }
    [field: SerializeField] public Camera cam { get; private set;}
    public PlayerGameFunctions _playerGameFunctions {get; private set;}
    public FirstPersonController _firstPersonController {get; private set;}
    [SerializeField] LayerMask GroundLayers;

    // Public Properties
    public Vector3 _playerPos {get; private set;}
    public bool _paused {get; private set;}
    public float timeElapsed {get; private set;}

    public float initPlayerXp = 0;
    public float initPlayerLvlThresh = 10;
    public float _playerXp {get; private set;}
    public float _playerLvlXpThresh {get; private set;} // xp to get to next level
    public int _playerLvl {get; private set;}

    public static Action levelUpEvent;
    public AbilityUtilities abilityUtilities;

    // ~~ Private Properties ~~
    // -- Game properties --
    private float timeSinceLastSpawned, timeSinceLastRateIncrease = 0f;
    private float spawnInterval = 2f;   // time before spawning new monsters
    private int spawnIncreaseInterval = 10; // time before increasing spawn rate
    private int spawnIncreaseDelta = 1; // how much to increase spawns
    private int enemiesToSpawn = 1;
    private EnemyPooler enemyPooler;
    private int areaBounds = 120;

    // static
    public static float _MOB_HEIGHT = 1;
    public static GameManager instance {get; private set;}

    void Start(){
        if(instance != null && instance != this){
            Destroy(this);
        }
        else {
            instance = this;
        }

        enemyPooler = ObjectPoolManager.instance.GetPoolByType<EnemyPooler>(PoolType.enemy);
        _playerGameFunctions = player.GetComponent<PlayerGameFunctions>();
        _firstPersonController = player.GetComponent<FirstPersonController>();

        // Initialize properties
        _playerXp = initPlayerXp;
        _playerLvlXpThresh = initPlayerLvlThresh;
        timeElapsed = 0f;
        GetSpawnPoints();
    }

    void Update(){

        // Debug
        if(debugPause) _paused = debugPause;
        
        // Check Pause
        Time.timeScale = _paused ? 0 : 1;

        // Increase Spawn Rate
        // if((int)(timeElapsed/spawnIncreaseInterval) == 1){
        if(timeSinceLastRateIncrease > spawnIncreaseInterval){
            
            enemiesToSpawn += spawnIncreaseDelta;
            timeSinceLastRateIncrease = 0;
        }

        // Spawn Timer
        if(timeSinceLastSpawned > spawnInterval){
            timeSinceLastSpawned = 0;
            SpawnEnemies(enemiesToSpawn);
            // SpawnEnemiesAllPoints();
        }

        

        // Loop player around area
        LoopPlayerPosition();

        // Property Updates
        _playerPos = player.transform.position;
        timeElapsed += Time.deltaTime;
        timeSinceLastSpawned += Time.deltaTime;
        timeSinceLastRateIncrease += Time.deltaTime;
    }

    void SpawnEnemies(int amount){
        Vector3[] spawnPoints = GetSpawnPoints();
        
        for(int i = 0; i < amount; i++){
            int index = UnityEngine.Random.Range(0, spawnPoints.Length);
            BaseEnemyScript newEnemy = enemyPooler.Get();
            newEnemy.transform.position = spawnPoints[index];
        }
    }

    void SpawnEnemiesAllPoints(){
        // Debug for checking spawn points

        Vector3[] spawnPoints = GetSpawnPoints();
        foreach(Vector3 point in spawnPoints){
            BaseEnemyScript newEnemy = enemyPooler.Get();
            newEnemy.transform.position = point;
        }
    }

    Vector3[] GetSpawnPoints(){
        // x(t) = r cos(t) + j
        // y(t) = r sin(t) + k
        // (j, k) is the origin
        // t is the degree from 0 to 360
        // r is radius
        // step = 360 / # of points
        // Vector3 worldCentrePoint = ScreenCentrePointToWorld();

        int points = 12;
        float step = 360f/points;
        float j = _playerPos.x;
        float k = _playerPos.z;
        float r = 25;
        
        float t = 0;
        float xPoint;
        float yPoint;

        Vector3[] listOfPoints = new Vector3[points];

        for(int i = 0; i < points; i++){
            float degreeInRadian = t * Mathf.Deg2Rad;       

            xPoint = (r * Mathf.Cos(degreeInRadian)) + j;
            yPoint = (r * Mathf.Sin(degreeInRadian)) + k;

            t += step;

            listOfPoints[i] = new Vector3(xPoint, _MOB_HEIGHT, yPoint);
        }

        return listOfPoints;
    }

    void LoopPlayerPosition(){
        // loop player around world when reaching a border
        if(_playerPos.x > areaBounds){
            _firstPersonController.WarpToPosition(new Vector3(-areaBounds, _playerPos.y, _playerPos.z));

        }
        if(_playerPos.x < -areaBounds){
            _firstPersonController.WarpToPosition(new Vector3(areaBounds, _playerPos.y, _playerPos.z));

        }
        if(_playerPos.z < -areaBounds){
            _firstPersonController.WarpToPosition(new Vector3(_playerPos.x, _playerPos.y, areaBounds));

        }
        if(_playerPos.z > areaBounds){
            _firstPersonController.WarpToPosition(new Vector3(_playerPos.x, _playerPos.y, -areaBounds));

        }
    }

    /*
    IEnumerator SpawnEnemyTimer(){

        yield return new WaitForSeconds(10f);

        while(true){

            // BaseEnemyScript newEnemy = enemyPooler.Get();
   
            // newEnemy.transform.position = _screenToWorldCentrePoint;

            GetSpawnPoints();


            yield return new WaitForSeconds(5f);
        }

        //yield return null;

    }
    */

    /*
    Vector3 ScreenCentrePointToWorld(){
        Ray cameraRay = cameraMain.ScreenPointToRay(new Vector3((cameraMain.pixelWidth - 1)/2f, (cameraMain.pixelHeight - 1)/2f, 0));
        RaycastHit cameraCastHit = new RaycastHit();
        Physics.Raycast(cameraRay, out cameraCastHit);

        // _screenToWorldCentrePoint = cameraCastHit.point;
        return cameraCastHit.point;
    }
    */

    public void GainXP(float _xp) {
        _playerXp += _xp;
        if(_playerXp >= _playerLvlXpThresh){
            LevelUp();
        }

    }

    private void LevelUp(){
        float leftOverXp = _playerXp - _playerLvlXpThresh;
        if(leftOverXp < 0) {
            Debug.Log("Level Up Error: tried to level up but not enough XP");
            return;
        }

        _playerXp = leftOverXp;
        _playerLvl++;
        _playerLvlXpThresh *= 1.1f;

        levelUpEvent.Invoke();

        // Debug.Log("Level UP");
        // DEBUGAbility.LevelUpAbility();
    }

    

    public void PauseGame(){
        _paused = true;
    }

    public void TogglePause(){
        _paused = !_paused; 
    }

    public Vector3 ScreenToWorldPos(Vector2 screenPos){
        Ray temp = cam.ScreenPointToRay(new Vector3(screenPos.x, screenPos.y, 0));
		RaycastHit cameraCastHit;
		Physics.Raycast(temp, out cameraCastHit, Mathf.Infinity, GroundLayers);

        return cameraCastHit.point;
    }
}
