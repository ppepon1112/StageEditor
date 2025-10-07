using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー体力管理
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    // ダメージ表示・HPバー・経験値バー
    public GameObject damageTextPrefab;
    public Slider HPBar;
    public Slider ExperienceBar;
    public TMP_Text HPText;
    public TMP_Text ExperienceText;
    // HP,経験値,防御力初期値
    public int maxHP = 100;
    public int experience = 0;
    public int maxExperience = 100;
    public int level = 1;
    public int currentHP;
    public int defence;
    public int attack;
    // ステータスデータ、インベントリ
    public PlayerStatusData statusData;
    private PlayerInventory inventory;
    private bool invincible = false;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<PlayerInventory>();
        //PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("保存完了");

        // ステータスデータの読み込み、最終位置の取得
        SaveLoadManager.Load(statusData);

        if (!PlayerWarpData.isWarping)
        {
            Vector3 savePos = new Vector3(
                statusData.saveData.posX,
                statusData.saveData.posY,
                statusData.saveData.posZ
            );
            transform.position = savePos;
        }
        else
        {
            Debug.Log("ワープ中のためセーブデータ位置を無視しました");
        }
        // セーブデータから復元
        level = statusData.saveData.level;
        currentHP = statusData.saveData.currentHP;
        maxHP = statusData.saveData.maxHP;
        experience = statusData.saveData.experience;
        maxExperience = statusData.saveData.maxExperience;
        defence = statusData.saveData.defence;
        attack = statusData.saveData.attack;
        if (HPBar != null)
        {
            HPBar.maxValue = statusData.saveData.maxHP;
            HPBar.value = statusData.saveData.currentHP;
        }
        if(ExperienceBar != null)
        {
            ExperienceBar.maxValue = statusData.saveData.maxExperience;
            ExperienceBar.value = statusData.saveData.experience;
        }
        StartCoroutine(AssignUIReferences());
        // 初期表示
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveLoadManager.Save(statusData);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoadManager.Load(statusData);
            Debug.Log("現在のHP: " + statusData.saveData.currentHP);
        }
        // ---HP回復---
        // Potion_001を持っている場合消費して回復
        // 回復量はHealBoost分
        if (Input.GetKeyDown(KeyCode.H))
        {
            bool used = inventory.UseItem("Potion_001");
        }
    }

    // ===【UI更新用】===
    private void UpdateUI()
    {
        if (HPBar != null)
        {
            HPBar.maxValue = statusData.saveData.maxHP;
            HPBar.value = statusData.saveData.currentHP;
        }
        if (ExperienceBar != null)
        {
            ExperienceBar.maxValue = statusData.saveData.maxExperience;
            ExperienceBar.value = statusData.saveData.experience;
        }
        if (HPText != null)
        {
            HPText.text = $"{statusData.saveData.currentHP} / {statusData.saveData.maxHP}";
        }
        if(ExperienceText != null)
        {
            ExperienceText.text = $"{statusData.saveData.experience} / {statusData.saveData.maxExperience}";
        }
    }

    // ===【回復】===
    // 現在のHPにアイテムの回復量を加算
    // セーブデータにも保存
    public void Heal(int amount)
    {
        statusData.saveData.currentHP = Mathf.Min(statusData.saveData.currentHP + amount, statusData.saveData.maxHP);
        currentHP = statusData.saveData.currentHP;
        if(HPBar != null)
        {
            HPBar.value = currentHP;
        }
        UpdateUI();
        Debug.Log($"HP回復:{statusData.saveData.currentHP}/{statusData.saveData.maxHP}");
    }
    // ===【経験値】===
    // 現在の経験値に新たに取得した経験値を加算
    // セーブデータにも保存
    public void Experience(int amount)
    {
        // セーブデータの最大経験値の値が入ってなければ中止
        if(statusData.saveData.maxExperience <= 0)
        {
            Debug.LogError("maxExperienceが0以下のため経験値計算を中止");
            return;
        }
        // 経験値の取得
        statusData.saveData.experience += amount;
        Debug.Log($"経験値 +{amount}(現在:{statusData.saveData.experience}/{statusData.saveData.maxExperience}");
        // 経験値がmaxExperience以上になればレベルアップ
        while(statusData.saveData.experience >= statusData.saveData.maxExperience)
        {
            statusData.saveData.experience -= statusData.saveData.maxExperience;
            LevelUp();
        }
        UpdateUI();
    }

    // ===【レベルアップ】===
    // レベルアップ時にステータスを強化
    // HP,経験値上限の解放(HPの最大値+20,経験値上限1.5倍)
    // HPの全回復
    private void LevelUp()
    {
        statusData.saveData.level++;
        statusData.saveData.maxHP += 20;
        statusData.saveData.currentHP = statusData.saveData.maxHP;
        statusData.saveData.maxExperience = Mathf.RoundToInt(statusData.saveData.maxExperience * 1.5f);
        statusData.saveData.attack = Mathf.RoundToInt(statusData.saveData.attack * 1.2f);
        statusData.saveData.defence = Mathf.RoundToInt(statusData.saveData.attack * 1.1f);

        UpdateUI();
        Debug.Log($"レベルが上がった！現在のレベル:{statusData.saveData.level},次の必要経験値:{statusData.saveData.maxExperience}");
    }
    // ===【バリア状態の判断】===
    public void SetInvincible(bool value)
    {
        invincible = value;
    }
    // ===【ダメージ処理】===
    // 現在HPから(敵からのダメージ量 - 防御力)分のダメージを受ける
    // バリア中はダメージ無効
    public void TakeDamage(int damage)
    {
        if (invincible)
        {
            Debug.Log("無敵中なのでダメージ0");
            return;
        }

        int actualDamage = damage - statusData.saveData.defence;

        if(actualDamage <= 0)
        {
            actualDamage = UnityEngine.Random.Range(1, 11);
        }
        else
        {
            float randFactor = UnityEngine.Random.Range(0.9f, 1.1f);
            actualDamage = Mathf.RoundToInt(actualDamage * randFactor);
            actualDamage = Mathf.Max(1, actualDamage);
        }

        statusData.saveData.currentHP -= actualDamage;
        currentHP = Mathf.Clamp(statusData.saveData.currentHP, 0, statusData.saveData.maxHP);
        
        UpdateUI();
        Debug.Log($"プレイヤーが{actualDamage}ダメージを受けた。残りHP：{currentHP}");

        ShowDamageText(actualDamage);

        if(currentHP <= 0)
        {
            Die();
        }
    }

    // ===【死亡処理】===
    void Die()
    {
        Debug.Log("プレイヤー死亡");
        Destroy(gameObject);
    }

    void ShowDamageText(int damage)
    {
        Vector3 spawnPos = transform.position + Vector3.up * 2f;
        GameObject obj = Instantiate(damageTextPrefab,spawnPos, Quaternion.identity);
        obj.GetComponent<DamageText>().SetUp(damage);
    }
    private IEnumerator AssignUIReferences()
    {
        yield return null; // 1フレーム待つ

        if (HPBar == null)
            HPBar = GameObject.Find("PlayerHPBar")?.GetComponent<Slider>();

        if (ExperienceBar == null)
            ExperienceBar = GameObject.Find("ExperienceBar")?.GetComponent<Slider>();

        if (HPText == null)
            HPText = GameObject.Find("HPText")?.GetComponent<TMP_Text>();

        if (ExperienceText == null)
            ExperienceText = GameObject.Find("ExperienceText")?.GetComponent<TMP_Text>();

        if (HPBar == null || ExperienceBar == null)
        {
            Debug.LogWarning("[PlayerHealth] UI要素が見つかりません。Canvasのオブジェクト名を確認してください。");
        }
        else
        {
            Debug.Log("[PlayerHealth] UI参照を自動割り当てしました。");
            UpdateUI();
        }
    }
    void OnApplicationQuit()
    {
        SaveLoadManager.Save(statusData);
    }
}
