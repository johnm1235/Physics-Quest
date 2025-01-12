using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LevelLoader : MonoBehaviourPunCallbacks
{
    public void Start()
    {

        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(10);
        PhotonNetwork.LoadLevel("Level1");
    }
}
