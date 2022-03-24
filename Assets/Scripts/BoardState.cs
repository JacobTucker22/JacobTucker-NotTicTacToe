using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

//ai utility class containing simple board state representation
//opponent is the ai. player is the human player
public class BoardState
{

     public int[] allPits;
     //unsure if the rest is needed
     public int[] playerPits;
     public int[] opponentPits;
     public int playerScore;
     public int opponentScore;

     //create boardstate from a pitmanager
     //board state is a 1d array length 14, first 7 are ai pits last 7 are player pits. 
     /*
      *  aiPits
      *  4 - 4 - 4 - 4 - 4 - 4 - 0 (last one is score pit) player pits 4 - 4 - 4 - 4 - 4 - 4 - 0 (last one is score pit)
      *  
      *  player pits
      *  
      *  numbers represent the current number of stones in each pit
      *  counter-clocwise motion through physical board is represented by 
      *  left to right, iteration from ai perspecitve of board
     */

     public BoardState()
     {

     }
     public BoardState(PitMgr newState)
     {
          allPits = new int[14];
          //populate all pits in array
          for(int i = 0; i < 14; i++)
          {
               allPits[i] = newState.pits[i].pitStones.Count;

          }
          //populate player and ai pits (may not need)
/*          for (int i = 0; i < 7; i++)
          {
               aiPits[i] = newState.OpponentPits[i].pitStones.Count;
               playerPits[i] = newState.PlayerPits[i].pitStones.Count;
          }*/

          playerScore = allPits[6];
          opponentScore = allPits[13];

     }
     //copy constructor
     //(not working with current setup)
     public BoardState(BoardState oldBoard)
     {
          if(oldBoard == null)
          {
               return;
          }
          allPits = new int[14];
          for (int i = 0; i < oldBoard.allPits.Length; i++)
          {
               allPits[i] = oldBoard.allPits[i];
          }

     }

     //debug print boardstate array to console
     public void printBoard()
     {
          Debug.Log("BS = " + string.Join("",
                      new List<int>(allPits)
                      .ConvertAll(i => i.ToString())
                      .ToArray()));
          /*          for(int i = 0; i < 7; i++)
                    {
                         Debug.Log(allPits[i].ToString() + ' ');
                    }

                    for (int i = 0; i < 7; i++)
                    {
                         Debug.Log(allPits[7 + i].ToString() + ' ');
                    }*/
     }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
