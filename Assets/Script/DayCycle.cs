using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public static DayCycle Instance;
    public bool IsNight => timeOfDay > 0.5f && timeOfDay < 0.9f;

    public delegate void TimePeriodChanged(bool isNight);
    public event TimePeriodChanged OnTimePeriodChanged;

    private bool currentIsNight;
    // ���z�A�F�ω��A���邳�ω��A1���̒���
    public Light directionalLight;
    public Gradient lightColor;
    public AnimationCurve lightIntensity;
    public float dayDuration = 120f;

    [Range(0f, 1f)] public float timeOfDay = 0.25f;

    void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        // ���ԏ���
        float prev = timeOfDay;
        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay > 1f) timeOfDay -= 1f;

        bool newIsNight = IsNight;
        if(newIsNight != currentIsNight)
        {
            currentIsNight = newIsNight;
            OnTimePeriodChanged?.Invoke(currentIsNight);
        }

        UpdateLighting();
    }

    private void UpdateLighting()
    {
        if(directionalLight != null)
        {
            directionalLight.color = lightColor.Evaluate(timeOfDay);
            directionalLight.intensity = lightIntensity.Evaluate(timeOfDay);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timeOfDay * 360f) - 90f, 0f, 0f));
        }
    }
}
