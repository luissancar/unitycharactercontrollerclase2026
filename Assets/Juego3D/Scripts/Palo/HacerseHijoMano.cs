using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HacerseHijoMano : MonoBehaviour
{
    [SerializeField] private GameObject mano;
    private bool tenemosPalo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mano") && !tenemosPalo)
        {
            tenemosPalo = true;
            transform.SetParent(mano.transform);
            transform.localPosition = new Vector3(-0.332999021f, -0.248126999f, -0.527649999f);
            transform.localRotation=Quaternion.Euler(350.021179f,135.050873f,240.544006f);
        }
    }

    public void SoltarPalo()
    {
        if (tenemosPalo)
        {
           StartCoroutine("Esperar");
        }
    }
    IEnumerator Esperar()
    {
        transform.SetParent(null);
        yield return new WaitForSeconds(2f);
        tenemosPalo = false;
    }

}
