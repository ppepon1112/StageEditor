using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffectTrigger : MonoBehaviour
{
    public enum WaterEffectType { None,Slow,Poison }
    public WaterEffectType effectType = WaterEffectType.Slow;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<RiraMove>();
        if(player != null)
        {
            if(effectType == WaterEffectType.Slow)
            {
                player.SetSpeedMultiplier(0.5f);
            }
            else if(effectType == WaterEffectType.Poison)
            {
                Debug.Log("ì≈ÇæÅIÅI");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<RiraMove>();
            if(player != null)
            {
                player.SetSpeedMultiplier(1.0f);
            }
        }
    }
}
