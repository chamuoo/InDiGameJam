using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI systemText;


    // Start is called before the first frame update
    void Start()
    {
        waveText.text = "";
        systemText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
