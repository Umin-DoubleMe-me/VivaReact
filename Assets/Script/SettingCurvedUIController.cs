using CurvedUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingCurvedUIController : MonoBehaviour
{
    CurvedUISettings curvedUISetting;

    // Start is called before the first frame update
    void Start()
    {
        curvedUISetting = GetComponent<CurvedUISettings>();
    }

    // Update is called once per frame
    void Update()
	{
	}
}
