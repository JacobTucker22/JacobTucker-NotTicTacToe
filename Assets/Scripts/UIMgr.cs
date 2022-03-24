using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{

     public static UIMgr inst;
     public PitMgr mPitMgr;
     public MiniMaxAI mAIMgr;
     public GameMgr mGameMgr;

     public Text playerScore, opponentScore;
     public Text numOfPlytext, doubleTurnPreftext;
     public GameObject PlayerWinUI, OpponentWinUI;
     public Slider plySlider, dTSlider;
     public List<Text> playerPitText, opponentPitText;

     public GameObject playerTurn, enemyTurn;

     public Button openMenu, closeMenu;
     public Button restartBtn, quiBtn;
     public Button openControlBtn, openRulesBtn, controlBack, rulesBack;
     public GameObject Menu;
     public GameObject controls, rules, pitKeyMap;

     private void Awake()
     {
          inst = this;
     }

     // Start is called before the first frame update
     void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          playerScore.text = mPitMgr.PlayerPits[6].pitStones.Count.ToString();
          opponentScore.text = mPitMgr.OpponentPits[6].pitStones.Count.ToString();
          numOfPlytext.text = plySlider.value.ToString();
          doubleTurnPreftext.text = dTSlider.value.ToString();
          mAIMgr.ply = (int)plySlider.value;
          mAIMgr.doubleMovePreference = (int)dTSlider.value;

          for(int i = 0; i < playerPitText.Count; i++)
          {
               playerPitText[i].text = mPitMgr.PlayerPits[i].pitStones.Count.ToString();
               opponentPitText[i].text = mPitMgr.OpponentPits[i].pitStones.Count.ToString();
          }

          playerTurn.SetActive(mGameMgr.playerTurn);
          enemyTurn.SetActive(!playerTurn.activeSelf);

          int winCheck = mPitMgr.CheckForWin();
          if(winCheck == 1)
          {
               PlayerWinUI.SetActive(true);
          }
          else if(winCheck == 2)
          {
               OpponentWinUI.SetActive(true);
          }

     }

     public void WinUI()
     {

     }


     public void OpenMenu()
     {
          mGameMgr.canMove = false;
          Menu.SetActive(true);
          pitKeyMap.SetActive(false);
     }

     public void CloseMenu()
     {
          mGameMgr.canMove = true;
          Menu.SetActive(false);
          pitKeyMap.SetActive(true);
     }

     public void RestartGame()
     {
          mPitMgr.resetPits();
          mGameMgr.playerTurn = true;
          Menu.SetActive(false);
          mGameMgr.canMove = true;
          PlayerWinUI.SetActive(false);
          OpponentWinUI.SetActive(false);
          pitKeyMap.SetActive(true);
     }
     public void QuitGame()
     {
          #if UNITY_EDITOR
          UnityEditor.EditorApplication.isPlaying = false;
          #endif
          Application.Quit();
     }

     public void OpenControls()
     {
          controls.SetActive(true);
     }

     public void CloseControls()
     {
          controls.SetActive(false);
     }

     public void OpenRules()
     {
          rules.SetActive(true);
     }

     public void CloseRules()
     {
          rules.SetActive(false);
     }

}

