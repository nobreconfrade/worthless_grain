using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUps : MonoBehaviour
{
    private GameObject[] customerCatSpawnerObj;
    private VisualElement catRushButton;
    private VisualElement doubleEarningsButton;
    private ProgressBar catRushBar;
    private ProgressBar doubleEarningsBar;
    private UIDocument mainDoc;
    private float timer = 0f;
    private float updateRate = 1f;
    public bool isDoubleEarnings = false;
    void Start()
    {
        customerCatSpawnerObj = GameObject.FindGameObjectsWithTag("CustomerSpawner");
        mainDoc = GetComponent<UIDocument>();

        catRushButton = mainDoc.rootVisualElement.Q("CatRushButton");
        doubleEarningsButton = mainDoc.rootVisualElement.Q("DoubleEarningsButton");
        catRushBar = mainDoc.rootVisualElement.Q<ProgressBar>("CatRushBar");
        doubleEarningsBar = mainDoc.rootVisualElement.Q<ProgressBar>("DoubleEarningsBar");
        

        catRushButton.RegisterCallback<ClickEvent>(OnCatRushButtonClick);
        doubleEarningsButton.RegisterCallback<ClickEvent>(OnDoubleEarningsButtonClick);
    }

    void Update()
    {
        if (timer < updateRate)
        {
            timer += Time.deltaTime;
        } else {
            UpdateCatRushBar();
            timer = 0;
        }
    }

    void UpdateCatRushBar()
    {
        if(catRushBar.value < 100)
        {
            catRushBar.value += 0.83333f;
        }
    }

    public void UpdateDoubleEarnings(int value)
    {
        if(doubleEarningsBar.value < 100)
        {
            if (doubleEarningsBar.value + value > 100)
            {
                doubleEarningsBar.value = 100;
            } else {
                doubleEarningsBar.value += value;
            }
        }
    }

    void OnCatRushButtonClick(ClickEvent clk)
    {
        if(catRushBar.value >= 100)
        {
            foreach (var obj in customerCatSpawnerObj)
            {
                CustomerCatSpawner customerCatSpawner = obj.GetComponent<CustomerCatSpawner>();
                StartCoroutine(customerCatSpawner.CatRush());
            }
            catRushBar.value = 0;
        }
    }

    void OnDoubleEarningsButtonClick(ClickEvent clk)
    {
        if(doubleEarningsBar.value >= 100 && !isDoubleEarnings)
        {
            StartCoroutine(DoubleEarnings());
        }
    }

    IEnumerator DoubleEarnings()
    {
        VisualElement box = mainDoc.rootVisualElement.Q("DoubleEarningsBox");

        isDoubleEarnings = true;
        box.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(30f);
        box.style.display = DisplayStyle.None;
        doubleEarningsBar.value = 0f;
        isDoubleEarnings = false;
    }

}
