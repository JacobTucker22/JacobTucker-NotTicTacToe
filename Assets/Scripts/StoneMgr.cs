using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StoneMgr : MonoBehaviour
{

     public static StoneMgr inst;

     public List<myStone> Stones;

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
        
    }

/*     public void resetStones()
     {
          for(int i = 0; i < Stones.Count; i++)
          {
               for (int j = 0; j < 4; j++)
               {
                    Stones[i].setPit()
               }
          }
     }*/

}
