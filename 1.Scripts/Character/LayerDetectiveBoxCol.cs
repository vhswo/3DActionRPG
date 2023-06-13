using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 콜리더를 많이 쓰는 이유는 캐릭터 콜리더와 겹치면서 튀는 현상이 생기기 때문이다
public class LayerDetectiveBoxCol : MonoBehaviour
{
    public Vector3 BoxSize = new Vector3(1,1,1);

    public Collider[] LayerDetective(LayerMask DectectiveLayer)
    {
        return Physics.OverlapBox(transform.position, BoxSize * 0.5f, transform.rotation, DectectiveLayer); // 기즈모와 다르게 0.5를 곱한 이유는 반지름을 나타내기 때문
    }

    private void OnDrawGizmos()
    {
        //월드 위치로 변환
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, BoxSize);
    }

}
