using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUps : MonoBehaviour
{
    private GameObject[] customerCatSpawnerObj;
    private VisualElement catRushButton;
    private UIDocument mainDoc;

    void Start()
    {
        customerCatSpawnerObj = GameObject.FindGameObjectsWithTag("CustomerSpawner");
        mainDoc = GetComponent<UIDocument>();

        catRushButton = mainDoc.rootVisualElement.Q("CatRushButton");

        catRushButton.RegisterCallback<ClickEvent>(OnCatRushButtonClick);
    }

    void OnCatRushButtonClick(ClickEvent clk)
    {
        foreach (var obj in customerCatSpawnerObj)
        {
            CustomerCatSpawner customerCatSpawner = obj.GetComponent<CustomerCatSpawner>();
            StartCoroutine(customerCatSpawner.CatRush());
        }
    }

}
