using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target; //타겟

    public float shakeAmount; //흔들림 강도
    public Transform originPos; //카메라 기존 위치

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position; //카메라 위치를 타겟의 위치로 이동
    }

    public void CameraShakeOn()
    {
        Vector3 vibration = Random.insideUnitSphere * shakeAmount + originPos.position;
        //진동 강도 만큼의 구체 범위와 카메라의 기존 위치를 합한 구역애서 무작위로 Vector3값 추출

        Camera.main.transform.position = vibration;
        //카메라를 흔듦
    }

    public void CameraShakeOff()
    {
        Camera.main.transform.position = originPos.position;
        //카메라의 위치를 기존 위치로 초기화
    }
}
