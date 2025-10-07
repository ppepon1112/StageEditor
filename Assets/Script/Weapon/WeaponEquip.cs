using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    public GameObject weaponPrefab;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Transform rightHand = anim.GetBoneTransform(HumanBodyBones.LeftHand);

        if(rightHand != null && weaponPrefab != null)
        {
            GameObject weapon = Instantiate(weaponPrefab,rightHand);
            weapon.transform.localPosition = new Vector3(-0.1f, 0f, 0.2f);
            weapon.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
