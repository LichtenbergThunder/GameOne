using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    //�ړ��p�̕ϐ����쐬
    private float x, z;
    //�ړ��X�s�[�h
    public float xspeed, zspeed = 0.0f;
    //�ϐ��̐錾
    public GameObject cam;
    Quaternion cameraRot, characterRot;
    public float Xsensityvity = 0.0f, Ysensityvity = 0.0f;
    //�}�E�X�J�[�\����\��
    private bool cursorLock = true;
    private float minX = -90f, maxX = 90f;

    // Start is called before the first frame update
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);
        
        //���_��90�x�ɐ���
        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        LateUpdateCursorLock();

        
    }
    private void FixedUpdate()
    {
        x = 0;
        z = 0;

        x = Input.GetAxisRaw("Horizontal") * xspeed * zspeed;
        z = Input.GetAxisRaw("Vertical") * xspeed * zspeed;

        //transform.position += new Vector3(x, 0, z);

        transform.position += cam.transform.forward * z + cam.transform.right * x;

    }

    public void LateUpdateCursorLock()
    {
        //Escape����������}�E�X�J�[�\���\��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        //���N���b�N�Ń}�E�X�J�[�\����\��
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }
        //false����\��
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        //true���\��
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
}
