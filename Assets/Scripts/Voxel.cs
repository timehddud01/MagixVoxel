using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    public float speed = 5;
    public float destroyTime = 5.0f; // �ı��ҽð�
    float currentTime = 0; //���� �ð�

    // Start is called before the first frame update


    void OnEnable() //start 대신에 활성화 되면 실행되도록
    {
        currentTime = 0; //시작 후 시간이 아닌 실행 후 시간
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
            gameObject.SetActive(false); //자기 자신을 gameobject로 지칭
            VoxelMaker.voxelPool.Add(gameObject); //VoxelMaker 스크립트에서 만든 voxelPool을 가져오고 자기자신(voxel)을 추가함
            //Destroy(gameObject); //�ڽ��� �ı��Ѵ�.
        }
        
    }
}