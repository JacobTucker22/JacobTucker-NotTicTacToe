using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myStone : MonoBehaviour
{
     public Vector3 stonePosition;

     
     private Pit parentPit;


     //Moves from previous pit to new pit, and moves itself to that pit
     public void SetPit(Pit newPit)
     {
          if (parentPit != null && parentPit.pitStones.Contains(this))
          {
               parentPit.pitStones.Remove(this);
          }

          Vector3 randomvec = new Vector3((Random.value - 0.5f)/2, (Random.value - 0.5f) * 3, (Random.value - 0.5f) / 2); ;

          parentPit = newPit;
          parentPit.pitStones.Add(this);
          this.gameObject.GetComponent<Rigidbody>().transform.position = 
               parentPit.GetComponentInParent<Transform>().transform.position + randomvec;

     }


}
