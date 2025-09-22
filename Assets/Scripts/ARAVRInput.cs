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

    // ���� ��Ʈ�ѷ�
    static Transform lHand; //public 차이로 l과 L. 외부에서 접근, 바꿀 수 없게 하기 위해서.
    // ���� ��ϵ� ���� ��Ʈ�ѷ��� ã�� ��ȯ
    public static Transform LHand // 이 녀석은 외부에서 바꿀 수 있음. lhand와 비교. 이 두 줄이 중요함.
    {//외부에서 받아온 것을 lhand와 비교
        get 
        {
            if (lHand == null)
            {
#if PC
                // LHand��� �̸����� ���� ������Ʈ�� �����.
                GameObject handObj = new GameObject("LHand");
                // ������� ��ü�� Ʈ�������� lHand�� �Ҵ�
                lHand = handObj.transform;
                // ��Ʈ�ѷ��� ī�޶��� �ڽ� ��ü�� ���
                lHand.parent = Camera.main.transform;
#elif Oculus
                lHand = GameObject.Find("LeftControllerAnchor").transform;
#endif
            }
            return lHand;
        }
    }

    // ������ ��Ʈ�ѷ�
    static Transform rHand;
    // ���� ��ϵ� ������ ��Ʈ�ѷ� ã�� ��ȯ
    public static Transform RHand
    {
        get
        {
            // ���� rHand�� ���� �������
            if (rHand == null)
            {
#if PC
                // RHand �̸����� ���� ������Ʈ�� �����.
                GameObject handObj = new GameObject("RHand");
                // ������� ��ü�� Ʈ�������� rHand�� �Ҵ�
                rHand = handObj.transform;
                // ��Ʈ�ѷ��� ī�޶��� �ڽ� ��ü�� ���
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
            // ���콺�� ��ũ�� ��ǥ ������
            Vector3 pos = Input.mousePosition;
            // z ���� 0.7m�� ����
            pos.z = 0.7f; //x,y로만 움직임
            // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
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
            // ���콺�� ��ũ�� ��ǥ ������
            Vector3 pos = Input.mousePosition;
            // z ���� 0.7m�� ����
            pos.z = 0.7f;
            // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
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

    // ��Ʈ�ѷ��� Ư�� ��ư�� ������ �ִ� ���� true�� ��ȯ
    public static bool Get(Button virtualMask, Controller hand = Controller.RTouch) //Bool
    {
#if PC
        // virtualMask�� ���� ���� ButtonTarget Ÿ������ ��ȯ�� �����Ѵ�.
        return Input.GetButton(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.Get((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif
    }

    // ��Ʈ�ѷ��� Ư�� ��ư�� ������ �� true�� ��ȯ
    public static bool GetDown(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonDown(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetDown((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif
    }

    // ��Ʈ�ѷ��� Ư�� ��ư�� ������ ������ �� true�� ��ȯ
    public static bool GetUp(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonUp(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetUp((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#endif
    }

    // ��Ʈ�ѷ��� Axis �Է��� ��ȯ
    // axis: Horizontal, Vertical ���� ���´�.
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

    // ��Ʈ�ѷ��� ���� ȣ���ϱ�
    public static void PlayVibration(Controller hand)
    {
#if Oculus
        PlayVibration(0.06f, 1, 1, hand);
#endif
    }

    // ��Ʈ�ѷ��� ���� ȣ���ϱ�
    // duration: ���� �ð�, frequency: ��,
    // amplify: ����, hand: ���� Ȥ�� ������ ��Ʈ�ѷ�
    public static void PlayVibration(float duration, float frequency, float amplitude,
    Controller hand)
    {
#if Oculus
        if (CoroutineInstance.coroutineInstance == null)
        {
            GameObject coroutineObj = new GameObject("CoroutineInstance");
            coroutineObj.AddComponent<CoroutineInstance>();
        }
        // �̹� �÷������� ���� �ڷ�ƾ�� ����
        CoroutineInstance.coroutineInstance.StopAllCoroutines();
        CoroutineInstance.coroutineInstance.StartCoroutine(VibrationCoroutine(duration, frequency, amplitude, hand));
#endif
    }

    // ī�޶� �ٶ󺸴� ������ �������� ���͸� ��´�.
    public static void Recenter()
    {
#if Oculus
        OVRManager.display.RecenterPose();
#endif
    }

    // ���ϴ� �������� Ÿ���� ���͸� ����
    public static void Recenter(Transform target, Vector3 direction)
    {
        target.forward = target.rotation * direction;
    }

#if PC
    static Vector3 originScale = Vector3.one * 0.02f;
#else
    static Vector3 originScale = Vector3.one * 0.005f;
#endif
    // ���� ���̰� ��� ���� ũ�ν��� ��ġ��Ű�� �ʹ�.
    public static void DrawCrosshair(Transform crosshair, bool isHand = true, Controller
    hand = Controller.RTouch)
    {
        Ray ray;
        // ��Ʈ�ѷ��� ��ġ�� ������ �̿��� ���� ����
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
            // ī�޶� �������� ȭ���� ���߾����� ���̸� ����
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        // ���� �� ���̴� Plane�� �����.
        Plane plane = new Plane(Vector3.up, 0);
        float distance = 0;
        // plane�� �̿��� ray�� ���.
        if (plane.Raycast(ray, out distance))
        {
            // ������ GetPoint �Լ��� �̿��� �浹 ������ ��ġ�� �����´�.
            crosshair.position = ray.GetPoint(distance);
            crosshair.forward = -Camera.main.transform.forward;
            // ũ�ν������ ũ�⸦ �ּ� �⺻ ũ�⿡�� �Ÿ��� ���� �� Ŀ������ �Ѵ�
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


// ARAVRInput Ŭ�������� ����� �ڷ�ƾ ��ü
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