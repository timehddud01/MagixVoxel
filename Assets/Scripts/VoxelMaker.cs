using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VoxelMaker : MonoBehaviour
{
    //생성할 대상 등록

    public GameObject voxelFactory;


    //메모리 누수방지를 위해 pool 생성



    //오브젝트 풀의 크기
    public int voxelPoolsize = 20;

    //오브젝트 풀
    //static : 정적변수  = 전체 통틀어서 하나만 생성, 즉 코드 실행 시 생성되는 값이 아닌, 전체에서 유일하게 존재하게 됨 
    public static List<GameObject> voxelPool = new List<GameObject>(); //General의 개념 확실히 공부하기
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < voxelPoolsize; i++) //풀 크기만큼 반복
        {
            //복셀 생성
            GameObject voxel = Instantiate(voxelFactory);


            //색상 램덤으로 생성하여 넣기
            MeshRenderer Render = voxel.GetComponent<MeshRenderer>();
            Render.material.color = UnityEngine.Random.ColorHSV();
            
            //복셀 비활성화
            voxel.SetActive(false);
            //복셀을 오브젝트 풀에 추가
            voxelPool.Add(voxel);
        }
        //풀에 미리 만들어놓고 비활성화 한 후, 클릭하면 주머니에서 꺼내는 것

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();

            //마우스의 위치가 바닥 위에 위치해 있다면
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (voxelPool.Count > 0)//오브젝트 풀 안에 복셀이 있는지 확인하고!
                {
                    GameObject voxel = voxelPool[0];                    //복셀 풀 최상단의 값을 가져오고
                    voxel.SetActive(true);                              //복셀을 활성화함
                    voxel.transform.position = hitInfo.point;           //Raycast를 통해 얻은 충돌지점의 위치로 객체를 이동(instantiate이 아님! 풀을 사용하게 되면서 비활성화 한 것을 가져오기만 하면 되는 것임)
                    voxelPool.RemoveAt(0);                              //오브젝트 풀에서 맨 위에 있는 voxel 1개 제거

                }
            }
        }
    }
}
