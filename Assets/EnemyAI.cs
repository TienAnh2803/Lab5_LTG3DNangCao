using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    private enum State { Moving, Jumping, SpeedBoost }
    private State currentState;

    private Transform target;
    public float speed = 3.5f;
    public float jumpHeight = 2f;
    public float speedBoostMultiplier = 2f;
    public float speedBoostDuration = 2f;

    private NavMeshAgent agent;
    private Vector3 startPosition;
    private bool specialActionTriggered = false;

    void Start()
    {
        target =GameObject.Find("EndPoint").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        startPosition = transform.position;
        currentState = State.Moving;
    }

    void Update()
    {
        if (target == null) return;

        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        float totalDistance = Vector3.Distance(startPosition, target.position);
        
        if (!specialActionTriggered && distanceTraveled >= totalDistance / 3)
        {
            specialActionTriggered = true;
            int randomAction = Random.Range(0, 2); // 0 = Jump, 1 = SpeedBoost
            if (randomAction == 0)
                StartCoroutine(JumpAction());
            else
                StartCoroutine(SpeedBoostAction());
        }
        
        if (currentState == State.Moving)
        {
            agent.destination = target.position;
        }
         if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GameOver();
        }
    }

    IEnumerator JumpAction()
    {
        currentState = State.Jumping;
        agent.isStopped = true;

        float jumpTime = 0.5f;
        float timer = 0f;
        Vector3 startPosition = transform.position;
        Vector3 peakPosition = startPosition + new Vector3(0, jumpHeight, 0);

        while (timer < jumpTime)
        {
            transform.position = Vector3.Lerp(startPosition, peakPosition, timer / jumpTime);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        while (timer < jumpTime)
        {
            transform.position = Vector3.Lerp(peakPosition, startPosition, timer / jumpTime);
            timer += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = false;
        currentState = State.Moving;
    }

    IEnumerator SpeedBoostAction()
    {
        currentState = State.SpeedBoost;
        agent.speed *= speedBoostMultiplier;
        yield return new WaitForSeconds(speedBoostDuration);
        agent.speed = speed;
        currentState = State.Moving;
    }
    private void GameOver()
    {
        Debug.Log("GameOver");
        Destroy(gameObject);
    }
}
