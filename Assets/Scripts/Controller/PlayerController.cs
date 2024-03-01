using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

    Vector3 _destPos;

    PlayerState _state = PlayerState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

    }


    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    void UpdateDie()
    {

    }

    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);

            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            transform.LookAt(_destPos);
        }

        // Animation

        Animator anime = GetComponent<Animator>();
        anime.SetFloat("speed", _speed);

    }

    void UpdateIdle()
    {
        Animator anime = GetComponent<Animator>();
        anime.SetFloat("speed", 0);

    }

    void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }

    //void OnKeyboard()
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        //transform.rotation = Quaternion.LookRotation(Vector3.forward);
    //        //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    //        transform.position += Vector3.forward * Time.deltaTime * _speed;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
    //    }
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        //transform.rotation = Quaternion.LookRotation(Vector3.back);
    //        //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    //        transform.position += Vector3.back * Time.deltaTime * _speed;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        //transform.rotation = Quaternion.LookRotation(Vector3.left);
    //        //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    //        transform.position += Vector3.left * Time.deltaTime * _speed;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
    //    }
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        //transform.rotation = Quaternion.LookRotation(Vector3.right);
    //        //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    //        transform.position += Vector3.right * Time.deltaTime * _speed;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
    //    }
    //}

    void OnMouseClicked(Define.MouseEvent evt)
    {

        if (_state == PlayerState.Die) return;

        //Debug.Log("OnMouseClicked !");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        LayerMask mask = LayerMask.GetMask("Wall");
        //int mask = (1 << 6);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
            //Debug.Log($"Raycast Camera @ name : {hit.collider.gameObject.name} tag : {hit.collider.gameObject.tag}");
        }

    }
}
