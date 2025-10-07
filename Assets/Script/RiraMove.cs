using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(CharacterController))]
public class RiraMove : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float speedMultiplier = 1f;
    public float rotationSpeed = 720f;
    public float gravity = -9.8f;
    public float jumpForce = 5f;

    private Animator anim;
    private CharacterController controller;
    private Transform cam;

    private Vector3 velocity;
    private bool isGround;

    public bool isAttacking = false;
    public bool canMove = true;
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main?.transform;

        SceneManager.sceneLoaded += OnSceneLoaded;
        if(anim == null)
        {
            Debug.LogWarning("Animator not found on Player.");
        }
    }

    void OnDestroy()
    {
        // イベント解除（メモリリーク防止）
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンが読み込まれたら呼ばれる
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main?.transform;
    }

    void Update()
    {
        if (!canMove) return;
        // 攻撃中なら移動させない
        if (isAttacking)
        {
            anim.SetFloat("float speed", 0f);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        isGround = controller.isGrounded;
        if(isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            if(cam == null) cam = Camera.main?.transform;

            // カメラの向きに対して移動方向を算出
            Vector3 camForward = cam.forward;
            camForward.y = 0;
            camForward.Normalize();
            Vector3 camRight = cam.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 moveDir = camForward * v + camRight * h;
            float currentSpeed = moveSpeed * speedMultiplier;
            controller.Move(currentSpeed * Time.deltaTime * moveDir.normalized);

            // プレイヤーの向き変更
            Quaternion toRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotationSpeed * Time.deltaTime);
            if(anim != null)
            {
                anim.SetFloat("float speed", moveDir.magnitude);
            }
        }
        else
        {
            if(anim != null)
            {
                anim.SetFloat("float speed", 0f);
            }
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void SetSpeedMultiplier(float speed)
    {
        speedMultiplier = speed;
    }
}
