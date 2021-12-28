using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gulybad : MonoBehaviour
{
    [Header("move and stuff")]

    public float dist_vision = 5;  //numero que indica que tan lejos puede ver 

    [Range(0, 4)]
    public int selected_move = 4; //numero que indica hacia donde se mueve el mob
    [Range(1, 100)]
    public int vel_desplazamiento =10; //numero que indica la velocidad de desplazamiendo
    Rigidbody MOV; //variable que controla la fisica 

    [Header("not public")]
    [SerializeField]private Vector3 target; //su objetivo
    [SerializeField] List<Vector3> direcciones; //vectores direccionales

    [Header("de momento sin uso o en testing")]
    public List<bool> verDir; //verificadores de si ese sitio es factible
    //TESTING
    public int vida = 4; //cantidad maxima de disparos que puede aguantar el gulybag
    public int ID = 0;
    

    void Start()
    {
        MOV = this.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        direct_ver_state();

        Set_velocity();
        MOV.AddForce(direcciones[selected_move]);
        //MOV.AddForce(direcciones[selected_move] * vel_desplazamiento); //por algun motivo esta funcion no trabaja como deberia 
        selector_dir();
    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player"){
           // Debug.Log(this.name + " ABISSAL SCREAM NOICES");
            collision.gameObject.GetComponent<character>().recibirDanio();
        }
    }

    void direct_ver_state()//de momento esta parte es inutil pero se usa para mantener el estado de las direcciones o sea si una direccion es factible o no 
    {  
        verDir[0] = !Physics.Raycast(transform.position, direcciones[0], dist_vision); //0  up
        verDir[1] = !Physics.Raycast(transform.position, direcciones[1], dist_vision); //1  down 
        verDir[2] = !Physics.Raycast(transform.position, direcciones[2], dist_vision); //2  right 
        verDir[3] = !Physics.Raycast(transform.position, direcciones[3], dist_vision); //3  left 
    }

    public void Set_target(Vector3 aux)//metodo utilizado para cambiar el target actual, es solo de acceso
    { 
        target = aux;
    }

    public void Set_velocity()//se utiliza para cambiar de la velocidad de cada vector, estoy seguro que no sirve de nada pero por si acaso mantendre la funcion aqui 
    { 
        direcciones[0] = Vector3.forward * vel_desplazamiento;
        direcciones[1] = -Vector3.forward * vel_desplazamiento;
        direcciones[2] = Vector3.right * vel_desplazamiento;
        direcciones[3] = Vector3.left * vel_desplazamiento;
    }

    void selector_dir() //este es la version primitiva del algoritmo de desplazamiento, compara que direccion es la adecuada para llegar al target
    { 
        Vector3 aux;
        aux = new Vector3(target.x-this.transform.position.x, target.y - this.transform.position.y, target.z - this.transform.position.z);
        if(Mathf.Abs(aux.x) > Mathf.Abs(aux.z)){  //este if else selecciona la primera posibilidad de movimiento del personaje, compara que eje es mas corto x o z
            if (aux.x>0 ){ //ahora compara si es positivo o negativo 
                selected_move = 2;
            }else{
                selected_move = 3;
            }
        }else{
            if (aux.z > 0){
                selected_move = 0;
            }else{
                selected_move = 1;
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
