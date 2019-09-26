using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFree : MonoBehaviour
{
    [Header("上下移动速度")]
    public float speedUD = 80.0f;
    [Header("左右移动速度")]
    public float speedLR = 2.0f;
    private GameObject gameObject;

    void Start()
    {
        gameObject = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        //空格键抬升高度
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        }
        if (Input.GetKey(KeyCode.Return))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        }

        //w键前进
        if (Input.GetKey(KeyCode.W))
        {
            this.gameObject.transform.Translate(new Vector3(0, speedUD * Time.deltaTime, 0));
        }
        //s键后退
        if (Input.GetKey(KeyCode.S))
        {
            this.gameObject.transform.Translate(new Vector3(0, -speedUD * Time.deltaTime, 0));
        }
        //a键上移
        if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Translate(new Vector3(-speedLR, 0, 0 * Time.deltaTime));
        }
        //d键下移
        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Translate(new Vector3(speedLR, 0, 0 * Time.deltaTime));
        }
    }
}
