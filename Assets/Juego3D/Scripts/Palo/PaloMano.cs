using System.Collections;
using UnityEngine;
public class PaloMano : MonoBehaviour
{

    [SerializeField] private GameObject mano;
    public bool coger = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            soltar();
            StartCoroutine("espera");   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mano"))
        {
            Debug.LogError("Colision con la mano");
            if (coger == true)
            {
                coger = false;
                transform.SetParent(mano.transform);
                transform.localPosition = new Vector3(-0.324000001f, 0.171000004f, 0.0810000002f);
                transform.localEulerAngles = new Vector3(0f, 0f, 69.336f);
            }
        }
    }

    // position Vector3(-0.324000001,0.171000004,0.0810000002)
    // rotacion 0, 0, 69.336
    

    private void soltar()
    {
        transform.SetParent(null);
        transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        Debug.LogError("Soltar palo");

    }

    IEnumerator espera()
    {
        yield return new WaitForSeconds(2);
        Debug.LogError("Espera 2 segundos");
        coger = true;
    }
    // position Vector3(54.6739998,21.8920002,-7.12699986)
}

