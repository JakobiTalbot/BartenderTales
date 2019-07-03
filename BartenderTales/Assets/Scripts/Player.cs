using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_mouseSensitivity = 10f;
    public float m_moveSpeed = 10f;

    private GameObject m_camera;
    private GameObject m_selectedObject;
    private Rigidbody m_rb;

    private float m_fDistance;
    public float m_itemMoveSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_camera = Camera.main.gameObject;
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        Vector3 v3Movement = Vector3.zero;
        // movement
        if (Input.GetKey(KeyCode.W))
            v3Movement += transform.forward;
        if (Input.GetKey(KeyCode.A))
            v3Movement -= transform.right;
        if (Input.GetKey(KeyCode.S))
            v3Movement -= transform.forward;
        if (Input.GetKey(KeyCode.D))
            v3Movement += transform.right;
        m_rb.velocity = v3Movement * m_moveSpeed;

        // camera rotation
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * m_mouseSensitivity * Time.deltaTime, 0));
        m_camera.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * m_mouseSensitivity * Time.deltaTime, 0, 0));

        // pickup stuff
        if (!m_selectedObject && Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 10f)
                && hit.transform.GetComponent<Rigidbody>())
            {
                m_selectedObject = hit.transform.gameObject;
                m_fDistance = hit.distance;
                m_selectedObject.GetComponent<Rigidbody>().drag = 10f;
            }
        }
        else if (m_selectedObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 v3Dest = ray.origin + (ray.direction * m_fDistance);
            Vector3 v3Dir = v3Dest - m_selectedObject.transform.position;
            m_selectedObject.GetComponent<Rigidbody>().AddForce(v3Dir * m_itemMoveSpeed * m_selectedObject.GetComponent<Rigidbody>().mass);

            if (Input.GetKeyDown(KeyCode.E))
            {
                m_selectedObject.GetComponent<Rigidbody>().drag = 1f;
                m_selectedObject = null;
            }
        }
#endif
    }
}