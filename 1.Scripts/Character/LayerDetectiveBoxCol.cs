using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ݸ����� ���� ���� ������ ĳ���� �ݸ����� ��ġ�鼭 Ƣ�� ������ ����� �����̴�
public class LayerDetectiveBoxCol : MonoBehaviour
{
    public Vector3 BoxSize = new Vector3(1,1,1);

    public Collider[] LayerDetective(LayerMask DectectiveLayer)
    {
        return Physics.OverlapBox(transform.position, BoxSize * 0.5f, transform.rotation, DectectiveLayer); // ������ �ٸ��� 0.5�� ���� ������ �������� ��Ÿ���� ����
    }

    private void OnDrawGizmos()
    {
        //���� ��ġ�� ��ȯ
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, BoxSize);
    }

}
