using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class gulybad : MonoBehaviour
{
    [Header("move and stuff")]
    public NavMeshAgent cerebro; 
   
    [Header("daño y vida")]
    //TESTING
    public int vida = 4; //cantidad maxima de disparos que puede aguantar el gulybag
    public int ID = 0;
    public float dist_vision = 5;  //numero que indica que tan lejos puede ver

    [Header("not public")]
    [SerializeField] private Vector3 target; //su objetivo
    [SerializeField] private string nombre;

    void Start()
    {
        cerebro = this.GetComponent<NavMeshAgent>();
        cerebro.destination = target; //al seleccionar esto como target unity movera al mob
    }

    // Update is called once per frame
    void Update()
    {
        //direct_ver_state();
        cerebro.destination = target;
        nombre = "ninguno";
        Chase();

    
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(this.name + " ABISSAL screech NOICES");

    }

    public void Set_target(Vector3 aux)//metodo utilizado para cambiar el target actual, es solo de acceso
    { 
        target = aux;
    }

    private void Chase()
    {
        Vector3 frente = target;
        //Vector3 pos0 = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Vector3 pos0 = transform.position;
        Ray aux = new Ray(pos0,frente);
        // Vector3 frente = transform.forward;
        Debug.DrawRay(pos0,frente, Color.black);
        if (Physics.Raycast( aux, out var HitInfo,dist_vision))
        {
            
            Debug.DrawRay(pos0, frente*dist_vision , Color.black);
            //this.gameObject.GetComponent<Animator>().SetTrigger("disparar");
            nombre = HitInfo.transform.name;
            if (HitInfo.transform.tag == "Player")
            {
                // Debug.Log(this.name + " ABISSAL SCREAM NOICES");
                HitInfo.transform.GetComponent<character>().recibirDanio();
            }
        }
    }

    public void interactuar()
    {
        if (vida > 1)
        {
            vida--;
            //Debug.Log(name + ": vida menos");
        }
        else
        {
            GameObject.FindGameObjectWithTag("hivequeen").transform.GetComponent<Director_IA>().matamob(ID);
        }
        

    }
}
