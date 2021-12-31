using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director_IA : MonoBehaviour
{
    [Header("MARCADORES")]
    public Transform objetivo; //hace referencia al jugador, de esta forma se sabe donde esta en todo momento 
    public Vector3 target; //esta es una variable probablemente temporal que solo se usa para no andar extrallendo la posicion del jugador cada vez que se necesita 
    public GameObject spawn;  //se crea para que se tenga un registro de que es un spawn 
    public GameObject mob;    //o un mob, se ocuparan mas adelante cuando se termine la creacion de entidades. 
    public GameObject vacio;

    [Header("GESTION de los mobs")]
    [Range(9, 1000)]
    public int max_enemy; //cantidad maxima de mobs que puede haber 
    [SerializeField] private int cant_enemys; //cantidad actual
    public List<GameObject> mobs_list; //listado para gestionarlos

    [Header("GESTION de los spawn")]
    [Range(5, 25)]
    public int max_spawn; //cantidad maxima de spawns que puede haber 
    [SerializeField]int cant_spawn; //cantidad actual
    public List<GameObject> spawns_list; //listado para gestionarlos
    public int areaCreacion; //el area en la que pueden aparecer los spawn 
    public Transform UbicacionM;
    public Transform UbicacionS;
    private Vector3 area ;

    [Header("tiempo")]
    public float TimerCrearSpawn = 5;
    [SerializeField] private float ACTTimerCrearSpawn;
    public float TimerCrearMob = 1 ;
    [SerializeField] private float ACTTimerCrearMob;

    void Start()
    {
        //mobs_list.AddRange(GameObject.FindGameObjectsWithTag("mobs")); //addrange se usa para agregar arreglos a la lista, esto agrega todos los mobs que ya existan 
        cant_enemys = mobs_list.Count;
        cant_spawn = spawns_list.Count;
        target = objetivo.position;
        ACTTimerCrearMob = TimerCrearMob;
        ACTTimerCrearSpawn = TimerCrearSpawn;

    }

    void Update()
    {
        act_target_pos();

        area = new Vector3(areaCreacion,1,areaCreacion);

        Gestor();

        if (Input.GetButtonDown("Fire3"))
        {
            crear_spawn();
        }
        if(cant_enemys == 0 && cant_spawn == 0){
            WinState();
        }


    }

    private void act_target_pos(){ //esta funcion actualiza la posicion del jugador en cada paso 
        target = objetivo.position;
        for (int i = 0; i < mobs_list.Count; i++){
            if (mobs_list[i])
            {
                mobs_list[i].gameObject.GetComponent<gulybad>().Set_target(objetivo.position);
            }
        }
    }
    
    public void crear_mob(Vector3 coor, Transform place){
        GameObject aux = Instantiate(mob, coor, Quaternion.identity,place);
        //GameObject aux = Instantiate(mob,place);
        aux.gameObject.name = "mob"+(mobs_list.Count + 1);
        aux.gameObject.GetComponent<gulybad>().ID = mobs_list.Count;
        //aux.transform.GetChild();
        mobs_list.Add(aux);
        cant_enemys++;
    }

    public void matamob(int index){

        //mobs_list.Remove(mobs_list[index]);
        Destroy(mobs_list[index]);
        mobs_list[index] = null;
        cant_enemys--;
    }

    private void crear_spawn()
    {
        Vector3 pos = transform.position + new Vector3(Random.Range(-(areaCreacion / 2), (areaCreacion / 2)), 0, Random.Range(-(areaCreacion / 2), (areaCreacion / 2)));
        GameObject aux = Instantiate(spawn, pos, Quaternion.identity,UbicacionS);
        aux.gameObject.name = "spawn " + (spawns_list.Count);
        aux.gameObject.GetComponent<spawn>().ID = spawns_list.Count;
        aux.gameObject.GetComponent<spawn>().ubicacion = UbicacionM;
        spawns_list.Add(aux);
        cant_spawn++;
    }

    public void mataSpawn(int index)
    {
        Destroy(spawns_list[index]);
        spawns_list[index] = null;
        cant_spawn--;

    }

    public void FailState(GameObject aux){

        Debug.Log("muerto");
        aux.gameObject.GetComponent<character>().vidaActual = 10;
        //Destroy(aux);
    }

    public void WinState()
    {
        Debug.Log("ganaste");
    }

    private void ReduxReloj(){
        float tima = Time.deltaTime;
        ACTTimerCrearSpawn -= tima;
        ACTTimerCrearMob -= tima;
    }

    private void Gestor()
    {
        ReduxReloj();
        if (ACTTimerCrearSpawn < 0)
        {
            ACTTimerCrearSpawn = TimerCrearSpawn;
            crear_spawn();
        }

        if (ACTTimerCrearMob < 0)
        {
            ACTTimerCrearMob = TimerCrearMob;
            int aux = Random.Range(0, spawns_list.Count);
            if (spawns_list[aux].transform.tag == "spawn")
            {
                spawns_list[aux].gameObject.GetComponent<spawn>().CrearBastago();
            }
               
        }
    }


    private void OnDrawGizmos(){

        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireCube(Vector3.zero, area);
        //Gizmos.DrawCube(new Vector3(0,1,0), area);

        }

    }
