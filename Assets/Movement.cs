using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Llama a animator de Manuel <avatar>
    private Animator animator;
    //llama al controlador
    private CharacterController characterController;
    //Seleccionar la camara principal en la cual basar el movimiento 
    public new Transform camera;
    public float jumpHeight = 3; //Altura de salto
    public float speed = 4; //Ajuste de la velocidad necesaria para el movimiento del teclado o sensor
    public float gravity = -9.81f; //Ajustado a la gravedad de la tierra


    Vector3 velocity;
    //Comprando que el avatar este en el suelo
    public Transform groundcheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    bool isGrounded;
    void Start()
    {
        characterController = GetComponent<CharacterController>();  //llamamos el character controller
        animator = GetComponent<Animator>();                        // llamamos el animator
    }

    
    void Update()
    {
        //Comprobar que el avatar esta en el suelo
        isGrounded = Physics.CheckSphere(groundcheck.position,groundDistance,groundMask);
        if (isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }

        //Conexion a gamepad o teclado 
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        

        Vector3 movement = Vector3.zero;
        float movementSpeed = 0;

        if (hor != 0 || ver != 0)
        {
            //normalizar y ajustar direccion de la camara en ver
            Vector3 forward = camera.forward;
            forward.y = 0;
            forward.Normalize();
            //Ajustar y normalizar valores de la camara en hor
            Vector3 right = camera.right;
            right.y = 0;
            right.Normalize(); 
           
            //Vector de movimiento 
            Vector3 direction = camera.forward * ver + camera.right * hor;
            movementSpeed = Mathf.Clamp01(direction.magnitude);
            direction.Normalize();

            movement = direction * speed  * Time.deltaTime; //variable de movimiento
            //Funcion de rotacion
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
        }
        movement.y += gravity * Time.deltaTime;

        //Funcion de movimiento
        characterController.Move(movement);
        //Funcion de animacion
        animator.SetFloat("Speed",movementSpeed);

        //Formulacion de salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity); //Formula de salto
        }
        velocity.y += gravity * Time.deltaTime; // velocidad de atraccion en el eje y
        // Funcion de salto
        characterController.Move(velocity*Time.deltaTime);
    }
}
