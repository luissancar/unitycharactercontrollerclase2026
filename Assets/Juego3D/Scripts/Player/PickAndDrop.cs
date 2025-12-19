using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickAndDrop : MonoBehaviour
{
    [Header("Mochila")] [SerializeField] private Transform mochila;

    [Header("Input System")]
    [Tooltip(
        "Opcional: arrastra aquí la acción 'Soltar' (InputActionReference). " +
        "Si lo dejas vacío, se busca por nombre en PlayerInput.")]
    [SerializeField]
    private InputActionReference soltarActionRef;

    [SerializeField] private string soltarActionName = "Soltar";

    [Header("Drop")] [SerializeField] private Vector3 dropOffset = new Vector3(2f, 0f, 0f);

    private GameObject objetoEnMochila;

    private InputAction soltarAction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // 1) Si el usuario asigna un InputActionReference, lo usamos
        if (soltarActionRef != null)
        {
            // Guardamos la acción "Soltar" directamente desde la referencia
            soltarAction = soltarActionRef.action;
        }
        else
        {
            // 2) Si no hay referencia, buscamos la acción por nombre en el componente PlayerInput del Player
            var playerInput = GetComponent<PlayerInput>();

            // Si existe PlayerInput, buscamos dentro de su asset una acción llamada "Soltar"
            if (playerInput != null)
                soltarAction = playerInput.actions.FindAction(soltarActionName, throwIfNotFound: false);
        }

        // Si no encontramos la acción, avisamos por consola para que lo arregles
        if (soltarAction == null)
            Debug.LogError(
                $"No se encontró la acción '{soltarActionName}'. Asigna soltarActionRef o añade PlayerInput con esa acción.");
    }

    private void OnEnable()
    {
        // Solo si existe la acción (no es null)
        if (soltarAction != null)
        {
            // Nos suscribimos al evento performed (cuando se ejecuta la acción)
            soltarAction.performed += OnSoltarPerformed;
            // Activamos la acción (por si no estaba activada)
            soltarAction.Enable();
        }
    }

    private void OnSoltarPerformed(InputAction.CallbackContext obj) => Soltar();
    private void OnTriggerEnter(Collider other) => TryPick(other.gameObject);

    private void Soltar()
    {
        if (objetoEnMochila == null)
            return;
        objetoEnMochila.transform.SetParent(null);
        objetoEnMochila.transform.position = transform.TransformPoint(dropOffset);

        objetoEnMochila = null;
    }

    private void TryPick(GameObject go)
    {
        if (objetoEnMochila != null)
            return;
        if (!go.CompareTag("Pick"))
            return;
        objetoEnMochila = go;
        if (objetoEnMochila.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        objetoEnMochila.transform.SetParent(mochila, worldPositionStays: false);
        objetoEnMochila.transform.localPosition = Vector3.zero;
        objetoEnMochila.transform.localRotation = Quaternion.identity;
    }
}