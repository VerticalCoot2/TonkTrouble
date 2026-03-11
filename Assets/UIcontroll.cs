using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIcontroll : MonoBehaviour
{
    [SerializeField] TMP_Text roundNum;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        roundNum.text = "Round: " + GameLogic.round;
    }
}
