using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class Script_DetectManger : MonoBehaviour
{
    /// <summary>
    /// 偵測地板點擊管理器
    /// 點擊地板後處理互動行為
    /// 生成物件與控制物件位置
    /// </summary>

    [Header("點擊後要生成的物件")]
    public GameObject spawnObj;

    [Header("AR射線管理器"), Tooltip("此管理器要放在攝影機Origin物件上")]
    public ARRaycastManager arRaycastManager;
    [Header("要面向的攝影機")]
    public Transform camTarget;
    [Header("面相速度")]
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
    /// 點擊與檢測地板
    /// 1. 偵測是否按指定按鈕
    /// 2.紀錄點擊座標
    /// 3.射線檢測
    /// 4.互動
    /// </summary>
    private void ClickAndDetechGround()
    {
        //如果按下指定按鍵
        if (inputMouseLeft)
        {
            //取得點擊座標
            Vector2 mousePos = Input.mousePosition;
            //將點擊座標轉為射線
            Ray ray = Camera.main.ScreenPointToRay(mousePos);


            //如果射線打到指定的地板物件
            if (arRaycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
            {
                //取得點到的座標
                Vector3 hitPos = hits[0].pose.position;
                //將物件生成在點到的座標上

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
