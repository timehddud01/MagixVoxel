using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    public float speed = 5;
    public float destroyTime = 5.0f; // �ı��ҽð�
    float currentTime = 0; //���� �ð�

    // Start is called before the first frame update


    void Start()
    {
        Vector3 direction = Random.insideUnitSphere; //ũ�Ⱑ 1�̰� ���⸸ ������
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = direction * speed; 
    }

    // Update is called once per frame
    void Update()
    {
        //���� �ð��� ������ �����ϱ�
        currentTime += Time.deltaTime; //���� �ð� ����(������ ���� ����ð�)
        if (currentTime > destroyTime) //�ð� �ʰ� ���� ��
        {
            Destroy(gameObject); //�ڽ��� �ı��Ѵ�.
        }
        
    }
}