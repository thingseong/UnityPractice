using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;
    float _dashTime = 0.4f;
    Vector3 _destPos;
    float _startDashTime;
    Animator anime;


    PlayerState _state = PlayerState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();

        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        //Managers.Resource.Instantiate("UI/UI_Button");
        //UI_Button ui = Managers.UI.ShowPopupUI<UI_Button>();

        //Managers.UI.ShowSceneUI<UI_Inven>();

    }


    public enum PlayerState
    {
        Die,
        Moving,
        Run,
        Dashing,
        Idle,
    }

    void UpdateDie()
    {

    }

    void UpdateRun()
    {
        //Animator anime = GetComponent<Animator>();
        anime.SetBool("isDashing", false);
        anime.SetFloat("speed", _speed);
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

        //Animator anime = GetComponent<Animator>();
        //anime.SetBool("isDashing", false);
        anime.SetFloat("speed", _speed);

    }
    
    void UpdateDashing()
    {
        

        //Vector3 dir = _destPos - transform.position;
        if (Time.time >_startDashTime + _dashTime)
        {
            if(Time.time > _startDashTime + _dashTime + 0.03f)
                _state = PlayerState.Idle;
        }
        else if(Time.time < _startDashTime + 0.03f)
        {

        }
        else
        {
            //float moveDist = Mathf.Clamp(_speed * Time.deltaTime * 3, 0, dir.magnitude);
            float moveDist = _speed * Time.deltaTime * 3;
            transform.position += transform.forward * moveDist;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            //transform.LookAt(_destPos);
        }

        //Animator anime = GetComponent<Animator>();
        //anime.SetFloat("speed", 0);
        anime.SetBool("isDashing", true);

    }

    void UpdateIdle()
    {
        //Animator anime = GetComponent<Animator>();
        anime.SetFloat("speed", 0);
        anime.SetBool("isDashing", false);

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
            case PlayerState.Run:
                UpdateRun();
                break;
            case PlayerState.Dashing:
                UpdateDashing();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }

    void OnKeyboard()
    {
        if(_state == PlayerState.Dashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _state = PlayerState.Dashing;
            //_destPos = transform.position + transform.forward * 10.0f;
            _startDashTime = Time.time;
            Managers.Sound.Play("UnityChan/univ0001");
            //Debug.Log(transform.forward);
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.forward);
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            Vector3 dirTo12 = new Vector3(-1.0f, 0.0f, 1.0f).normalized;
            transform.position += dirTo12 * Time.deltaTime * _speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirTo12), 0.2f);
            _state = PlayerState.Run;

        }
        if (Input.GetKey(KeyCode.S))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.back);
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            Vector3 dirTo6 = new Vector3(1.0f, 0.0f, -1.0f).normalized;
            transform.position += dirTo6 * Time.deltaTime * _speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirTo6), 0.2f);
            _state = PlayerState.Run;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            Vector3 dirTo9 = new Vector3(-1.0f, 0.0f, -1.0f).normalized;
            transform.position += dirTo9 * Time.deltaTime * _speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirTo9), 0.2f);
            _state = PlayerState.Run;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.right);
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            Vector3 dirTo3 = new Vector3(1.0f, 0.0f, 1.0f).normalized;
            transform.position += dirTo3 * Time.deltaTime * _speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirTo3), 0.2f);
            _state = PlayerState.Run;
        }


        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            _state = PlayerState.Idle;
            Debug.Log("123123");
        }


        


        
    }

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
