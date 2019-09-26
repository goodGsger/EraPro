using UnityEngine;
using System.Collections;
using Framework;

namespace Framework
{
    public class AppBehaviour : MonoBehaviour
    {
        private void Update()
        {
            App.inst.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            App.inst.Destroy();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                App.inst.OnApplicationGetFocus();
            else
                App.inst.OnApplicationLoseFocus();
        }

        private void OnApplicationQuit()
        {
            App.inst.OnApplicationQuit();
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        private void OnGUI()
        {
            App.inst.OnGUI();
        }
#endif
    }
}
