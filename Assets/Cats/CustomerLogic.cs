using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLogic : MonoBehaviour
{
    private GameObject[] foods;
    private int selectedFood;
    private WorkerLogic workerLogic;
    private PowerUps powerUps;
    private CoinCounter coinCounter;
    private Animator animator;
    private CircleCollider2D circleCollider2D;
    private SpriteRenderer spriteRenderer;
    private float raycastDistance = 3f;
    private Color[] colors = {Color.red, Color.gray, Color.blue, Color.white, Color.green};
    private bool ordered;
    private bool onQueue;
    public float catSpeed;
    public Rigidbody2D catRigidBody;
    public GameObject burguer;
    public GameObject soda;
    
    void Start()
    {
        workerLogic = GameObject.FindGameObjectWithTag("ChefCat").GetComponent<WorkerLogic>();
        GameObject mainUI = GameObject.FindGameObjectWithTag("MainUI"); 
        
        coinCounter = mainUI.GetComponent<CoinCounter>();
        powerUps = mainUI.GetComponent<PowerUps>();
        animator = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Enable raycast verification
        Physics2D.queriesStartInColliders = false;

        foods = new GameObject[] {burguer, soda};
        selectedFood = Random.Range(0, foods.Length);
        int colorNum = Random.Range(0, colors.Length);
        spriteRenderer.color = colors[colorNum];

        animator.SetTrigger("startWiggle");
        catRigidBody.velocity = Vector2.down * catSpeed;
    }

    void Update()
    { 
        if(transform.position.x > 55f)
        {
            Destroy(gameObject);
        }
        // Perform the raycast
        // Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * raycastDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), raycastDistance);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("CustomerCat") && !ordered)
            {
                catRigidBody.velocity = Vector2.zero;
                onQueue = true;
            }
        } else if (hit.collider == null && onQueue) {
            catRigidBody.velocity = Vector2.down * catSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collidingObj) {
        if (collidingObj.gameObject.tag == "Table")
        {
            foods[selectedFood].SetActive(true);
            workerLogic.OrderFood(foods[selectedFood]);
            catRigidBody.velocity = Vector2.zero;
            animator.SetTrigger("stopWiggle");
            ordered = true;
        }
    }

    public void orderCompleted()
    {
        foods[selectedFood].SetActive(false);
        circleCollider2D.isTrigger = true;
        catRigidBody.velocity = new Vector2(1, 0.25f) * catSpeed;
        animator.SetTrigger("startWiggle");
        onQueue = false;
        int foodPrice = 0;
        if (foods[selectedFood].name == "burguer")
        {
            foodPrice = 30;
        } 
        else if (foods[selectedFood].name == "soda")
        {
            foodPrice = 20;
        }
        if (powerUps.isDoubleEarnings)
        {
            foodPrice *= 2;
        } else {
            powerUps.UpdateDoubleEarnings(foodPrice);
        }
        coinCounter.increaseCoinValue(foodPrice);
    }

}


    // private void WiggleWiggle()
    // {
    //     if (walkState == 1)
    //     {
    //         if (leftWiggle)
    //         {
    //             transform.eulerAngles = new Vector3(0, 0, wiggleOffset);
    //             leftWiggle = false;
    //         } 
    //         else 
    //         {
    //             transform.eulerAngles = new Vector3(0, 0, -wiggleOffset);
    //             leftWiggle = true;
    //         }
    //     }
    // }