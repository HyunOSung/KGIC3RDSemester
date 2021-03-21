using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Player : FSM
{
    public Transform marker;
    public float moveSpeed;
    public float rotationSpeed;

    LayerMask layerMask;

    CharacterController cc;

    AnimationClip anim;


    protected override void Start()
    {
        base.Start();
        cc = GetComponent<CharacterController>();
        layerMask = LayerMask.GetMask("Enemy", "Board", "Item");
    }

    void Update()
    {
        Vector3 markerXZ = new Vector3(
            marker.position.x,
            transform.position.y,
            marker.position.z);
        if (GetCurrentState() != State.Dead)
        {
            if (currentHP <= 0)
            {
                SetState(State.Dead);
            }
        }

        if (Input.GetMouseButtonDown(0) == true)
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, rayLength, layerMask))
            {
                //Debug.Log("Picked! " + hitInfo.collider.gameObject);

                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    marker.SetParent(hitInfo.collider.gameObject.transform, false);
                    marker.position = hitInfo.point;
                    SetState(State.AttackRun);
                }
                else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Board"))
                {
                    marker.SetParent(null);
                    marker.position = hitInfo.point;
                    target = null;
                    SetState(State.Run);
                }
            }          
        }

        if (Vector3.Distance(markerXZ, transform.position) <= 0.01f)
        {
            if (currentState == State.Run)
            {
                SetState(State.Idle);
            }
            return;
        }
        else
        {
            Vector3 dir = marker.position - transform.position;
            Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);

            cc.Move(dirXZ.normalized * moveSpeed * Time.deltaTime
                + Vector3.up * Physics.gravity.y * Time.deltaTime);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(dirXZ.normalized),
                rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {

    }

    public float rayLength;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * rayLength);
    }

    protected override IEnumerator Idle()
    {
        //Debug.Log(gameObject.name + ".idle.start");


        while (isNewState == false)
        {
            //Debug.Log(gameObject.name + ".idle.update");
            yield return null;
        }

        //Debug.Log(gameObject.name + ".idle.end");
    }

    protected override IEnumerator Run()
    {
        //Debug.Log(gameObject.name + ".run.start");
        Vector3 markerXZ = new Vector3(
        marker.position.x,
        transform.position.y,
        marker.position.z);
        while (isNewState == false)
        {
            //Debug.Log(gameObject.name + ".run.update");
            yield return null;

            if(Vector3.Distance(markerXZ, transform.position) <= 0.01f)
            {
                SetState(State.Idle);
            }
        }

        //Debug.Log(gameObject.name + ".run.end");
    }

    float targetTime = 0;

    public Enemy target;

    IEnumerator Attack()
    {
        target = marker.GetComponentInParent<Enemy>();


        //Debug.Log(targetTime);
        while (isNewState == false)
        {
            yield return null;

            targetTime = targetTime + Time.deltaTime;

            if (targetTime >= 0.9f)
            {
                SendDamage();                
            }

        }

    }

    protected override IEnumerator AttackRun()
    {
        //적이 죽었을 경우에는 Idle() 아니면 이 상태 유지

        //플레이어와 적의 거리를 판단해서 거리가 가까워지면 Attack() 아니면 이 상태 유지
        //Debug.Log(currentState);
        while(isNewState == false)
        {
            yield return null;
            
            if(marker.GetComponentInParent<Enemy>().GetCurrentState() == State.Dead)
            {
                SetState(State.Idle);
            }

            if (Vector3.Distance(transform.position, marker.position) <= 2.5f)
            {
                SetState(State.Attack);
            }
        }
    }

    IEnumerator Dead()
    {
        yield return null;


        gameObject.SetActive(false);
        //Destroy(gameObject);
        
    }

    void Damage()
    {
        Debug.Log(transform.name+ ".Damage");
        targetTime = 0;
        //SetState(State.Attack);
    }

    [SerializeField]
    int currentLevel = 1;
    float currentHP = 10;


    public float attack = 3;
    Enemy enemy;

    public void SendDamage()
    {
        if (target.GetCurrentState() == State.Dead)
        {
            target = null;
            SetState(State.Idle);
            return;
        }


        //타겟에 공격력을 전달
        //float attack = DataManager.Instance.GetPlayerDB(currentLevel).baseAttack;

        target.TakenDamage(attack);
    }
    public void TakenDamage(float attack)
    {
        //상대방의 공격력을 받아서 내방어력 대비 HP 정산
        //float damage = DataManager.Instance.GetPlayerDB(currentLevel).baseDef - attack;

        Debug.Log("Hit_Player");
        float damage = attack;
        if (damage > 0)
        {
            currentHP -= damage;
        }
    }
}

