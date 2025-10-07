using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float speed = 1f;
    public float fadeDuration = 1f;

    private float timer = 0f;

    public void SetUp(int damage)
    {
        textMesh.text = damage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        if(Camera.main != null)
        {
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
        timer += Time.deltaTime;

        Color c = textMesh.color;
        c.a = Mathf.Lerp(1, 0, timer / fadeDuration);
        textMesh.color = c;

        if(timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
