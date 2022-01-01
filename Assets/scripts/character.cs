using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    [Header("movimiento y rotacion:")]
    [Range(100f, 1000f)] public float velocidadMov = 0.25f; //la velocidad a la que se desplaza
    [Range(0f, 10f)] public float velocidadRot = 10; //que tan sensible es la rotacion (posiblemente es temporal)
    [SerializeField] private bool can_rotate = true; // si la funcion de rotar esta disponible
    [SerializeField] private Vector3 mov; //vector de movimiento
    private float x, y;

    [Header("da�o y vida")]
    [SerializeField] private bool daniado; //indica sido da�ado recientemente
    public int vidaActual; //cantidad de puntos de vida actual
    [SerializeField] private int vidaMax = 10; //cantidad maxima de puntos de vida que puede tener el jugador 

    [Header("componentes y accesos directos")]
    public Camera camara;
    Rigidbody MotFis; //esto se usa para acceder al rigidbody del cuerpo
    public Animator animator;

    //--------------------------------------------------------------------------
    [Header("timers")]
    [SerializeField] private float MaxTime = 0.3f;
    [SerializeField] private float AcTTime = 0f;
    [SerializeField] private bool bala = true;

    void Start(){
        MotFis = this.GetComponent<Rigidbody>(); //declara el motfis (el motfis resibe su nombre de MOTor de FISicas)
        vidaActual = vidaMax;
    }

    void Update(){

        //---------------------------------------------------------------------

        //x = Input.GetAxis("Horizontal");
        //y = Input.GetAxis("Vertical");
        //transform.Translate(x*Time.deltaTime*velMovimiento,0,0);
        //transform.Translate(0,0,y*Time.deltaTime*velMovimiento);
        //animator.SetFloat("VelX", x);
        //animator.SetFloat("VelY", y);
        //apuntado();
        //----------------------------------------------------------------------

        //este bloque se encarga de gestionar el movimiento del jugador 
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {

            mov = new Vector3(velocidadMov * Input.GetAxis("Horizontal"), 0, velocidadMov * Input.GetAxis("Vertical"));

        } else{
               mov = Vector3.zero;                
        }
        MotFis.AddForce(mov);

        animator.SetFloat("VelX", Input.GetAxis("Horizontal"));
        animator.SetFloat("VelY", Input.GetAxis("Vertical"));

        if (can_rotate){
            apuntado();
        }

        disparar();

        if (vidaActual == 0)
        {
            GameObject.FindGameObjectWithTag("hivequeen").gameObject.GetComponent<Director_IA>().FailState(this.gameObject);
           
        }

    }
   
    //funcion sacada de un tutorial que pille 
    private (bool success, Vector3 position) getposicionmouse(){ 
        var ray = camara.ScreenPointToRay(Input.mousePosition); //genera un rayo hacia adonde apunte el mouse
        
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity,groundLayer))
        {
            //si el raycast pega con algo este mandara informacion de donde pego ;
            return (success: true, position: hitInfo.point);
        }
        else
        {
            //no ah pegado nada nadita 
            return (success: false, position: Vector3.zero);
        }
    }
    //
    private void apuntado()
    {
        var (success, position) = getposicionmouse();
        if (success){
            var direccion = position - transform.position;
            direccion.y = 0;
            transform.forward = direccion;
            //Debug.DrawRay(transform.position, transform.forward*5, Color.yellow);
        }
    }

    private void disparar()
    {
        if (Input.GetButton("Fire1")){
            AcTTime += Time.deltaTime;
            if (AcTTime > MaxTime)
            {
                bala = true;
                AcTTime = 0;
            }
            if (bala)
            {
                bala = false;
                //Vector3 pos0 = transform.position;
                Vector3 frente = transform.forward;
                Vector3 pos0 = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                // Vector3 frente = transform.forward;
                if (Physics.Raycast(pos0, frente, out var HitInfo))
                {
                    //Debug.Log("\npos0: "+pos0+"\tforward: "+frente);
                    Debug.DrawRay(pos0, frente * 10000, Color.magenta);
                    //this.gameObject.GetComponent<Animator>().SetTrigger("disparar");
                    if (HitInfo.transform.tag == "mobs")
                    {
                        HitInfo.transform.GetComponent<gulybad>().interactuar();
                    }
                    if (HitInfo.transform.tag == "spawns")
                    {

                        HitInfo.transform.GetComponent<spawn>().interactuar();
                    }
                }
            }
        }
        else
        {
            AcTTime = 0;
        }
    }

    public void recibirDanio()
    {
        vidaActual--;
    }
    
}

