using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{

     public static GameMgr inst;
     public PitMgr myPitMgr;
     public MiniMaxAI myAiMgr;
     public bool playerTurn = true;
     public bool canMove = true;
     public bool canAIMove = false;
     public int winCondition = 0;
     public int winNumber = 0;

     private void Awake()
     {
          inst = this;
     }

     // Update is called once per frame
     void Update()
     {
          InputContols();


     }

     //Make a move based on a pit and whose turn it is
     public IEnumerator Move(Pit movePit)
     {
          //check for valid move
          if (movePit.pitStones.Count > 0)
          {
               //block input while making move
               canMove = false;

               //get info from chosen pit
               int pitIndex = myPitMgr.pits.IndexOf(movePit);
               List<myStone> moveStones = new List<myStone>();

               //keep track of stones to be moved
               foreach (myStone p in movePit.pitStones)
               {
                    moveStones.Add(p);
               }

               //clear stones from selected pit
               movePit.pitStones.Clear();

               //make the move

               //set correct scorepit to skip
               Pit skipPit = playerTurn ? myPitMgr.OpponentPits[6] : myPitMgr.PlayerPits[6];
               int offset = 1;
               foreach (myStone stone in moveStones)
               {
                    if(myPitMgr.pits[(pitIndex + offset) % 14] == skipPit)
                    {
                         offset++;
                    }
                    stone.SetPit(myPitMgr.pits[(pitIndex + offset) % 14]);
                    offset++;
                    yield return new WaitForSeconds(0.3f);

               }
               //Keep track of last pit dropped
               Pit lastPit = myPitMgr.pits[(pitIndex + --offset) % 14];



               //unblock input
               canMove = true;

               //change the player's turn unless last pit is score pit
               if (!IsScorePit(lastPit))
               {
                    //check for empty pit
                    if (lastPit.pitStones.Count == 1)
                    {
                         //check whose turn and if the empty last pit is on their side
                         if (playerTurn && myPitMgr.PlayerPits.Contains(lastPit))
                         {
                              StartCoroutine(myPitMgr.CaptureStones(lastPit, playerTurn));
                         }
                         else if (!playerTurn && myPitMgr.OpponentPits.Contains(lastPit))
                         {
                              StartCoroutine(myPitMgr.CaptureStones(lastPit, playerTurn));
                         }
                    }

                    playerTurn = !playerTurn;
                    if (!playerTurn)
                    {
                         canAIMove = true;
                    }
               }
               canAIMove = true;

          }
          winCondition = AllPitsEmpty(myPitMgr);
          if (winCondition != 0)
          {
               EndGameWrapUp(myPitMgr, winCondition);
          }
     }

     bool IsScorePit(Pit p)
     {
          return playerTurn ? p == myPitMgr.PlayerPits[6] : p == myPitMgr.OpponentPits[6];

     }

     //when there is a win condition, do endgame steps and decide winner.
     //return 1 for player win, 2 for opponent win
     //takes int of player with empty side of board from AllPitsEmpty() and pitMgr
     void EndGameWrapUp(PitMgr pitState, int emptyPlayer)
     {
          //FIXME seems to work, make stone move into coroutine like move
          //player has empty pits
          if(emptyPlayer == 1 && canMove)
          {
               StartCoroutine(MoveStonesToScorePit(pitState.OpponentPits));
          }
          else if (emptyPlayer == 2 && canMove)
          {
               StartCoroutine(MoveStonesToScorePit(pitState.PlayerPits));
          }

          StartCoroutine(CalculateWin());

     }

     IEnumerator CalculateWin()
     {
          yield return new WaitUntil(() => canMove);

          int winNum = myPitMgr.PlayerPits[6].pitStones.Count - myPitMgr.OpponentPits[6].pitStones.Count;
          if (winNum > 0)
          {
               print("Player Wins!");
               winNumber = 1;
          }
          else if (winNum < 0)
          {
               print("Opponent Wins!");
               winNumber = 2;
          }
          else
          {
               print("Draw!");
               winNumber = 0;
          }

     }

     //Move stones one by one to passed player's pits to their score pit
     IEnumerator MoveStonesToScorePit(List<Pit> pitsToMove)
     {
          canMove = false;
          for (int i = 0; i < 6; i++)
          {
               int stoneCount = pitsToMove[i].pitStones.Count;
               while(pitsToMove[i].pitStones.Count != 0)
               {
                    pitsToMove[i].pitStones[0].SetPit(pitsToMove[6]);
                    yield return new WaitForSeconds(0.2f);
               }
          }
          canMove = true;
     }

     //checks for all empty wells for each player. returns 1 for player, 2 for opponent. 0 if no end game condition
     int AllPitsEmpty(PitMgr pitState)
     {
          bool playerAllEmpty = true;
          foreach(Pit p in pitState.PlayerPits)
          {
               if (p != pitState.PlayerPits[6]) {
                    if (p.pitStones.Count > 0)
                    {
                         playerAllEmpty = false;
                         break;
                    }
               }
          }
          if(playerAllEmpty)
          {
               return 1;
          }

          bool opponentAllEmpty = true;
          foreach (Pit p in pitState.OpponentPits)
          {
               if (p != pitState.OpponentPits[6])
               {
                    if (p.pitStones.Count > 0)
                    {
                         opponentAllEmpty = false;
                         break;
                    }
               }
          }
          if (opponentAllEmpty)
          {
               return 2;
          }

          return 0;
     }

     //Player controls during game
     void InputContols()
     {
          //test empty pits wrap up
/*          if (Input.GetKeyDown(KeyCode.Space))
          {
               StartCoroutine(MoveStonesToScorePit(myPitMgr.PlayerPits));
          }*/
          //testing ai board states
/*          if (Input.GetKeyDown(KeyCode.P))
          {
               BoardState currentBoard = new BoardState(myPitMgr);
               int aiMove = myAiMgr.AiMove(true);
               BoardState nextMove = new BoardState(myAiMgr.CreateMove(currentBoard, aiMove, false));
               //currentBoard.printBoard();
               Debug.Log(aiMove);
               nextMove.printBoard();
               //Debug.Log(myAiMgr.StaticEvaluator(currentBoard).ToString());
               //Debug.Log(myAiMgr.StaticEvaluator(nextMove).ToString());
               //Debug.Log(myAiMgr.EndGameEval(currentBoard, 1).ToString());
          }*/

          //AIMove()
          if (Input.GetKeyDown(KeyCode.A) && !playerTurn && canAIMove)
          {
               canAIMove = false;
               int myMove = myAiMgr.AiMove(!playerTurn);
               Debug.Log(myMove.ToString());
               if(!(myMove == -20))
               {
                    StartCoroutine(Move(myPitMgr.pits[myMove]));
               }
               else
               {
                    Debug.Log("Error, no move found");
               }

          }


          //player moves
          if (playerTurn && canMove)
          {
               if (Input.GetKeyDown(KeyCode.Alpha1))
               {
                    StartCoroutine(Move(myPitMgr.PlayerPits[0]));
               }
               else if (Input.GetKeyDown(KeyCode.Alpha2))
               {
                    StartCoroutine(Move(myPitMgr.PlayerPits[1]));
               }
               else if (Input.GetKeyDown(KeyCode.Alpha3))
               {
                    StartCoroutine(Move(myPitMgr.PlayerPits[2]));
               }
               else if (Input.GetKeyDown(KeyCode.Alpha4))
               {
                    StartCoroutine(Move(myPitMgr.PlayerPits[3]));
               }
               else if (Input.GetKeyDown(KeyCode.Alpha5))
               {
                    StartCoroutine(Move(myPitMgr.PlayerPits[4]));
               }
               else if (Input.GetKeyDown(KeyCode.Alpha6))
               {
                    StartCoroutine(Move(myPitMgr.PlayerPits[5]));
               }
          }

          if (!playerTurn && canMove)
          {
               //debug opponent moves
               if (Input.GetKeyDown(KeyCode.Keypad1))
               {
                    StartCoroutine(Move(myPitMgr.OpponentPits[0]));
               }
               else if (Input.GetKeyDown(KeyCode.Keypad2))
               {
                    StartCoroutine(Move(myPitMgr.OpponentPits[1]));
               }
               else if (Input.GetKeyDown(KeyCode.Keypad3))
               {
                    StartCoroutine(Move(myPitMgr.OpponentPits[2]));
               }
               else if (Input.GetKeyDown(KeyCode.Keypad4))
               {
                    StartCoroutine(Move(myPitMgr.OpponentPits[3]));
               }
               else if (Input.GetKeyDown(KeyCode.Keypad5))
               {
                    StartCoroutine(Move(myPitMgr.OpponentPits[4]));
               }
               else if (Input.GetKeyDown(KeyCode.Keypad6))
               {
                    StartCoroutine(Move(myPitMgr.OpponentPits[5]));
               }
          }
     }
}
