using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public Transform handBone;
    public PlayerInventory inventory;
    public RiraAttack attackLogic;

    GameObject currentWeaponGO;
    int currentIndex = -1;

    void Start()
    {
        if (!handBone)
            handBone = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftHand);
        //NextWeapon();
    }
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) PrevWeapon();
        if (Input.GetKeyDown(KeyCode.P)) NextWeapon();
    }
    void PrevWeapon() => Switch(-1);
    void NextWeapon() => Switch(+1);
    void Switch(int delta)
    {
        List<ItemData> weaponList = new List<ItemData>();

        foreach(var entry in inventory.statusData.saveData.ownedItems)
        {
            ItemData data = inventory.itemDatabase.GetItemByName(entry.itemID);
            if (data != null && data.type == ItemType.Weapon)
            {
                weaponList.Add(data);
            }
        }
        if (weaponList.Count == 0) return;

        currentIndex = (currentIndex + delta + weaponList.Count) % weaponList.Count;
        ItemData newWeapon = weaponList[currentIndex];

        // ãåïêäÌçÌèú
        if(currentWeaponGO) Destroy(currentWeaponGO);
        // êVÇµÇ¢ïêäÌê∂ê¨
        currentWeaponGO = Instantiate(newWeapon.equipPrefab,handBone);
        currentWeaponGO.transform.localPosition = newWeapon.equipPosition;
        currentWeaponGO.transform.localRotation = Quaternion.Euler(newWeapon.equipRotation);

        attackLogic.SetDamage(newWeapon.attackBoost);

        var sword = currentWeaponGO.GetComponentInChildren<SwordColliderController>();
        if (sword != null)
        {
            attackLogic.SetSword(sword);
        }
        //inventory.Equals(newWeapon);
    }
}
