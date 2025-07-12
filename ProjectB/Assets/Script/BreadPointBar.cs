using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BreadPointBar : MonoBehaviour
{
    public Slider breadPointSlider;
    public int maxBreadPoint = 50;
    public int currentBreadPoint;
    [SerializeField] private TextMeshProUGUI breadPointText;

    // Start is called before the first frame update
    void Start()
    {
        currentBreadPoint = 10;
        breadPointSlider.maxValue = maxBreadPoint;
        breadPointSlider.value = currentBreadPoint;
        breadPointText.text = currentBreadPoint.ToString() + " / " + maxBreadPoint.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            AddBreadPoint(10);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            CostBreadPoint(1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            CostBreadPoint(5);
        }
    }

    public void AddBreadPoint(int BreadPoint)
    {
        currentBreadPoint += BreadPoint;
        breadPointSlider.value = currentBreadPoint;
        breadPointText.text = currentBreadPoint.ToString() + " / " + maxBreadPoint.ToString();
        if (currentBreadPoint >= maxBreadPoint)
        {
            currentBreadPoint = maxBreadPoint;
            breadPointSlider.value = currentBreadPoint;
            breadPointText.text = currentBreadPoint.ToString() + " / " + maxBreadPoint.ToString();
        }
    }
    public void CostBreadPoint(int BreadPoint)
    {
        if(currentBreadPoint >= BreadPoint)
        {
            currentBreadPoint -= BreadPoint;
            breadPointSlider.value = currentBreadPoint;
            breadPointText.text = currentBreadPoint.ToString() + " / " + maxBreadPoint.ToString();
        }
        else
        {
            Debug.Log("빵 포인트가 모자랍니다");
        }

    }
}
