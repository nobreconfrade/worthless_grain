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
    private VisualElement grillUpgrader;
    private VisualElement storageUpgrader;
    private VisualElement sodaMachineUpgrader;
    private VisualElement[] grillStars = new VisualElement[5];
    private VisualElement[] storageStars = new VisualElement[5];
    private VisualElement[] sodaMachineStars = new VisualElement[5];
    private Label valueChef;
    private Label grillValue;
    private Label storageValue;
    private Label sodaMachineValue;
    public Sprite unpressedButton;
    public Sprite pressedButton;
    private int[] ChefSpeedPrice = {25, 50, 75, 125, 200, 450};
    
    private int[] upgraderPrices = {20, 40, 60, 80, 120};

    void Start()
    {
        workerLogic = GameObject.FindGameObjectWithTag("ChefCat").GetComponent<WorkerLogic>();
        coinCounter = GetComponent<CoinCounter>();
        mainDoc = GetComponent<UIDocument>();

        QueriesInitialization();

        changeChefSpeedValue();
        changeGrillUpCost();
        changeStorageUpCost();
        changeSodaMachineUpCost();
        chefButton.RegisterCallback<ClickEvent>(OnChefButtonClick);
        chefButton.RegisterCallback<PointerDownEvent>(PointerDownSpriteChange);
        chefButton.RegisterCallback<PointerUpEvent>(PointerUpSpriteChange);

        grillUpgrader.RegisterCallback<ClickEvent>(OnGrillUpgraderClick);
        storageUpgrader.RegisterCallback<ClickEvent>(OnStorageUpgraderClick);
        sodaMachineUpgrader.RegisterCallback<ClickEvent>(OnSodaMachineUpgraderClick);
    }

    private void changeChefSpeedValue()
    {
        valueChef.text = ChefSpeedPrice[workerLogic.getAgentSpeedLevel()].ToString();
    }
    private void changeGrillUpCost()
    {
        int iterLevel = workerLogic.getGrillIterLevel();
        if (iterLevel < upgraderPrices.Length)
        {
            grillValue.text = upgraderPrices[iterLevel].ToString();
        } else {
            grillValue.text = "MAX";
        }
    }
    private void changeStorageUpCost()
    {
        int iterLevel = workerLogic.getStorageIterLevel();
        if (iterLevel < upgraderPrices.Length)
        {
            storageValue.text = upgraderPrices[iterLevel].ToString();
        } else {
            storageValue.text = "MAX";
        }
    }
    private void changeSodaMachineUpCost()
    {
        int iterLevel = workerLogic.getSodaMachineIterLevel();
        if (iterLevel < upgraderPrices.Length)
        {
            sodaMachineValue.text = upgraderPrices[iterLevel].ToString();
        } else {
            sodaMachineValue.text = "MAX";
        }
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
        if (
            coinCounter.doTransaction(ChefSpeedPrice[workerLogic.getAgentSpeedLevel()])
            )
        {
            workerLogic.IncreaseChefSpeed();
            changeChefSpeedValue();
            Debug.Log("chef speed increased");
        }
        else 
        {
            // create a responsive feedback
            Debug.Log("not enough coins stranger");
        }
    }

    void OnGrillUpgraderClick(ClickEvent clk)
    {
        int iterLevel = workerLogic.getGrillIterLevel();
        if (iterLevel >= upgraderPrices.Length)
        {
            return;
        }
        if (coinCounter.doTransaction(upgraderPrices[iterLevel]))
        {
            workerLogic.IncreaseGrillIterLevel();
            changeGrillUpCost();
            grillStars[workerLogic.getGrillIterLevel() - 1].style.display = DisplayStyle.Flex;
            Debug.Log("grill iter level increased");
        }
        else 
        {
            // create a responsive feedback
            Debug.Log("not enough coins for grill upgrade stranger");
        }
    }
    void OnStorageUpgraderClick(ClickEvent clk)
    {
        int iterLevel = workerLogic.getStorageIterLevel();
        if (iterLevel >= upgraderPrices.Length)
        {
            return;
        }
        if (coinCounter.doTransaction(upgraderPrices[iterLevel]))
        {
            workerLogic.IncreaseStorageIterLevel();
            changeStorageUpCost();
            storageStars[workerLogic.getStorageIterLevel() - 1].style.display = DisplayStyle.Flex;
            Debug.Log("storage iter level increased");
        }
        else 
        {
            // create a responsive feedback
            Debug.Log("not enough coins for storage upgrade stranger");
        }
    }
    void OnSodaMachineUpgraderClick(ClickEvent clk)
    {
        int iterLevel = workerLogic.getSodaMachineIterLevel();
        if (iterLevel >= upgraderPrices.Length)
        {
            return;
        }
        if (coinCounter.doTransaction(upgraderPrices[iterLevel]))
        {
            workerLogic.IncreaseSodaMachineIterLevel();
            changeSodaMachineUpCost();
            sodaMachineStars[workerLogic.getSodaMachineIterLevel() - 1].style.display = DisplayStyle.Flex;
            Debug.Log("soda machine iter level increased");
        }
        else 
        {
            // create a responsive feedback
            Debug.Log("not enough coins for soda machine upgrade stranger");
        }
    }

    void QueriesInitialization()
    {
        chefButton = mainDoc.rootVisualElement.Q("PlusButtonChef");
        grillUpgrader = mainDoc.rootVisualElement.Q("GrillUpgrader");
        storageUpgrader = mainDoc.rootVisualElement.Q("StorageUpgrader");
        sodaMachineUpgrader = mainDoc.rootVisualElement.Q("SodaMachineUpgrader");

        valueChef = mainDoc.rootVisualElement.Q<Label>("ValueChef");
        grillValue = mainDoc.rootVisualElement.Q<Label>("GrillValue");
        storageValue = mainDoc.rootVisualElement.Q<Label>("StorageValue");
        sodaMachineValue = mainDoc.rootVisualElement.Q<Label>("SodaMachineValue");

        for (int i = 1; i <= 5; i++)
        {
            grillStars[i-1] = mainDoc.rootVisualElement.Q("GrillStar" + i);
            storageStars[i-1] = mainDoc.rootVisualElement.Q("StorageStar" + i);
            sodaMachineStars[i-1] = mainDoc.rootVisualElement.Q("SodaMachineStar" + i);
        }
    }
}
