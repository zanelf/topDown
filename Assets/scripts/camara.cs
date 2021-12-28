using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara : MonoBehaviour
{

    public Transform poscams;
    public Vector3 offset;
    //public int offset_x = 0;

   
    void Start()
    {
        this.transform.position = new Vector3(poscams.transform.position.x + offset.x,this.transform.position.y + offset.y, poscams.transform.position.z+offset.z);
    }
    
    void Update()
    {
        //offset.y = 0;
        this.transform.position = new Vector3( (poscams.position.x + offset.x) , (this.transform.position.y + offset.y) , (poscams.position.z+offset.z) );


    }


}
