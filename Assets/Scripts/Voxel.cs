using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    public float speed = 5;
    public float destroyTime = 5.0f; // 파괴할시간
    float currentTime = 0; //현재 시간

    // Start is called before the first frame update


    void Start()
    {
        Vector3 direction = Random.insideUnitSphere; //크기가 1이고 방향만 존재함
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = direction * speed; 
    }

    // Update is called once per frame
    void Update()
    {
        //일정 시간이 지나면 삭제하기
        currentTime += Time.deltaTime; //현재 시간 누적(프레임 기준 경과시간)
        if (currentTime > destroyTime) //시간 초과 했을 때
        {
            Destroy(gameObject); //자신을 파괴한다.
        }
        
    }
}