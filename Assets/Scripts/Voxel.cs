using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    public float speed = 5;
    public float destroyTime = 5.0f; //스스로 파괴할 시간
    float currentTime = 0; //현재 시간

    // Start is called before the first frame update


    void OnEnable() //start 대신에 활성화 되면 실행되도록
    {
        currentTime = 0; //시작 후 시간이 아닌 실행 후 시간
        Vector3 direction = Random.insideUnitSphere;  // 반지름이 1인 가상의 구 생성, 그 내부의 임의의 점을 반환. 즉 0과 1 사이 랜덤한 방향과 크기를 가짐
        //Vector3이기 때문에 3차원 좌표계에서의 방향을 나타냄
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = direction * speed;                           //velocity 는 AddForce와는 다름. AddForce는 밀어주는 것. velocity는 발사 !
    }

    // Update is called once per frame
    void Update()
    {
        
        currentTime += Time.deltaTime; 
        if (currentTime > destroyTime) 
        {
            gameObject.SetActive(false); //자기 자신을 gameobject로 지칭
            VoxelMaker.voxelPool.Add(gameObject); //VoxelMaker 스크립트에서 만든 voxelPool이라는 리스트에 자기자신(voxel)을 추가함
            //Destroy(gameObject); 비활성화 했으므로 파괴는 쓰지 않음.
        }
        
    }
}