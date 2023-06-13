using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라 
/// follow player, 마우스와 움직임을 같이함, 기본 3인칭모드 , 줌 가능,회전
/// </summary>
public class Cam : MonoBehaviour
{
    public Transform target;
    public Vector3 pivotOffset = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 camOffset = new Vector3(0.4f, 0.5f, -2.0f);

    public float smooth = 10f;
    public float HorizontalAimSpeed = 6f;
    public float VerticalAimSpeed = 6f;
    public float VerticalMaxAngle = 30f;
    public float VerticalMinAngle = -60f;
    public float angleH = 0;
    public float angleV = 0;
    public float ExangleH;

    private Transform myTransform;
    private Camera myCam;

    private Vector3 FromTargetToCameraPos; //카메라와 플레이어의 벡터
    private float FromTargetToCameraMag; //카메라와 플레이어의 거리
    private Vector3 smoothPivotOffset;
    private Vector3 smoothCamOffset;
    private Vector3 targetPivotOffset;
    private Vector3 targetCamOffset;
    private float defaultFOV;
    private float targetFOV;

    Quaternion ex1;
    Quaternion ex2;

    private void Awake()
    {
        myTransform = transform;
        myCam = GetComponent<Camera>();

        myTransform.position = target.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        myTransform.rotation = Quaternion.identity;

        FromTargetToCameraPos = myTransform.position - target.position;
        FromTargetToCameraMag = FromTargetToCameraPos.magnitude - 0.5f;

        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;
        defaultFOV = myCam.fieldOfView;
        angleH = target.eulerAngles.y;

        ResetTargetOffset();
        ResetFOV();
    }

    public void ResetTargetOffset()
    {
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
    }

    public void ResetFOV()
    {
        targetFOV = defaultFOV;
    }

    public void SetFOV(float changeFOV)
    {
        targetFOV = changeFOV;
    }
    bool ViewingPosChekc(Vector3 checkPos, float deltaPlayerHeight)
    {
        Vector3 target = this.target.position + (Vector3.up * deltaPlayerHeight);
        if (Physics.SphereCast(checkPos, 0.2f, target - checkPos, out RaycastHit hit, FromTargetToCameraMag))
        {
            if (hit.transform != this.target && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    bool ReverserViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistance)
    {
        Vector3 origin = target.position + (Vector3.up * deltaPlayerHeight);
        if (Physics.SphereCast(origin, 0.2f, checkPos - origin, out RaycastHit hit, maxDistance))
        {
            if (hit.transform != target && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    bool DoubleViewingPosChekc(Vector3 checkPos, float offset)
    {
        float playerFocusHeight = target.GetComponent<CapsuleCollider>().height * 0.75f;

        return ViewingPosChekc(checkPos, playerFocusHeight) && ReverserViewingPosCheck(checkPos, playerFocusHeight, offset);
    }
    //휠버튼 누르면 자유카메라
    //휠버튼 누르지 않으면 플레이어를 따라간다
    public void MovingCam(bool ben = false)
    {
        if (!ben)
        {
            if (Input.GetMouseButtonUp(2)) angleH = ExangleH;
            else if (Input.GetMouseButtonDown(2)) ExangleH = angleH;

            angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f) * HorizontalAimSpeed; // 왼오
            angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1f, 1f) * VerticalAimSpeed; //위아래 -60 ~ 30

            angleV = Mathf.Clamp(angleV, VerticalMinAngle, VerticalMaxAngle); //위아래 제한
        }

        Quaternion camYRotation = Quaternion.Euler(0.0f, angleH, 0.0f); 
        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0.0f);

        if (!Input.GetMouseButton(2)) target.rotation = camYRotation;

        myTransform.rotation = aimRotation;

        myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, targetFOV, Time.deltaTime);

        Vector3 baseTempPosition = target.position + camYRotation * targetPivotOffset;
        Vector3 noCollisionOffset = targetCamOffset;

        for (float zOffset = targetCamOffset.z; zOffset <= 0f; zOffset += 0.5f)
        {
            noCollisionOffset.z = zOffset;
            if (DoubleViewingPosChekc(baseTempPosition + aimRotation * noCollisionOffset, Mathf.Abs(zOffset)) || zOffset == 0f)
            {
                break;
            }
        }

        smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
        smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);

        myTransform.position = target.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

    }


    public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
    {
        return Mathf.Abs((finalPivotOffset - smoothPivotOffset).magnitude);
    }
}
