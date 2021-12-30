using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
   
    public float area_de_creacion = 10; //area que tiene para crear entidades al rededor suyo 
    public Transform ubicacion; //esta variable se utiliza 
    Vector3 centro = new Vector3(30,1,10);
    public int ID = 0;
    public int vidaMax = 20;
    [SerializeField]private int vidaActual;


    // Start is called before the first frame update
    void Start(){
        centro = transform.position;
        vidaActual = vidaMax;


        for (int i = 0; i < GameObject.FindGameObjectWithTag("hivequeen").gameObject.GetComponent<Director_IA>().max_enemy; i++)
        {
            CrearBastago();
        }
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetButtonDown("Fire2")){
            CrearBastago();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(centro, area_de_creacion);
    }

    public void CrearBastago(){

        Vector3 pos = centro + new Vector3(Random.Range(-(area_de_creacion/2),(area_de_creacion / 2)),2, Random.Range(-(area_de_creacion / 2), (area_de_creacion / 2)));
        GameObject.FindGameObjectWithTag("hivequeen").transform.GetComponent<Director_IA>().crear_mob(pos,ubicacion);
        
    }

    public void interactuar()
    {
        if (vidaActual > 1)
        {
            vidaActual--;
        }
        else
        {
            GameObject.FindGameObjectWithTag("hivequeen").transform.GetComponent<Director_IA>().mataSpawn(ID);
        }

    }
}
