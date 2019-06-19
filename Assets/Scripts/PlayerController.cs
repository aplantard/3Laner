using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float laneDistance = 1.5f; 

    public float speed = 0.1f;

    public int desiredLane = 1;
    private Vector2 Transform2D;
    private bool moving = false;

    private bool gameLost = false;

    // Update is called once per frame
    void Update()
    {   
        Transform2D = new Vector2(transform.position.x,transform.position.y);
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q)){
            MoveLane(false);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)){
            MoveLane(true);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        GameManager.instance.playerCollision(col);
    }

    public void Loose(){
        this.moving = true;
        this.gameLost = true;
    }

    public void Restart(){
        this.moving = false;
        this.desiredLane = 1;
        transform.position = new Vector3(0.1f,-3f,-1f);
        this.gameLost = false;
    }

    private void MoveLane(bool goingRight){
        if(!moving){
            bool authorizedMove = true;
            desiredLane += (goingRight) ? 1 : -1;
            if(desiredLane > 2 || desiredLane < 0){
                authorizedMove = false;
            }
            desiredLane = Mathf.Clamp(desiredLane, 0, 2);
            if(authorizedMove){
                Vector2 targetPosition = Transform2D;
                switch (desiredLane){
                    case 0:
                        targetPosition += Vector2.left*laneDistance;
                        break;
                    case 2:
                        targetPosition += Vector2.right*laneDistance;
                        break;
                    case 1:
                        if(goingRight){
                            targetPosition += Vector2.right*laneDistance;
                        }
                        else{
                            targetPosition += Vector2.left*laneDistance;
                        }
                        break;
                    default:
                        break;
                }
                
                StartCoroutine(AnimateMove(Transform2D,targetPosition));
            }
        }
    }
    IEnumerator AnimateMove(Vector2 origin, Vector2 target)
    {
        moving = true;
        float duration = speed*laneDistance;
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);
            
            transform.position = Vector2.Lerp(origin, target, percent);
            
            yield return null;
        }
        if(!gameLost){
            moving = false;
        }
    }
}