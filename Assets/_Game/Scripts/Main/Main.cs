using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        App.inst.Init();
        App.inst.InitDefaultManagers();
        App.logManager.enabled = true;
        App.logManager.isPrint = true;
    }
}
