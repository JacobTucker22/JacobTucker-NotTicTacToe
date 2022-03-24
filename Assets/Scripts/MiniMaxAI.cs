using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxAI : MonoBehaviour
{
     public static MiniMaxAI inst;
     public int ply = 2;
     public int doubleMovePreference = 2;
     public PitMgr myPitMgr;
     public bool isAITurn;

     private void Awake()
     {
          inst = this;
     }


     //Returns best move
     //takes if its AI move or player move, and a max depth to minimax search
     //Initially set up to do either player's turn, but changed to do only Ai turn.
     public int AiMove(bool AITurn)
     {
          BoardState currentBoard = new BoardState(myPitMgr);
          //If double move exists, return that move
/*          int simpleMoveCheck = DoubleTurnMove(currentBoard, AITurn);
          if (simpleMoveCheck != -1)
          {
               return simpleMoveCheck;
          }*/

          int bestMove = -20;
          int bestScore = -999;
          int offset = AITurn ? 7 : 0;

          for(int i = 0 + offset; i < 6 + offset; i++)
          {
               //invalid move
               if (currentBoard.allPits[i] < 1)
               {
                    continue;
               }
               //if double turn, it goes to maximizer for another turn + ai preference for double turns
               int score;
               int simpleMoveCheck = DoubleTurnMove(currentBoard, AITurn);
               if (simpleMoveCheck == i)
               {
                    score = Maximizer(currentBoard, ply - 1, i) + doubleMovePreference;
               }
               else
               {
                    score = Minimizer(currentBoard, ply - 1, i);
               }

               if (score > bestScore)
               {
                    bestScore = score;
                    bestMove = i;
               }

          }
          Debug.Log("Best Move: " + bestMove.ToString());
          Debug.Log("MiniMax Score: " + bestScore.ToString());
          return bestMove;
     }

/*     public int MiniMax(BoardState bState, int maxDepth, int candidateMoveIndex)
     {
          int bestBoardState = -999;


     }*/

     public int Minimizer(BoardState bState, int maxDepth, int candidateMoveIndex)
     {
          BoardState currentBoard = new BoardState(CreateMove(bState, candidateMoveIndex, false));
          if (currentBoard.allPits == null)
          {
               return 9999;
          }
          int endGameCond = EndGameCondition(currentBoard);
          if (endGameCond != 0)
          {
               return EndGameEval(currentBoard, endGameCond);
          }
          if (maxDepth < 1)
          {
               return StaticEvaluator(currentBoard);
          }
          //int bestMove = -20;
          int worstScore = 999;
          int offset = 0;

          for (int i = 0 + offset; i < 6 + offset; i++)
          {
               if (currentBoard.allPits[i] < 1)
               {
                    continue;
               }


               int score;
               int simpleMoveCheck = DoubleTurnMove(currentBoard, false);
               if (simpleMoveCheck == i)
               {
                    score = Minimizer(currentBoard, maxDepth - 1, i);
               }
               else
               {
                    score = Maximizer(currentBoard, maxDepth - 1, i);
               }

               if (score <= worstScore)
               {
                    worstScore = score;
                    //bestMove = i;
               }
          }

          return worstScore;
     }

     public int Maximizer(BoardState bState, int maxDepth, int candidateMoveIndex)
     {
          BoardState currentBoard = new BoardState(CreateMove(bState, candidateMoveIndex, true));
          if(currentBoard.allPits == null)
          {
               return -9999;
          }
          int endGameCond = EndGameCondition(currentBoard);
          if (endGameCond != 0)
          {
               return EndGameEval(currentBoard, endGameCond);
          }
          if(maxDepth < 1)
          {
               return StaticEvaluator(currentBoard);
          }
          //int bestMove = -20;
          int bestScore = -999;
          int offset = 7;

          for (int i = 0 + offset; i < 6 + offset; i++)
          {

               if (currentBoard.allPits[i] < 1)
               {
                    continue;
               }

               int score;
               int simpleMoveCheck = DoubleTurnMove(currentBoard, true);
               if (simpleMoveCheck == i)
               {
                    score = Maximizer(currentBoard, maxDepth - 1, i) + doubleMovePreference;
               }
               else
               {
                    score = Minimizer(currentBoard, maxDepth - 1, i);
               }

               if (score > bestScore)
               {
                    bestScore = score;
                    //bestMove = i;
               }
          }

          return bestScore;

     }

     //check for move landing in score pit for double turn.
     //This is simplest move to make. Will return -1 if there is not such a move
     //Otherwise it will return the index of the pit for the move
     //If it is AI turn, it will set an offset for otherside of the board.
     public int DoubleTurnMove(BoardState bState, bool AITurn)
     {
          //set offset for AI
          int offset = AITurn ? 7 : 0;
          for(int i = 0; i < 7; i++)
          {
               if(bState.allPits[i + offset] + (i + offset) == 6 + offset)
               {
                    return i + offset;
               }
          }
          return -1;
     }

     //checks for all empty pits for both sides, returns 1 for player, 2 for ai, 0 for none
     public int EndGameCondition(BoardState bstate)
     {
          bool emptyPlayer = true;
          bool emptyAI = true;
          for (int i = 0; i < 6; i++)
          {
               if (bstate.allPits[i] > 0)
               {
                    emptyPlayer = false;
               }
               if ((bstate.allPits[i + 7] > 0))
               {
                    emptyAI = false;
               }
          }
          if (emptyPlayer)
          {
               return 1;
          }
          if (emptyAI)
          {
               return 2;
          }

          return 0;
     }

     //create a candidate board state based on selected pit
     public BoardState CreateMove(BoardState bState, int pitIndex, bool isPlayerTurn)
     {
          //check for invalid move. Return null, if move does not exist
          if(bState.allPits[pitIndex] == 0)
          {
               return null;
          }


          //copy board into new board state to be manipulated
          BoardState newBoardState = new BoardState(bState);

          int numberToMove = newBoardState.allPits[pitIndex];
          newBoardState.allPits[pitIndex] = 0;
          int skipScorePitIndex = isPlayerTurn ? 13 : 6;

          int offset = 1;
          for(int i = 0; i < numberToMove; i++)
          {
               //skip opponents score pit
               if((pitIndex + offset) % 14 == skipScorePitIndex)
               {
                    offset++;
               }
               newBoardState.allPits[(pitIndex + offset) % 14]++;
               offset++;
          }

          int lastPitIndex = (pitIndex + offset - 1) % 14;
          if(!isPlayerTurn && lastPitIndex > 6 && lastPitIndex < 13 && newBoardState.allPits[lastPitIndex] == 1)
          {
               int victimPit = 12 - lastPitIndex;
               newBoardState.allPits[13] += newBoardState.allPits[victimPit] + 1;
               newBoardState.allPits[victimPit] = 0;
               newBoardState.allPits[lastPitIndex] = 0;
               
          }
          else if (isPlayerTurn && lastPitIndex >= 0 && lastPitIndex < 6 && newBoardState.allPits[lastPitIndex] == 1)
          {
               int victimPit = 12 - lastPitIndex;
               newBoardState.allPits[6] += newBoardState.allPits[victimPit] + 1;
               newBoardState.allPits[victimPit] = 0;
               newBoardState.allPits[lastPitIndex] = 0;
          }

          newBoardState.playerScore = newBoardState.allPits[6];
          newBoardState.opponentScore = newBoardState.allPits[13];
          return newBoardState;
     }

     //End game condition evaluator. 
     //Takes board state and which player is empty from EndGameCondition(). Must be 1 or 2
     //Moves non empty side to their score pit
     public int EndGameEval(BoardState bState, int emptyPlayer)
     {
          BoardState newBoardState = new BoardState(bState);
          //set an offset for ai player
          int playerOffset = emptyPlayer == 2 ? 0 : 7;

          for(int i = 0; i < 6; i++)
          {
               newBoardState.allPits[6 + playerOffset] += newBoardState.allPits[i + playerOffset];
               newBoardState.allPits[i + playerOffset] = 0;
          }

          return StaticEvaluator(newBoardState);

     }

     //Evaulates and quantifies utility of board state
     public int StaticEvaluator(BoardState bState)
     {

          return bState.allPits[13] - bState.allPits[6];

          //return bState.allPits[6] - bState.allPits[13];

     }


}
