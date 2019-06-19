using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public Canvas canvas;
    public GameObject LooseScreen;

    private Text ScoreText;
   

    void Awake()
    {
        if(instance){
            Destroy(gameObject);
        }
        else{
            instance = this;
        };
    }

    void Start(){
        ScoreText = canvas.GetComponentInChildren<Text>();
    }

    public void UpdateScore(int newScore){
        ScoreText.text = "Score : "+newScore;
    }

    public void Loose(int finalScore){
        LooseScreen.SetActive(true);
        ScoreText.gameObject.SetActive(false);
        LooseScreen.transform.Find("ScoreLoose").gameObject.GetComponent<Text>().text = "You Scored : "+finalScore;
    }

    public void Restart(){
        LooseScreen.SetActive(false);
        ScoreText.gameObject.SetActive(true);
    }
}
