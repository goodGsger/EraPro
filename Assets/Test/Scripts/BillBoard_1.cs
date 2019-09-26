using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard_1 : MonoBehaviour
{
    private Vector3 initCameraEuler = Vector3.zero;  //相机全局欧拉
    private Vector3 target = Vector3.zero;           //目标位置
    private Vector3 up = Vector3.zero;               //变换垂直向
    private Quaternion initCameraQuaternion;         //相机全局四元数
    private Quaternion newRotation;                  //z更新相机四元数
    void Start()
    {
        initCameraEuler = Camera.main.transform.rotation.eulerAngles;
    }

    void Update()
    {
        RotateListener();
    }

    /// <summary>
    /// 公告板呈像
    /// </summary>
    private void RotateListener()
    {
        initCameraQuaternion = Camera.main.transform.rotation;

        //四元数转成欧拉角
        Vector3 cameraEuler = initCameraQuaternion.eulerAngles;
        Debug.Log("相机欧拉角：" + cameraEuler);

        //欧拉角转四元数
        //Quaternion rotation = Quaternion.Euler(cameraEuler);

        Debug.Log("与相机深度旋转量：" + initCameraQuaternion * Vector3.back);
        target = transform.position + initCameraQuaternion * Vector3.back;


        Debug.Log("与相机垂直旋转量：" + initCameraQuaternion * Vector3.up);
        //相机垂直方向不变
        if (cameraEuler.z != initCameraEuler.z)
        {
            cameraEuler.z = initCameraEuler.z;
            newRotation = Quaternion.Euler(cameraEuler);
            up = newRotation * Vector3.up;
        }
        else
        {
            up = initCameraQuaternion * Vector3.up;
        }
        transform.LookAt(target, up);
    }
}
