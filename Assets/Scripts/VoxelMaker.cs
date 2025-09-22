//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VoxelMaker : MonoBehaviour
{
    //생성할 대상 등록

    public GameObject voxelFactory;
    float currentTime = 0; //자동 생성을 위한 시간 
    float createTime = 0.1f; //0.1초마다 자동 생성

    //메모리 누수방지를 위해 pool 생성

    //크로스헤어 변수
    public Transform crosshair;


    //오브젝트 풀의 크기
    public int voxelPoolsize = 20;

    //오브젝트 풀
    //static : 정적변수  = 전체 통틀어서 하나만 생성, 즉 코드 실행 시 생성되는 값이 아닌, 전체에서 유일하게 존재하게 됨 
    public static List<GameObject> voxelPool = new List<GameObject>(); //Generic 은 <>인데, 괄호 안에 속성을 명시해 줌으로써 리스트가 담을 데이터를 지정함
    //장점 : 
    // 1. 메모리 크기 결정: 리스트에 데이터를 저장하려면 얼마나 많은 메모리 공간이 필요한지 알아야 합니다. GameObject와 int(정수)는 차지하는 메모리 크기가 다릅니다. 타입을 지정해 줘야 컴파일러가 효율적으로 메모리를 할당할 수 있습니다.
    // 2.타입 안정성 보장: 제네릭의 핵심적인 장점입니다. List<GameObject>라고 지정하는 순간, 프로그래머는 "이 리스트에는 GameObject만 들어있다"고 보장받게 됩니다. 만약 실수로 다른 것을 넣으려고 하면 컴파일러가 즉시 에러를 알려줘서 버그를 막아줍니다.
    
    
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < voxelPoolsize; i++) //풀 크기만큼 반복
        {
            //복셀 생성
            GameObject voxel = Instantiate(voxelFactory);


            //색상 램덤으로 생성하여 넣기
            MeshRenderer Render = voxel.GetComponent<MeshRenderer>(); //GetComponent는 속성을 가져오고, 바꿀 수 있는 리모컨. Reference의 개념
            Render.material.color = Random.ColorHSV(); //왜 그냥 Random 을 쓰면 안될까?? - 찾아보기 using system을 지우면 그냥 Random이 가능해짐_ .net이랑 유니티에서 둘 다 제공하기 때문일 걸로 추측
            
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
        //크로스헤어 그리기
        ARAVRInput.DrawCrosshair(crosshair);
        
        //if (Input.GetButtonDown("Fire1")) //기본 설정 : 왼쪽 마우스, 왼쪽 Ctrl
        if (ARAVRInput.Get(ARAVRInput.Button.One)) //오큘러스 입력 가져오기
        {
            currentTime+=Time.deltaTime;
            if (currentTime>createTime)
            {
            
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //메인 카메라에서 마우스 커서 위치를 뚫고 나가는 가상의 광선(Ray)를 생성
                //RaycastHit hitInfo = new RaycastHit(); //충돌한 물체의 이름, 충동 지점의 좌표, 충돌 지점까지의 거리 등 저장! 새 보고서를 만들고 그걸 hitInfo에 담자. 

                Ray ray  =new Ray(ARAVRInput.RHandPosition,ARAVRInput.RHandDirection);
                RaycastHit hitInfo = new RaycastHit();

                //마우스의 위치가 바닥 위에 위치해 있다면
                if (Physics.Raycast(ray, out hitInfo)) //physics.raycast는 발사, ray는 true인지 false인지(부딪혔는지 아닌지), 부딪힌 기록(hitInfo)은 out으로 빼달라는 뜻
                {
                    if (voxelPool.Count > 0)//오브젝트 풀 안에 복셀이 있는지 확인하고!
                    {
                    

                        GameObject voxel = voxelPool[0];                    //복셀 풀 최상단의 값을 가져오고
                        voxel.SetActive(true);                              //복셀을 활성화함
                        voxel.transform.position = hitInfo.point;           //Raycast를 통해 얻은 충돌지점의 위치로 객체를 이동(instantiate이 아님! 풀을 사용하게 되면서 비활성화 한 것을 가져오기만 하면 되는 것임)
                        voxelPool.RemoveAt(0);       //오브젝트 풀에서 맨 위에 있는 voxel 1개 제거
                        currentTime =0;                       

                    }
                }
            }
            
        }
    }
}
