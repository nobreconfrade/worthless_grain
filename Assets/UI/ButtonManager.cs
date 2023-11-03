using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : MonoBehaviour
{
    private WorkerLogic workerLogic;
    private CoinCounter coinCounter;
    private UIDocument mainDoc;
    private VisualElement chefButton;
    private Label valueChef;
    public Sprite unpressedButton;
    public Sprite pressedButton;
    private int[] ChefSpeedPrice = {25, 50, 75, 125, 200, 450};

    void Start()
    {
        workerLogic = GameObject.FindGameObjectWithTag("ChefCat").GetComponent<WorkerLogic>();
        coinCounter = GetComponent<CoinCounter>();
        mainDoc = GetComponent<UIDocument>();

        chefButton = mainDoc.rootVisualElement.Q("PlusButtonChef");
        valueChef = mainDoc.rootVisualElement.Q<Label>("ValueChef");

        changeChefSpeedValue();
        chefButton.RegisterCallback<ClickEvent>(OnChefButtonClick);
        chefButton.RegisterCallback<PointerDownEvent>(PointerDownSpriteChange);
        chefButton.RegisterCallback<PointerUpEvent>(PointerUpSpriteChange);
    }

    private void changeChefSpeedValue()
    {
        valueChef.text = "-" + ChefSpeedPrice[workerLogic.agentSpeedLevel];
    }

    private void PointerUpSpriteChange(PointerUpEvent evt)
    {
        chefButton.style.backgroundImage = new StyleBackground(unpressedButton);

    }
    private void PointerDownSpriteChange(PointerDownEvent evt)
    {
        chefButton.style.backgroundImage = new StyleBackground(pressedButton);
    }

    void OnChefButtonClick(ClickEvent clk)
    {
        if (coinCounter.doTransaction(ChefSpeedPrice[workerLogic.agentSpeedLevel]))
        {
            workerLogic.IncreaseChefSpeed();
            Debug.Log("chef speed increased");
        }
        else 
        {
            // create a responsive feedback
            Debug.Log("not enough coins stranger");
        }
    }
}
