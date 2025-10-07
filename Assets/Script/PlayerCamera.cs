using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : MonoBehaviour
{
    public Transform target; // プレイヤー
    public Vector3 offset = new Vector3(0f, 5f, -8f);
    public float rotateSpeed = 50f;
    public float mouseSensitivity = 2f;
    public float minPitch = -30f;
    public float maxPitch = 60f;
    public float cameraDistance = 8f;
    public LayerMask collisionMask;

    private float pitch = 10f;

    void Start()
    {
        if(target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
            {
                target = player.transform;
            }
        }
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            target = player.transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void LateUpdate()
    {
        if (!target) return;

        // Q/Eで左右回転
        float rotateInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateInput = -1f;
        if (Input.GetKey(KeyCode.E)) rotateInput = 1f;

        // カメラの位置を回転
        if (rotateInput != 0f)
        {
            offset = Quaternion.Euler(0, rotateInput * rotateSpeed * Time.deltaTime, 0) * offset;
        }

        // ---マウス上下でピッチ調整---
        float mouseY = Input.GetAxis("Mouse Y");
        pitch -= mouseY * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion pitchRotation = Quaternion.Euler(pitch, 0, 0);
        Vector3 rotatedOffset = pitchRotation * offset.normalized * cameraDistance;

        Vector3 targetPos = target.position + Vector3.up * 1.5f;
        Vector3 desiredPos = targetPos + rotatedOffset;

        Ray ray = new Ray(targetPos, rotatedOffset.normalized);
        if(Physics.Raycast(ray,out RaycastHit hit,cameraDistance,collisionMask))
        {
            desiredPos = hit.point - rotatedOffset.normalized * 0.3f;
        }

        // カメラの位置・向きを更新
        transform.position = desiredPos;
        transform.LookAt(targetPos); // 少し見上げる
    }
}
