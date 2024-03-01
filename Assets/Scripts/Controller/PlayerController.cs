using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

    bool _moveToDest = false;
    Vector3 _destPos;

    // Start is called before the first frame update
    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

    }

    // Update is called once per frame

    void Update()
    {
        if (_moveToDest)
        {
            Vector3 dir = _destPos - transform.position;
            if (dir.magnitude < 0.0001f)
            {
                _moveToDest = false;
            }
            else
            {
                float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);

                transform.position += dir.normalized * moveDist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
                transform.LookAt(_destPos);
            }
        }
    }

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.forward);
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            transform.position += Vector3.forward * Time.deltaTime * _speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.back);
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            transform.position += Vector3.back * Time.deltaTime * _speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            transform.position += Vector3.left * Time.deltaTime * _speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.right);
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            transform.position += Vector3.right * Time.deltaTime * _speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
        }
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (evt != Define.MouseEvent.Click)
            return;

        Debug.Log("OnMouseClicked !");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        LayerMask mask = LayerMask.GetMask("Wall");
        //int mask = (1 << 6);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            _destPos = hit.point;
            _moveToDest = true;
            //Debug.Log($"Raycast Camera @ name : {hit.collider.gameObject.name} tag : {hit.collider.gameObject.tag}");
        }

    }
}
