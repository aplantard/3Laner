using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    //General Game

    public PlayerController player;
    public float ThresholdSpeed;
    public float ThresholdSpeedObejects;
    private int score = 0;

    // Road Scroller 
    public Transform CheckAnchor;
    public Transform targetAnchor;
    public Transform RoadDuplicate;
    public Transform Road;

    public float speed;

    public float minDistance;

    private float startingSpeed;

    // Spawn Objects

    public Transform spawnLane1;

    public Transform spawnLane2;
    
    public Transform spawnLane3;

    public GameObject Coins;
    public GameObject oilSpill;
    public float scrollSpeedObject;

    public float positionToDestroy;

    private float time = 0;

    private float startingScrollSpeedObject;

    public static GameManager instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(instance){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        startingScrollSpeedObject = scrollSpeedObject;
        startingSpeed = speed;
    }


    // Update is called once per frame
    void Update()
    {
        spawnObjects();
        objectsScroll();
        RoadScroll();
        if(score % ThresholdSpeed == 0 && score != 0){
            speed = startingSpeed * (score/ThresholdSpeed);
        }
        if(score % ThresholdSpeedObejects == 0 && score != 0){
            scrollSpeedObject = startingScrollSpeedObject * (score/ThresholdSpeedObejects);
        }
    }

    public void playerCollision(Collider2D col){
        if(col.gameObject.CompareTag("Coins")){
            score++;
            UIManager.instance.UpdateScore(score);
            Destroy(col.gameObject);
        }
        else if(col.gameObject.CompareTag("OilSpills")){
            speed = 0;
            scrollSpeedObject = 0; 
            UIManager.instance.Loose(score);
            Destroy(col.gameObject); 
            player.Loose();
        }
    }

    public void Restart(){
        score = 0;
        UIManager.instance.Restart();
        UIManager.instance.UpdateScore(score);
        GameObject[] coinToScroll = GameObject.FindGameObjectsWithTag("Coins");
        GameObject[] spillToScroll = GameObject.FindGameObjectsWithTag("OilSpills");
        GameObject[] objectToScroll = coinToScroll.Concat(spillToScroll).ToArray();
        foreach(GameObject obj in objectToScroll){
            Destroy(obj);
        }
        speed = startingSpeed;
        scrollSpeedObject = startingScrollSpeedObject;
        player.Restart();
    }

    private void spawnObjects(){
        GameObject objectSpawed = null;
        time += Time.deltaTime;
        if(time > 1){
            float spawnpoint = Random.Range(0,3);
            float coinsOrOillSpill = Random.Range(0,2);
            switch(coinsOrOillSpill){
                case 0:
                    switch(spawnpoint){
                        case 0:
                            objectSpawed = Instantiate(Coins, spawnLane1.position, Quaternion.identity);
                            break;
                        case 1:
                            objectSpawed = Instantiate(Coins, spawnLane2.position, Quaternion.identity);
                            break;
                        case 2:
                            objectSpawed = Instantiate(Coins, spawnLane3.position, Quaternion.identity);
                            break;
                        default:
                            break;
                    }
                    break;
                case 1:
                    switch(spawnpoint){
                        case 0:
                            objectSpawed = Instantiate(oilSpill, spawnLane1.position, Quaternion.identity);
                            break;
                        case 1:
                            objectSpawed = Instantiate(oilSpill, spawnLane2.position, Quaternion.identity);
                            break;
                        case 2:
                            objectSpawed = Instantiate(oilSpill, spawnLane3.position, Quaternion.identity);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            time = 0;
        }   
    }

    private void objectsScroll(){
        GameObject[] coinToScroll = GameObject.FindGameObjectsWithTag("Coins");
        GameObject[] spillToScroll = GameObject.FindGameObjectsWithTag("OilSpills");
        GameObject[] objectToScroll = coinToScroll.Concat(spillToScroll).ToArray();
        foreach(GameObject obj in objectToScroll){
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y-Time.deltaTime*scrollSpeedObject, obj.transform.position.z);
            if(obj.transform.position.y <= positionToDestroy){
                Destroy(obj);
            }
        }
    }

    private void RoadScroll(){
        if(Vector3.Distance(Road.position, CheckAnchor.position) < minDistance){
            RoadDuplicate.position = targetAnchor.position;
            
        }
        if(Vector3.Distance(RoadDuplicate.position, CheckAnchor.position) < minDistance){
            Road.position = targetAnchor.position;
        }

        Road.position = new Vector3(Road.position.x,Road.position.y-Time.deltaTime*speed,Road.position.z);
        RoadDuplicate.position = new Vector3(RoadDuplicate.position.x,RoadDuplicate.position.y-Time.deltaTime*speed,RoadDuplicate.position.z);
    }
}
