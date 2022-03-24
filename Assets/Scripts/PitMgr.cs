using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitMgr : MonoBehaviour
{

     public static PitMgr inst;
     public List<Pit> pits;
     public List<Pit> PlayerPits;
     public List<Pit> OpponentPits;

     public StoneMgr myStoneMgr;

     private void Awake()
     {
          inst = this;
          resetPits();
     }

     public int CheckForWin()
     {
          for (int i = 0; i < 6; i++)
          {
               if(PlayerPits[i].pitStones.Count > 0)
               {
                    return -1;
               }
               if (OpponentPits[i].pitStones.Count > 0)
               {
                    return -1;
               }
          }

          if(PlayerPits[6].pitStones.Count >= OpponentPits[6].pitStones.Count)
          {
               return 1;
          }
          else
          {
               return 2;
          }
          


     }


     public IEnumerator CaptureStones(Pit myPit, bool isPlayer)
     {
          List<Pit> capturePlayerPits;
          List<Pit> otherPits;
          capturePlayerPits = isPlayer ? PlayerPits : OpponentPits;
          otherPits = isPlayer ? OpponentPits : PlayerPits;
          Pit victimPit = otherPits[5 - capturePlayerPits.IndexOf(myPit)];

          while(myPit.pitStones.Count > 0)
          {
               myPit.pitStones[0].SetPit(capturePlayerPits[6]);
               yield return new WaitForSeconds(0.3f);
          }
          while (victimPit.pitStones.Count > 0)
          {
               victimPit.pitStones[0].SetPit(capturePlayerPits[6]);
               yield return new WaitForSeconds(0.3f);
          }

     }


     //reset stones to 4 in each pit
     public void resetPits()
     {
          int stoneIndex = 0;

          //player pits
          for (int i = 0; i < PlayerPits.Count - 1; i++)
          {
               for(int j = 0; j < 4; j++)
               {
                    myStoneMgr.Stones[stoneIndex].SetPit(PlayerPits[i]);
                    
                    stoneIndex++;
               }
          }

          //opponent pits
          for (int i = 0; i < OpponentPits.Count - 1; i++)
          {
               for (int j = 0; j < 4; j++)
               {
                    myStoneMgr.Stones[stoneIndex].SetPit(OpponentPits[i]);

                    stoneIndex++;
               }
          }
     }
}
