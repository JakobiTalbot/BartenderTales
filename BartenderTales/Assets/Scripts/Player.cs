using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_mouseSensitivity = 10f;
    public float m_moveSpeed = 10f;

    private GameObject m_camera;
    private Rigidbody m_rb;

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
#endif
    }
}