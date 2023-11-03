using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class CoinCounter : MonoBehaviour
{
    private UIDocument mainDoc;
    private Label coinValue;
    private Label coinTarget;
    
    void Start()
    {
        mainDoc = GetComponent<UIDocument>();
        coinValue = mainDoc.rootVisualElement.Q<Label>("CoinValue");
        coinTarget = mainDoc.rootVisualElement.Q<Label>("CoinTarget");
        setCoinTargetValue();
    }

    void Update()
    {
        
    }

    void setCoinTargetValue()
    {
        int[] levelTarget = {150, 400, 750, 999};
        // Logic to choose level based on scene
        var level = 0;
        coinTarget.text = levelTarget[level].ToString();
    }

    public void increaseCoinValue(int coins)
    {
        int current = int.Parse(coinValue.text);
        coinValue.text = (current + coins).ToString("D3");
    }
}
