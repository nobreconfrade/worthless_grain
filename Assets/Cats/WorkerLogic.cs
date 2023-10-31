using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WorkerLogic : MonoBehaviour
{
    private List<GameObject> toPrepare = new List<GameObject>();
    private bool cooking = false;
    private NavMeshAgent agent;
    private float distanceToStop = 2.5f;
    private CustomerLogic customerLogic;
    public GameObject grill;
    public GameObject storage; 
    public GameObject sodaMachine;
    public GameObject table;
    public Animator animator;
    public float sodaMachineIterSeconds = 10f;
    public float storageIterSeconds = 10f;
    public float grillIterSeconds = 10f;
    private int[] agentSpeed = {4, 6, 8, 10, 15, 20};
    public int agentSpeedLevel = 0;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        DefineAgentSpeed();
    }

    void Update()
    {
        if(agent.remainingDistance < distanceToStop && agent.velocity != Vector3.zero)
        {
            agent.SetDestination(transform.position);
            float distanceToTable = Vector2.Distance(transform.position, table.transform.position + new Vector3(-9.77f, -2.47f, 0));
            if (distanceToTable <= distanceToStop)
            {
                customerLogic = toPrepare[0].transform.parent.GetComponent<CustomerLogic>();
                customerLogic.orderCompleted();
                toPrepare.RemoveAt(0);
                cooking = false;
                animator.SetBool("cooking", cooking);
            }
        }
        if (toPrepare.Any() && !cooking)
        {
            prepareFood();
        }
        DefineAgentSpeed();
    }

    public void OrderFood(GameObject food)
    {
        toPrepare.Add(food);
    }

    public void IncreaseChefSpeed()
    {
        if (agentSpeedLevel + 1 < agentSpeed.Length)
        {
            agentSpeedLevel += 1;
        }
    }

    private void prepareFood()
    {
        cooking = true;
        animator.SetBool("cooking", cooking);
        string nextOrder = toPrepare[0].name;
        if (nextOrder == "soda")
        {
            Debug.Log("preparing soda");
            StartCoroutine(sodaPrepare());

        }
        else if (nextOrder == "burguer")
        {
            Debug.Log("preparing burguer");
            StartCoroutine(burguerPrepare());
        }
    }

    void DefineAgentSpeed()
    {
        if(agent.acceleration != agentSpeed[agentSpeedLevel] && agent.speed != agentSpeed[agentSpeedLevel])
        {
            agent.acceleration = agentSpeed[agentSpeedLevel];
            agent.speed = agentSpeed[agentSpeedLevel];
        }
    }

    IEnumerator sodaPrepare() 
    {
        agent.SetDestination(sodaMachine.transform.position + new Vector3(-2.25f, -1.73f, 0));
        Debug.Log("Chef going to Soda Machine!");
        bool preparing = true;
        while (preparing)
        {
            yield return new WaitForSeconds(0.1f);
            if(agent.remainingDistance < distanceToStop && agent.velocity == Vector3.zero)
            {
                Debug.Log("Chef arrived! " + Time.time.ToString());
                WaitForSeconds wait = new WaitForSeconds(sodaMachineIterSeconds);
                yield return wait;
                Debug.Log("Soda ready! "+ Time.time.ToString());
                preparing = false;
            }
        }
        deliverFood();
    }

    IEnumerator burguerPrepare()
    {
        agent.SetDestination(storage.transform.position + new Vector3(0, 6, 0));
        Debug.Log("Chef going to storage!");
        bool preparing = true;
        bool storageDone = false;
        while (preparing)
        {
            yield return new WaitForSeconds(0.1f);
            if(agent.remainingDistance < distanceToStop && agent.velocity == Vector3.zero)
            {
                Debug.Log("Chef arrived! " + Time.time.ToString());
                if (!storageDone) 
                {
                    WaitForSeconds wait = new WaitForSeconds(storageIterSeconds);
                    yield return wait;
                    Debug.Log("Itens retrieved! "+ Time.time.ToString());
                    storageDone = true;
                    agent.SetDestination(grill.transform.position + new Vector3(5, 0, 0));
                } else {
                    WaitForSeconds wait = new WaitForSeconds(grillIterSeconds);
                    yield return wait;
                    Debug.Log("Burguer done! "+ Time.time.ToString());
                    preparing = false;
                }
            }
        }
        deliverFood();
    }

    private void deliverFood()
    {
        Debug.Log("Delivering");
        agent.SetDestination(table.transform.position + new Vector3(-9.77f, -2.47f, 0));
    }
}
