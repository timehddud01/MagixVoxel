using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMaker : MonoBehaviour
{
    //생성할 대상 등록
      //메모리 누수방지를 위해 pool 생성
    public GameObject voxelFactory;
    // Start is called before the first frame update
    void Start()
    {
      
        
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
                GameObject voxel = Instantiate(voxelFactory); //생성
                voxel.transform.position = hitInfo.point; //그 위치로 옮김
            }
        }
    }
}
