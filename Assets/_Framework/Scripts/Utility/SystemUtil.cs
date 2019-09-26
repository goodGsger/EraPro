using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public static class SystemUtil
    {
#if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void copyTextToClipboard(string text);
        [DllImport("__Internal")]
        private static extern string getTextFromClipboard();
        [DllImport("__Internal")]
        private static extern string getDeviceId();
        [DllImport("__Internal")]
        private static extern string getOperatorName();
        [DllImport("__Internal")]
        private static extern string getCpu();
        [DllImport("__Internal")]
        private static extern bool getIsjailbreak();
        [DllImport("__Internal")]
        private static extern string getIdfa();
        [DllImport("__Internal")]
        private static extern string getIdfv();
        [DllImport("__Internal")]
        private static extern float getCpuUsage();
        [DllImport("__Internal")]
        private static extern long getMemoryUsage();
#endif

        /// <summary>
        /// 获取当前网络IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            //return Network.player.ipAddress;
            return null;
        }

        /// <summary>
        /// 向剪贴板中添加文本
        /// </summary>
        /// <param name="str"></param>
        public static void CopyTextToClipboard(string str)
        {
            if (str == null || str == "")
                return;
#if !UNITY_EDITOR
#if UNITY_ANDROID
            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                AndroidJavaObject currentActivity = getApplicationContext();
                if (currentActivity != null)
                    GameTools.CallStatic("copyTextToClipboard", currentActivity, str);
            }
#elif UNITY_IPHONE
            copyTextToClipboard(str);
#endif
#endif
        }
        /// <summary>
        /// 从剪贴板中获取文本
        /// </summary>
        /// <returns></returns>
        public static string GetTextFromClipboard()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                return GameTools.CallStatic<string>("getTextFromClipboard");
            }
#elif UNITY_IPHONE
            return getTextFromClipboard();
#endif
#endif
            return "";
        }

        /// <summary>
        /// 获取设备号
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceID()
        {
#if UNITY_IOS
		return getDeviceId();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            string str = "unknown";

            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                AndroidJavaObject currentActivity = getApplicationContext();
                if (currentActivity != null)
                    str = GameTools.CallStatic<string>("getDeviceID", currentActivity);
            }
            return str;
#endif
            return "unknown";
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaObject getApplicationContext()
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    return jo.Call<AndroidJavaObject>("getApplicationContext");
                }
            }
        }
#endif

        /// <summary>
        /// 获取运营商
        /// </summary>
        /// <returns></returns>
        public static string GetOperatorName()
        {
#if UNITY_IOS
		return getOperatorName();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                AndroidJavaObject currentActivity = getApplicationContext();
                if (currentActivity != null)
                    return GameTools.CallStatic<string>("getOperatorName", currentActivity);
            }
#endif
            return "unknown";
        }

        /// <summary>
        /// 获取cpu类型|频率|核数
        /// </summary>
        /// <returns></returns>
        public static string GetCpu()
        {
#if UNITY_IOS
		return getCpu();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                return GameTools.CallStatic<string>("getCpu");
            }
#endif
            return "unknown";
        }

        /// <summary>
        /// IOS时，传Vindor标示符,Android时，传androidid
        /// </summary>
        /// <returns></returns>
        public static string GetIdfv()
        {
#if UNITY_IOS
		return getIdfv();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                AndroidJavaObject currentActivity = getApplicationContext();
                if (currentActivity != null)
                    return GameTools.CallStatic<string>("getAndroidId", currentActivity);
            }
#endif
            return "unknown";
        }

        /// <summary>
        /// 标识符（IOS时，传广告标示符，android时，传手机唯一识别码）
        /// </summary>
        /// <returns></returns>
        public static string GetIdfa()
        {
#if UNITY_IOS
		return getIdfa();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                return GetDeviceID();
            }
#endif
            return "unknown";
        }

        /// <summary>
        /// 获取App cup使用率
        /// </summary>
        /// <returns></returns>
        public static float GetCpuUsage()
        {
#if UNITY_IOS
		return getCpuUsage();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                return GameTools.CallStatic<float>("getCpuUsage");
            }
#endif
            return 0;
        }

        /// <summary>
        /// 获取当前内存占用
        /// </summary>
        /// <returns></returns>
        public static float GetMemoryUsage()
        {
#if UNITY_IOS
		return (float)getMemoryUsage();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                AndroidJavaObject currentActivity = getApplicationContext();
                if (currentActivity != null)
                    return GameTools.CallStatic<float>("getMemoryUsage", currentActivity);
            }
#endif
            return 0;
        }

        /// <summary>
        /// 获取设备是否越狱/Root
        /// </summary>
        /// <returns></returns>
        public static bool GetIsjailbreak()
        {
#if UNITY_IOS
		return getIsjailbreak();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaClass GameTools = new AndroidJavaClass("com.wmhd.tools.GameTools"))
            {
                return GameTools.CallStatic<bool>("getIsjailbreak");
            }
#endif
            return false;
        }

        /// <summary>
        /// 获取网络制式
        /// </summary>
        /// <returns></returns>
        public static string GetNetworkType()
        {
            string netType = "";
            NetworkReachability net = Application.internetReachability;
            switch (net)
            {
                case NetworkReachability.NotReachable:
                    netType = "无网络";
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    netType = "4G";
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    netType = "WIFI";
                    break;
                default:
                    break;
            }
            return netType;
        }

        /// <summary>
        /// 获取设备分辨率 w*h
        /// </summary>
        /// <returns></returns>
        public static string GetResolution()
        {
            return Screen.currentResolution.width + "*" + Screen.currentResolution.height;
        }
    }
}
