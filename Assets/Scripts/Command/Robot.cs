using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace CommandPattern
{
    public class Robot : MonoBehaviour
    {
        public enum RobotType
        {
            Perseguidor,
            Disparador,
            Patrullador
        }

        public RobotType tipoDeRobot;  // Tipo de robot asignado desde el inspector
        public Transform player;  // Referencia al jugador
        public NavMeshAgent agent;  // Agente de navegación para el movimiento
        public GameObject projectilePrefab;  // Prefab del proyectil
        public Transform shootPoint;  // Punto desde donde se disparan los proyectiles
        public float shootInterval = 2f;  // Intervalo de tiempo entre disparos
        public List<Transform> patrolPoints;  // Lista de puntos de patrullaje
        public float patrolSpeed = 2f;  // Velocidad de patrullaje
        public int currentPatrolIndex = 0;
        public NavMeshSurface navMeshSurface;  // Referencia al NavMeshSurface

        private Queue<ICommand> commandQueue = new Queue<ICommand>();
        private float shootTimer;
        private bool isReversing = false;  // Variable para controlar la dirección del recorrido
        private Vector3 initialPosition;  // Posición inicial del robot
        private bool isPlayerInRange = false;  // Variable para controlar si el jugador está en rango

        void Start()
        {
            // Guardar la posición inicial del robot
            initialPosition = transform.position;

            // Configuración de comandos según el tipo de robot
            switch (tipoDeRobot)
            {
                case RobotType.Perseguidor:
                    AddCommand(new ChaseCommand());
                    break;
                case RobotType.Disparador:
                    AddCommand(new ShootCommand());
                    break;
                case RobotType.Patrullador:
                    AddCommand(new PatrolCommand());
                    break;
            }

            // Asignar el destino inicial del agente
            if (tipoDeRobot == RobotType.Perseguidor && agent != null && player != null)
            {
                agent.SetDestination(player.position);
            }

            shootTimer = shootInterval;
        }

        void Update()
        {
            // Ejecuta los comandos en cada frame
            ExecuteCommands();

            // Actualizar el destino del agente para que siga al jugador si está en el área navegable
            if (tipoDeRobot == RobotType.Perseguidor && agent != null && player != null)
            {
                if (IsPlayerInNavMeshSurface())
                {
                    agent.SetDestination(player.position);
                    isPlayerInRange = true;
                }
                else if (isPlayerInRange)
                {
                    agent.SetDestination(initialPosition);
                    isPlayerInRange = false;
                }
            }

            // Lógica para disparar proyectiles
            if (tipoDeRobot == RobotType.Disparador)
            {
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0f && CanShoot())
                {
                    ShootProjectile();
                    shootTimer = shootInterval;
                }
            }

            // Ejecutar el comando de patrullaje continuamente
            if (tipoDeRobot == RobotType.Patrullador)
            {
                AddCommand(new PatrolCommand());
            }
        }

        // Método para añadir un comando a la cola
        public void AddCommand(ICommand command)
        {
            commandQueue.Enqueue(command);
        }

        // Método para ejecutar el siguiente comando en la cola
        public void ExecuteCommands()
        {
            if (commandQueue.Count > 0)
            {
                ICommand currentCommand = commandQueue.Dequeue();
                currentCommand.Execute(this);
            }
        }

        // Método para disparar proyectiles
        public void ShootProjectile()
        {
            if (projectilePrefab != null && shootPoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = shootPoint.forward * 10f;  // Ajusta la velocidad del proyectil según sea necesario
                }
                Debug.Log("Robot disparando al jugador");
            }
        }

        // Determina si el robot puede disparar (se puede ajustar a un temporizador)
        public bool CanShoot()
        {
            // Aquí puedes implementar lógica de cooldown o condiciones para disparar
            return true;
        }

        // Configura el siguiente punto de patrullaje
        public void SetNextPatrolPoint()
        {
            if (patrolPoints.Count == 0) return;

            if (!isReversing)
            {
                currentPatrolIndex++;
                if (currentPatrolIndex >= patrolPoints.Count)
                {
                    currentPatrolIndex = patrolPoints.Count - 1;
                    isReversing = true;
                }
            }
            else
            {
                currentPatrolIndex--;
                if (currentPatrolIndex < 0)
                {
                    currentPatrolIndex = 0;
                    isReversing = false;
                }
            }

            Debug.Log("Cambiando al siguiente punto de patrullaje");
        }

        // Verifica si el robot está en el punto de patrullaje actual
        public bool IsAtPatrolPoint()
        {
            if (patrolPoints.Count == 0) return false;

            return Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.5f;
        }

        // Verifica si el jugador está dentro del área navegable del NavMeshSurface
        private bool IsPlayerInNavMeshSurface()
        {
            NavMeshHit hit;
            return NavMesh.SamplePosition(player.position, out hit, 1.0f, NavMesh.AllAreas);
        }

        // Método para manejar colisiones
        private void OnCollisionEnter(Collision collision)
        {
            if (tipoDeRobot == RobotType.Patrullador && collision.gameObject.CompareTag("Bloque"))
            {
                // Invertir la dirección de patrullaje
                isReversing = !isReversing;
                SetNextPatrolPoint();
                Debug.Log("Robot patrullador invirtiendo dirección debido a colisión con bloque");
            }
        }
    }
}
