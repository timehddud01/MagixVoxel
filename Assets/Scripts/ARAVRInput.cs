#define PC //pc버전으로 할지 , 오큘러스로 할지 정함. 둘중 안쓰는 것 주석처리
//#define Oculus

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public static class ARAVRInput //받은 입력을 다른 곳에서 받아갈 수 있도록
{
#if PC
    public enum ButtonTarget //숫자의 집합, 묶음을 의미
    {
        Fire1, //왼쪽 마우스
        Fire2, //오른쪽
        Fire3, //중간
        Jump,  
    }
#endif
    public enum Button
    {
#if PC
        One = ButtonTarget.Fire1,
        Two = ButtonTarget.Jump,
        Thumbstick = ButtonTarget.Fire1,
        IndexTrigger = ButtonTarget.Fire3,
        HandTrigger = ButtonTarget.Fire2
#elif Oculus
        One = OVRInput.Button.One, //OVRInput은 오큘러스 입력
        Two = OVRInput.Button.Two,
        Thumbstick = OVRInput.Button.PrimaryThumbstick,
        IndexTrigger = OVRInput.Button.PrimaryIndexTrigger,
        HandTrigger = OVRInput.Button.PrimaryHandTrigger
#endif
    }

    public enum Controller
    {
#if PC
        LTouch,
        RTouch
#elif Oculus
        LTouch = OVRInput.Controller.LTouch,
        RTouch = OVRInput.Controller.RTouch
#endif
    }

#if Oculus
    static Transform rootTransform;
#endif
#if Oculus
    static Transform GetTransform() //Get-> 값을 읽어온다는 뜻(참조). Set은 값을 설정할 때.
    {
        if (rootTransform == null)
        {
            rootTransform = GameObject.Find("TrackingSpace").transform;
        }
        return rootTransform;
    }
# endif

    // 왼쪽 컨트롤러
    static Transform lHand; //public 차이로 l과 L. 외부에서 접근, 바꿀 수 없게 하기 위해서.
    // 씬에 등록된 왼쪽 컨트롤러를 찾아 변환
    public static Transform LHand // 이 녀석은 외부에서 바꿀 수 있음. lhand와 비교. 이 두 줄이 중요함.
    {//외부에서 받아온 것을 lhand와 비교
        get 
        {
            if (lHand == null)
            {
#if PC
                // LHand라는 이름으로 게임 오브젝트를 만든다.
                GameObject handObj = new GameObject("LHand");
                // 만들어진 객체의 트랜스폰을 Lhand에 할당
                lHand = handObj.transform;
                // 컨트롤러를 카메라의 자식 객체로 등록
                lHand.parent = Camera.main.transform;
#elif Oculus
                lHand = GameObject.Find("LeftControllerAnchor").transform;
#endif
            }
            return lHand;
        }
    }

    // 오른쪽 컨트롤러
    static Transform rHand;
    // 씬에 등록된 오른쪽 컨트롤러를 찾아 변환
    public static Transform RHand
    {
        get
        {
            // 만약 rHand가 null이라면
            if (rHand == null)
            {
#if PC
                // RHand 라는 이름으로 게임 오브젝트를 만든다.
                GameObject handObj = new GameObject("RHand");
                // 만들어진 객체의 트랜스폼을 Rhand에 할당
                rHand = handObj.transform;
                // 컨트롤러를 카메라의 자식 객체로 등록
                rHand.parent = Camera.main.transform;
#elif Oculus
                rHand = GameObject.Find("RightControllerAnchor").transform;
#endif
            }
            return rHand;
        }
    }

    public static Vector3 RHandPosition
    {
        get
        {
#if PC
            // 마우스의 스크린 위치 좌표 얻어오기
            Vector3 pos = Input.mousePosition;
            // z 값은 0.7m로 설정
            pos.z = 0.7f; //x,y로만 움직임
            // 스크린 좌표를 월드 좌표로 변환
            pos = Camera.main.ScreenToWorldPoint(pos);
            RHand.position = pos;
            return pos;
#elif Oculus
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            pos = GetTransform().TransformPoint(pos); //평변화
            return pos;
#endif
        }
    }

    public static Vector3 RHandDirection
    {
        get
        {
#if PC
            Vector3 direction = RHandPosition - Camera.main.transform.position; //출발지 =  카메라, 도착지 = RhandPosition. 도착지에서 카메라를 바라보는 방향을 구할 수 있음.
            RHand.forward = direction;
            return direction;
#elif Oculus
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.
            RTouch) * Vector3.forward;
            direction = GetTransform().TransformDirection(direction);
            return direction;
#endif
        }
    }

    public static Vector3 LHandPosition
    {
        get
        {
#if PC
            // 마우스의 스크린 위치 좌표 얻어오기
            Vector3 pos = Input.mousePosition;
            // z 값은 0.7m로 설정
            pos.z = 0.7f;
            // 스크린 좌표를 월드 좌표로 변환
            pos = Camera.main.ScreenToWorldPoint(pos);
            LHand.position = pos;
            return pos;
#elif Oculus
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            pos = GetTransform().TransformPoint(pos);
            return pos;
#endif
        }
    }

    public static Vector3 LHandDirection
    {
        get
        {
#if PC
            Vector3 direction = LHandPosition - Camera.main.transform.position;
            LHand.forward = direction;
            return direction;
#elif Oculus
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.
            LTouch) * Vector3.forward;
            direction = GetTransform().TransformDirection(direction);
            return direction;
#endif
        }
    }

    // 컨트롤러의 특정 버튼을 누르고 있는 동안 true를 반환
    public static bool Get(Button virtualMask, Controller hand = Controller.RTouch) //Bool
    {
#if PC
        // virtualMask에 들어온 값을 ButtonTarget 타입으로 변환해 전달한다.
        return Input.GetButton(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.Get((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif
    }

    // 컨트롤러의 특정 버튼을 처음 누를 때 true를 반환
    public static bool GetDown(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonDown(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetDown((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif
    }

    // 컨트롤러의 특정 버튼을 처음 뗄 때 true를 반환
    public static bool GetUp(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonUp(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetUp((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif
    }

    // 컨트롤러의 Axis 입력을 반환
    // axis: Horizontal, Vertical 값을 갖는다.
    public static float GetAxis(string axis, Controller hand = Controller.LTouch)
    {
#if PC
        return Input.GetAxis(axis);
#elif Oculus
        if (axis == "Horizontal" || axis == "Mouse X")
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)
            hand).x;
        }
        else
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)
            hand).y;
        }
#endif
    }

    // 컨트롤러에 진동 호출하기
    public static void PlayVibration(Controller hand)
    {
#if Oculus
        PlayVibration(0.06f, 1, 1, hand);
#endif
    }

    // 컨트롤러에 진동 호출하기
    // duration: 지속시간 , frequency: 빈도
    // amplify: 진폭, hand: 컨트롤러(오른쪽 혹은 왼쪽)
    public static void PlayVibration(float duration, float frequency, float amplitude,
    Controller hand)
    {
#if Oculus
        if (CoroutineInstance.coroutineInstance == null)
        {
            GameObject coroutineObj = new GameObject("CoroutineInstance");
            coroutineObj.AddComponent<CoroutineInstance>();
        }
        //이미 플레이중인 진동 코루틴은 정지시킴
        CoroutineInstance.coroutineInstance.StopAllCoroutines();
        CoroutineInstance.coroutineInstance.StartCoroutine(VibrationCoroutine(duration, frequency, amplitude, hand));
#endif
    }

    // 카메라가 바라보는 방향을 기준으로 센터를 잡는다.
    public static void Recenter()
    {
#if Oculus
        OVRManager.display.RecenterPose();
#endif
    }

    // 원하는 방향으로 타깃의 센터를 설정
    public static void Recenter(Transform target, Vector3 direction)
    {
        target.forward = target.rotation * direction;
    }

#if PC
    static Vector3 originScale = Vector3.one * 0.02f;
#else
    static Vector3 originScale = Vector3.one * 0.005f;
#endif
    // 광선 레이가 닿는 곳에 크로스헤어를 위치시키고 싶다.
    public static void DrawCrosshair(Transform crosshair, bool isHand = true, Controller
    hand = Controller.RTouch)
    {
        Ray ray;
        // 컨트롤러의 위치와 방향을 이용해 레이 제작
        if (isHand)
        {
#if PC
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
            if (hand == Controller.RTouch)
                ray = new Ray(RHandPosition, RHandDirection);
            else
                ray = new Ray(LHandPosition, LHandDirection);
#endif
        }
        else
        {
            // 카메라를 기준으로 화면의 정중앙으로 레이 제작
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        // 눈에 안보이는 Plane을 하나 만든다. y=0 평면
        Plane plane = new Plane(Vector3.up, 0);
        float distance = 0;
        // plane을 이용해 ray를 쏜다
        if (plane.Raycast(ray, out distance))
        {
            // 레이의 GetPoint 함수를 이용해 충돌 지점 좌표 계산
            crosshair.position = ray.GetPoint(distance);
            crosshair.forward = -Camera.main.transform.forward;
            // 크로스헤어의 크기를 최소 기본 크기에서 거리에 따라 더 커지도록 한다.
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
        else
        {
            crosshair.position = ray.origin + ray.direction * 100;
            crosshair.forward = -Camera.main.transform.forward;
            distance = (crosshair.position - ray.origin).magnitude;
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
    }

#if Oculus
    static IEnumerator VibrationCoroutine(float duration, float frequency, float amplitude,
    Controller hand)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            OVRInput.SetControllerVibration(frequency, amplitude, (OVRInput.Controller)
            hand);
            yield return null;
        }
        OVRInput.SetControllerVibration(0, 0, (OVRInput.Controller)hand);
    }
#endif
}


// ARAVRInput 클래스에서 사용할 코루틴 객체
class CoroutineInstance : MonoBehaviour
{
    public static CoroutineInstance coroutineInstance = null;
    private void Awake()
    {
        if (coroutineInstance == null)
        {
            coroutineInstance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}