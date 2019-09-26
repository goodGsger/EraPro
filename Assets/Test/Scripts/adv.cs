using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adv : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("与相机深度旋转量：" + Camera.main.transform.rotation * Vector3.back);
        Vector3 target = transform.position + Camera.main.transform.rotation * Vector3.forward;

        Debug.Log("与相机垂直旋转量：" + Camera.main.transform.rotation * Vector3.up);
        Vector3 up = Camera.main.transform.rotation * Vector3.up;
        transform.LookAt(target, up);
    }
}
