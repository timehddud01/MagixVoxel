using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    Vector3 angle;
    public float sensitivity = 200;

//������ ��ŭ�� ȸ���ϵ���


    // Start is called before the first frame update
    void Start()
    {
        angle = Camera.main.transform.eulerAngles;
        angle.x *= -1; //���� : ī�޶� �ٶ󺸴� ����(ī�޶� x���� �ݴ���)
    }

    // Update is called once per frame
    void Update()
    {
    //���콺 ���� �Է�
    float x = Input.GetAxis("Mouse Y");
    float y = Input.GetAxis("Mouse X");
    //����Ȯ��
    angle.x += x * sensitivity * Time.deltaTime; //���ݸ� �������� �� ȸ���ϵ��� �ΰ����� �߰�
    angle.y += y * sensitivity * Time.deltaTime;
    angle.z = transform.eulerAngles.z;

    //���� ���� ����
    angle.x = Mathf.Clamp(angle.x, -90,90); //clamp�� ���� ����
    //ȸ��(ī�޶� ����)

    //�������� ���� �� ���� ��û �����ϰ� �Ǿ��ִ� ����(�̷��� ���� ������ �� �޸� ��������, �� �� ���� ����ϴ���)
    transform.eulerAngles = new Vector3(-angle.x,angle.y,transform.eulerAngles.z);
    
    
    }
}