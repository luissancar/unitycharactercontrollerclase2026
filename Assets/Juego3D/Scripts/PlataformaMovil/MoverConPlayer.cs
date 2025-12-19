using System;
using UnityEngine;

public class MoverConPlayer : MonoBehaviour
{
    private bool estamosDentro = false;
    private GameObject player;

    // proyect settings -> Physics -> Settings [Game object] -> auto Sync trnasform (activar)
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player=other.gameObject;
            estamosDentro = true;
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (estamosDentro)
        {
            estamosDentro = false;
            player.transform.parent = null;
        }
    }
}