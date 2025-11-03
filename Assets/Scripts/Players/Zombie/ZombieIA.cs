using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ZombieIA : MonoBehaviour
{
    // Quantitat de dany que fa el zombie al jugador.
    public float amount = 10f;
    
    // Component NavMeshAgent per controlar la navegació del zombie.
    public NavMeshAgent agent;
    
    // Referència al objectiu del zombie (normalment el jugador).
    public GameObject target;
    
    // Distància a la qual el zombie comença a perseguir el jugador.
    public float chaseRange = 5f;
    
    // Distància a la qual el zombie pot atacar el jugador.
    public float attackRange = 1.3f;

    // Velocitat del zombie quan està patrullant.
    public float patrolSpeed = 1.5f;
    
    // Velocitat del zombie quan està perseguint el jugador.
    public float chaseSpeed = 4.5f;

    // Component Animator per controlar les animacions del zombie.
    public Animator anim;
    
    // Variable per determinar la rutina de patrulla actual.
    public int routine;
    
    // Temporitzador per controlar canvis en la patrulla.
    public float timer;

    // Temps mínim entre atacs consecutius.
    public float timeBetweenAttacks = 2f;
    
    // Temps de l'últim atac realitzat.
    private float lastAttackTime = 0f;
    
    // Flag per indicar si el zombie està atacant actualment.
    private bool isAttacking = false;

    // Punt on es reubica el jugador després de ser atacat.
    public Transform PlayerSpawnPoint;
    
    // Controlador del jugador per desactivar-lo temporalment durant el respawn.
    public CharacterController PlayerController;

    // Flag per evitar aplicar dany múltiples vegades durant un mateix atac.
    private bool damageApplied = false;

    void Start()
    {
        // Obtenir els components necessaris al iniciar.
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");

        // Configurar la velocitat inicial del agent.
        if (agent != null)
        {
            agent.speed = patrolSpeed;
        }
    }

    void Update()
    {
        // Sortir si no hi ha objectiu o agent configurat.
        if (target == null || agent == null) return;
        EnemyBehavior();
    }

    public void EnemyBehavior()
    {
        // Calcular la distància actual entre el zombie i el jugador.
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // Decidir el comportament basat en la distància amb el jugador.
        if (distance > chaseRange)
        {
            // El jugador està fora del rang de persecució, patrullar.
            Patrol();
        }
        else if (distance <= attackRange)
        {
            // El jugador està a rang d'atac, atacar.
            Attack();
        }
        else
        {
            // El jugador està dins del rang de persecució però fora d'atac, perseguir.
            Chase();
        }
    }

    void Patrol()
    {
        // No patrullar si està atacant.
        if (isAttacking) return;

        // Activar el moviment del agent.
        agent.isStopped = false;
        agent.speed = patrolSpeed;

        // Configurar animacions per patrulla.
        anim.SetBool("walk", true);
        anim.SetBool("run", false);

        // Incrementar el temporitzador.
        timer += Time.deltaTime;
        
        // Canviar de rutina cada 2 segons.
        if (timer >= 2)
        {
            routine = Random.Range(0, 2);
            timer = 0;

            // Si la rutina és 0, moure's a un punt aleatori.
            if (routine == 0)
            {
                Vector3 randomPoint = Random.insideUnitSphere * 10f + transform.position;
                NavMeshHit hit;
                
                // Trobar una posició vàlida en el NavMesh.
                if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
        }

        // Si ha arribat al destí, aturar l'animació de caminar.
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            anim.SetBool("walk", false);
        }
    }

    void Chase()
    {
        // No perseguir si està atacant.
        if (isAttacking) return;

        // Activar el moviment del agent.
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        
        // Establir el jugador com a destí.
        agent.SetDestination(target.transform.position);

        // Configurar animacions per persecució.
        anim.SetBool("walk", false);
        anim.SetBool("run", true);
    }

    void Attack()
    {
        // Aturar el moviment del zombie durant l'atac.
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // Iniciar atac si no està atacant i ha passat el temps entre atacs.
        if (!isAttacking && Time.time - lastAttackTime >= timeBetweenAttacks)
        {
            StartAttack();
        }
        else
        {
            // Mentre espera el cooldown, mantenir animacions aturades.
            anim.SetBool("walk", false);
            anim.SetBool("run", false);
        }
    }

    void StartAttack()
    {
        // Marcar que està atacant.
        isAttacking = true;
        
        // Reiniciar el flag de dany per aquest atac.
        damageApplied = false;
        
        // Actualitzar el temps de l'últim atac.
        lastAttackTime = Time.time;

        // Activar l'animació d'atac.
        anim.SetTrigger("attack");
        
        // Programar l'aplicació del dany a la meitat de l'animació.
        Invoke("ApplyDamage", 0.5f);

        // Programar la finalització de l'atac quan acabi l'animació.
        Invoke("FinishAttack", 1.2f);
    }

    void FinishAttack()
    {
        // Marcar que ha acabat l'atac.
        isAttacking = false;
    }

    void ApplyDamage()
    {
        // Sortir si ja s'ha aplicat el dany en aquest atac.
        if (damageApplied) return;

        // Verificar que el jugador encara està a rang d'atac.
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRange + 0.5f)
        {
            // Marcar que s'ha aplicat el dany.
            damageApplied = true;

            // Programar el reinici del jugador després de l'animació.
            Invoke("ResetPlayer", 0.7f);

            // Aplicar dany només si estem al nivell 2.
            if (SceneManager.GetActiveScene().buildIndex == (int)GameManager.GameScene.LEVEL_2)
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(amount);
                }
            }
        }
    }

    void ResetPlayer()
    {
        // Reubicar el jugador al punt de spawn.
        if (PlayerController != null && PlayerSpawnPoint != null && target != null)
        {
            PlayerController.enabled = false;
            target.transform.position = PlayerSpawnPoint.position;
            PlayerController.enabled = true;
        }
    }
}