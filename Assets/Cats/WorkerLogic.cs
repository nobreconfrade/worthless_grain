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
    public int[] machinesIterTime = {10, 9, 8, 7, 5, 3};
    private int[] agentSpeedValue = {4, 6, 8, 10, 15, 20};

    private AgentLevels agentLevels = new AgentLevels();

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
        if (agentLevels.agentSpeed + 1 < agentSpeedValue.Length)
        {
            agentLevels.agentSpeed += 1;
        }
    }
    public void IncreaseGrillIterLevel()
    {
        if (agentLevels.grillIter + 1 < machinesIterTime.Length)
        {
            agentLevels.grillIter += 1;
        }
    }
    public void IncreaseStorageIterLevel()
    {
        if (agentLevels.storageIter + 1 < machinesIterTime.Length)
        {
            agentLevels.storageIter += 1;
        }
    }
    public void IncreaseSodaMachineIterLevel()
    {
        if (agentLevels.sodaMachineIter + 1 < machinesIterTime.Length)
        {
            agentLevels.sodaMachineIter += 1;
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
        if(agent.acceleration != agentSpeedValue[agentLevels.agentSpeed] && agent.speed != agentSpeedValue[agentLevels.agentSpeed])
        {
            agent.acceleration = agentSpeedValue[agentLevels.agentSpeed];
            agent.speed = agentSpeedValue[agentLevels.agentSpeed];
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
                WaitForSeconds wait = new WaitForSeconds(machinesIterTime[agentLevels.sodaMachineIter]);
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
                    WaitForSeconds wait = new WaitForSeconds(machinesIterTime[agentLevels.storageIter]);
                    yield return wait;
                    Debug.Log("Itens retrieved! "+ Time.time.ToString());
                    storageDone = true;
                    agent.SetDestination(grill.transform.position + new Vector3(5, 0, 0));
                } else {
                    WaitForSeconds wait = new WaitForSeconds(machinesIterTime[agentLevels.grillIter]);
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

    
    public int getAgentSpeedLevel(){
        return agentLevels.agentSpeed;
    }
    public int getGrillIterLevel(){
        return agentLevels.grillIter;
    }    
    public int getStorageIterLevel(){
        return agentLevels.storageIter;
    }    public int getSodaMachineIterLevel(){
        return agentLevels.sodaMachineIter;
    }

    class AgentLevels 
    {
        public int sodaMachineIter {get;set;} = 0;
        public int storageIter {get;set;} = 0;
        public int grillIter {get;set;} = 0;
        public int agentSpeed {get;set;} = 0;
    }
}
