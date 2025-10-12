using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    Vector3 angle;
    public float sensitivity = 200;

//움직인 만큼만 회전하도록


    // Start is called before the first frame update
    void Start()
    {
        angle = Camera.main.transform.eulerAngles;
        angle.x *= -1; //반전 : 카메라가 바라보는 방향(카메라 x축의 반대임)
    }

    // Update is called once per frame
    void Update()
    {
    //마우스 정보 입력
    float x = Input.GetAxis("Mouse Y");
    float y = Input.GetAxis("Mouse X");
    //방향확인
    angle.x += x * sensitivity * Time.deltaTime; //조금만 움직여도 잘 회전하도록 민감도를 추가
    angle.y += y * sensitivity * Time.deltaTime;
    angle.z = transform.eulerAngles.z;

    //상하 각도 제한
    angle.x = Mathf.Clamp(angle.x, -90,90); //clamp는 범위 조절
    //회전(카메라에 적용)

    //생성자의 개념 이 줄은 엄청 간소하게 되어있는 것임(이렇게 하지 않으면 왜 메모리 누수인지, 왜 이 줄을 사용하는지)
    transform.eulerAngles = new Vector3(-angle.x,angle.y,transform.eulerAngles.z);
    
    
    }
}