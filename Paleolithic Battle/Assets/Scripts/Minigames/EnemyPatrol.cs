using UnityEngine;


[RequireComponent(typeof(CharacterController2D))]
public class EnemyPatrol : MonoBehaviour
{
    public MinigamePlatform minigamePlatform;
    private CharacterController2D controller;

    public float runSpeed = 40f;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    float horizontalMove = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length == 0) return; // Si no hay puntos de patrulla, no hacemos nada

        // Mover al enemigo hacia el siguiente punto de patrulla
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);

        // Si el enemigo está cerca del punto de patrulla, cambiar al siguiente
        if (distance < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Ciclar a través de los puntos de patrulla
            direction = (waypoints[currentWaypointIndex].position - transform.position).normalized; // Actualizar dirección
        }
        horizontalMove = direction.x * runSpeed;
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false); // Mover al enemigo
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aquí puedes agregar la lógica para cuando el enemigo colisiona con el jugador
            Debug.Log("Enemigo ha colisionado con el jugador");
            minigamePlatform.Lose(); // Llamar al método Lose del minijuego
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aquí puedes agregar la lógica para cuando el enemigo entra en contacto con el jugador
            Debug.Log("Enemigo ha entrado en contacto con el jugador");
            minigamePlatform.Win(); // Llamar al método Win del minijuego

        }
    }
}
