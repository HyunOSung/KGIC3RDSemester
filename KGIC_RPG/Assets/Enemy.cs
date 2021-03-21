using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어는 Idle, Run, AttackRun, Attack, Dead
//적 Idle, Run, AttackRun, Attack, Dead

public class Enemy : FSM
{
	public float moveSpeed;
	public float rotationSpeed;
	public float attackRunSpeed;
	public float sightRange;
    public float attackRange;

	public GameObject[] movePoints;

	public GameObject target;
    public Player player;





	CharacterController cc;

	protected override void Start()
	{
		base.Start();
		cc = GetComponent<CharacterController>();

		//currentHP = DataManager.Instance.GetEnemyDB(currentLevel).maxHP;

		SetState(State.Idle);
	}

    private void Update()
    {
        if (GetCurrentState() != State.Dead)
        {
            if (currentHP <= 0)
            {
                SetState(State.Dead);
            }
        }


        //업데이트 상에서 거리는 정상으로 나옴
        //Debug.Log(Vector3.Distance(player.transform.position, transform.position));
    }




    protected override IEnumerator Idle()
    {
        int i = Random.Range(0, movePoints.Length);
        target = movePoints[i];

        float time = 0f;
        Debug.Log("Idle Enemy");

        while (isNewState == false)
        {
            yield return null;
            time += Time.deltaTime;
            if (MoveUtil.DetectPlayer(player.transform, transform, sightRange))
            {
                SetState(State.AttackRun);
            }      
            else
            {
                if (time >= 1f)
                {
                    SetState(State.Patrol);
                }
            }

        }
    }

    IEnumerator Patrol()
    {
        //Debug.Log("Patrol " + isNewState);
        //Debug.Log("Moving Enemy");

        while (isNewState == false)
        {
            yield return null;

            //float dist = Vector3.Distance(target.transform.position, transform.position);
            //Debug.Log("InPatrol" + Mathf.Round(dist));
            
            if (MoveUtil.Move(target.transform.position, cc, sightRange, moveSpeed, rotationSpeed))
            {
                //Debug.Log(Vector3.Distance(target.transform.position, transform.position));
               
                if(MoveUtil.DetectPlayer(player.transform, transform, sightRange))
                {
                    SetState(State.AttackRun);
                }

                if (Vector3.Distance(target.transform.position, transform.position) < 1f)
                {
                    SetState(State.Idle);                   
                }
                //if (MoveUtil.DetectPlayer(target.transform, gameObject.transform, sightRange))
                //{

                //    SetState(State.AttackRun);
                //}
            }


        }
    }

    protected override IEnumerator AttackRun()
    {

        while (isNewState == false)
        {
            yield return null;

            if (MoveUtil.Move(player.transform.position, cc, sightRange, moveSpeed, rotationSpeed))
            {
                if (MoveUtil.DetectPlayer(player.transform, transform, attackRange))
                {
                    SetState(State.Attack);
                }


            }
            if (Vector3.Distance(player.transform.position, transform.position) > sightRange)
            {

                target = null;
                SetState(State.Idle);
            }
        }
    }
    void Damage()
    {
        Debug.Log("Damage");
    }

    [SerializeField]
    int currentLevel = 1;
    float currentHP = 5;

    public float attack = 1;

    public void SendDamage(float attack)
    {
        //타겟에 공격력을 전달

        if (target == null)
        {
            return;
        }

        player.TakenDamage(attack);
    }
    public void TakenDamage(float attack)
    {
        //상대방의 공격력을 받아서 내방어력 대비 HP 정산
        //float damage = DataManager.Instance.GetPlayerDB(currentLevel).baseDef - attack;

        float damage = attack;
        if (damage > 0)
        {
            if (currentHP <= 0)
            {
               
                player.marker.SetParent(null);
                player.marker.transform.position = transform.position;
                
                SetState(State.Dead);
            }
            else
            {
                Debug.Log("Hit_Enemy");
                currentHP -= damage;
                //target = DataManager.Instance.GetPlayer().gameObject;             
            }
        }
    }

    IEnumerator Attack()
    {
        float time = 0f;

        while (isNewState == false)
        {
            yield return null;
            time += Time.deltaTime;

            Debug.Log("isAttack?");
            if (time >= 1f)
            {
                SendDamage(attack);
                time = 0f;
            }


            if (Vector3.Distance(player.transform.position, transform.position) > attackRange)
            {
                SetState(State.AttackRun);
            }

            if (Vector3.Distance(player.transform.position, transform.position) > sightRange)
            {
                SetState(State.Idle);
            }
        }
    }

    IEnumerator Dead()
    {
        yield return null;

        Transform marker = transform.Find("marker");
        marker.SetParent(null);
        marker.position = transform.position;

        gameObject.SetActive(false);

        //DataManager.Instance.GetPlayer().marker.SetParent(null);
        //DataManager.Instance.GetPlayer().target = null;

        
        //Debug.Log("Enemy_Dead");
        //Destroy(gameObject);
    }


}
