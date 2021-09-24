using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    //移動用の変数を作成
    private float x, z;
    //移動スピード
    public float xspeed, zspeed = 0.0f;
    //変数の宣言
    public GameObject cam;
    Quaternion cameraRot, characterRot;
    public float Xsensityvity = 0.0f, Ysensityvity = 0.0f;
    //マウスカーソル非表示
    private bool cursorLock = true;
    private float minX = -90f, maxX = 90f;
    //地面判定
    private bool GroundEnter = false, GroundStay = false, GroundExit = false;
    private bool isground = false;

    //ジャンプキー
    bool SpaceKey = false;

    [SerializeField] private float jumpspeed;
    [SerializeField]private Rigidbody rb;//playerのrigitbody
    // Start is called before the first frame update
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);
        
        //視点を90度に制限
        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        LateUpdateCursorLock();
        SpaceKey = Input.GetButtonDown("Jump");
        bool isG = Isground();
        if (SpaceKey && isG)
        {
            rb.AddForce(transform.up * jumpspeed);
        }
    }//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private void FixedUpdate()//-----------------------------------------------------
    {
        x = 0;
        z = 0;
        x = Input.GetAxis("Horizontal") * xspeed * zspeed;
        z = Input.GetAxis("Vertical") * xspeed * zspeed;

        transform.position += cam.transform.forward * z + cam.transform.right * x;

    }//-----------------------------------------------------------------------------


    public void LateUpdateCursorLock()
    {
        //Escapeを押したらマウスカーソル表示
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        //左クリックでマウスカーソル非表示
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }
        //false時非表示
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        //true時表示
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;

        }
    }
    public Quaternion ClampRotation(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);
        return q;
    }
    private bool Isground()
    {
        if (GroundEnter || GroundStay)
        {
            isground = true;
        }
        else if (GroundExit)
        {
            isground = false;
        }
        GroundEnter = false;
        GroundStay = false;
        GroundExit = false;
        return isground;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            GroundEnter = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            GroundStay = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            GroundExit = true;
        }
    }

}
