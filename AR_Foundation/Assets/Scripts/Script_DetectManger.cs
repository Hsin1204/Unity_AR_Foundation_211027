using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class Script_DetectManger : MonoBehaviour
{
    /// <summary>
    /// �����a�O�I���޲z��
    /// �I���a�O��B�z���ʦ欰
    /// �ͦ�����P������m
    /// </summary>

    [Header("�I����n�ͦ�������")]
    public GameObject spawnObj;

    [Header("AR�g�u�޲z��"), Tooltip("���޲z���n��b��v��Origin����W")]
    public ARRaycastManager arRaycastManager;
    [Header("�n���V����v��")]
    public Transform camTarget;
    [Header("���۳t��")]
    public float turnSpeed = 3.5f;

    private Transform spawnTar;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool inputMouseLeft { get => Input.GetKeyDown(KeyCode.Mouse0); }

    private void Update()
    {
        ClickAndDetechGround();
        SpawnLookAtCam();
    }
    /// <summary>
    /// �I���P�˴��a�O
    /// 1. �����O�_�����w���s
    /// 2.�����I���y��
    /// 3.�g�u�˴�
    /// 4.����
    /// </summary>
    private void ClickAndDetechGround()
    {
        //�p�G���U���w����
        if (inputMouseLeft)
        {
            //���o�I���y��
            Vector2 mousePos = Input.mousePosition;
            //�N�I���y���ର�g�u
            Ray ray = Camera.main.ScreenPointToRay(mousePos);


            //�p�G�g�u������w���a�O����
            if (arRaycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
            {
                //���o�I�쪺�y��
                Vector3 hitPos = hits[0].pose.position;
                //�N����ͦ��b�I�쪺�y�ФW

                if (spawnTar == null)
                {
                    spawnTar = Instantiate(spawnObj, hitPos, Quaternion.identity).transform;
                    spawnTar.localScale = Vector3.one * 0.5f;
                }
                else
                {
                    spawnTar.position = hitPos;
                }

            }


        }
    }
    private void SpawnLookAtCam()
    {
        Quaternion angle = Quaternion.LookRotation(camTarget.position - spawnTar.position);
        spawnTar.rotation = Quaternion.Lerp(spawnTar.rotation, angle, Time.deltaTime);
        Vector3 ori_Angle = spawnTar.localEulerAngles;
        ori_Angle.x = 0;
        ori_Angle.z = 0;
        spawnTar.localEulerAngles = ori_Angle;
    }
}
