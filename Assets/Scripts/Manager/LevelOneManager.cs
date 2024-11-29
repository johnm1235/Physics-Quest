using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneManager : MonoBehaviour
{
    [SerializeField] private AudioClip levelMusic;   
    [SerializeField] private AudioClip itemPickupSFX;
    [SerializeField] private AudioClip energyChargeSFX;
    [SerializeField] private AudioClip speedArrowSFX;

    private void Start()
    {

        AudioManager.Instance.PlayMusic(levelMusic);
    }

    public void PlayItemPickupSound()
    {

        AudioManager.Instance.PlaySFX(itemPickupSFX);
    }

    public void PlayEnergyBar()
    {

        AudioManager.Instance.PlaySFX(energyChargeSFX);
    }

    public void PlaySpeedArrow()
    {

        AudioManager.Instance.PlaySFX(speedArrowSFX);
    }
}
