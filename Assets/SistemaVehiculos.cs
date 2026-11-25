
using UnityEngine;
using UnityEngine.InputSystem;

public class SistemaVehiculos : MonoBehaviour
{
    [Header("Referencias")]
    public Camera cam;

    [Header("Vehiculo")]
    public GameObject vehiculo;

    [Header("Apuntado")]
    public float distanciaApuntado = 40f;
    public Color colorResaltado = new Color(0f, 0.2f, 1f, 0.4f);
    public float escalaResaltado = 1.1f;

    private GameObject resaltado;
    private Material materialResaltado;

    private bool montado = false;

    private Transform padreOriginal;
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        if (vehiculo == null)
        {
            Debug.LogError("Asigna PickUp_Damaged en Vehiculo.");
            enabled = false;
            return;
        }

        if (cam == null)
        {
            Debug.LogError("No se encuentra la camara.");
            enabled = false;
            return;
        }

        padreOriginal = cam.transform.parent;
        posicionOriginal = cam.transform.localPosition;
        rotacionOriginal = cam.transform.localRotation;

        materialResaltado = new Material(
            Shader.Find("Sprites/Default")
        );

        materialResaltado.color = colorResaltado;

        CrearResaltado();
    }

    void Update()
    {
        Apuntar();
    }

    // CREAR RESALTADO

    void CrearResaltado()
    {
        Renderer[] rs =
            vehiculo.GetComponentsInChildren<Renderer>();

        if (rs.Length == 0)
        {
            Debug.LogWarning("El vehiculo no tiene Renderer.");
            return;
        }

        Bounds b = rs[0].bounds;

        foreach (Renderer r in rs)
            b.Encapsulate(r.bounds);

        resaltado = GameObject.CreatePrimitive(
            PrimitiveType.Cube
        );

        resaltado.name = "ResaltadoAzul";

        Destroy(resaltado.GetComponent<Collider>());

        resaltado.transform.position = b.center;
        resaltado.transform.rotation = Quaternion.identity;
        resaltado.transform.localScale =
            b.size * escalaResaltado;

        MeshRenderer mr =
            resaltado.GetComponent<MeshRenderer>();

        mr.material = materialResaltado;

        mr.shadowCastingMode =
            UnityEngine.Rendering.ShadowCastingMode.Off;

        mr.receiveShadows = false;

        resaltado.SetActive(false);
    }

    // DETECTAR VEHICULO

    void Apuntar()
    {
        if (cam == null || vehiculo == null || montado)
            return;

        bool mirandoVehiculo = false;

        Ray ray = new Ray(
            cam.transform.position,
            cam.transform.forward
        );

        RaycastHit[] impactos = Physics.RaycastAll(
            ray,
            distanciaApuntado
        );

        System.Array.Sort(
            impactos,
            (a, b) => a.distance.CompareTo(b.distance)
        );

        foreach (RaycastHit impacto in impactos)
        {
            Transform golpeado = impacto.collider.transform;

            // Ignorar objetos que no sean el vehículo
            if (golpeado == cam.transform ||
                golpeado.IsChildOf(cam.transform))
            {
                continue;
            }

            // Comprobar si pertenece al vehículo
            if (golpeado == vehiculo.transform ||
                golpeado.IsChildOf(vehiculo.transform))
            {
                mirandoVehiculo = true;
            }

            // El primer objeto que no sea el jugador
            // bloquea la visión si no es el vehículo.
            break;
        }

        if (resaltado != null)
            resaltado.SetActive(mirandoVehiculo);

        if (mirandoVehiculo &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            Montarse();
        }
    }

    // MONTARSE

    void Montarse()
    {
        Transform puntoCamara =
            vehiculo.transform.Find("Camara");

        if (puntoCamara == null)
        {
            Debug.LogError(
                "No existe un objeto llamado Camara " +
                "dentro de PickUp_Damaged."
            );

            return;
        }

        cam.transform.SetParent(puntoCamara, false);

        cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.identity;

        montado = true;

        if (resaltado != null)
            resaltado.SetActive(false);

        Debug.Log("Te has montado en el vehiculo.");
    }

    // SALIR DEL VEHICULO

    public void SalirVehiculo()
    {
        if (!montado)
            return;

        cam.transform.SetParent(padreOriginal, false);

        cam.transform.localPosition = posicionOriginal;
        cam.transform.localRotation = rotacionOriginal;

        montado = false;

        Debug.Log("Has salido del vehiculo.");
    }

    void OnDestroy()
    {
        if (resaltado != null)
            Destroy(resaltado);

        if (materialResaltado != null)
            Destroy(materialResaltado);
    }
}
