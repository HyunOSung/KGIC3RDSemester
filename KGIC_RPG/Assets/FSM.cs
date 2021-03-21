using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
	public enum State
	{
		Idle = 0,
		Run = 1,
		Attack = 2,
		Dead = 3,
		AttackRun = 4,
		Patrol = 5
	}

	protected State currentState = State.Idle;
	protected Animator animator;

	protected bool isNewState = false;

    public State GetCurrentState()
    {
        return currentState;
    }


	private void OnEnable()
	{
		StartCoroutine(FsmUpdate());
	}

	protected virtual void Start()
	{
		animator = GetComponent<Animator>();
		//SetState(State.Idle);
	}

	IEnumerator FsmUpdate()
	{
		while (true)
		{
			isNewState = false;
			yield return StartCoroutine(currentState.ToString());
		}
	}

	protected virtual IEnumerator Idle()
	{
		yield return null;
	}

	protected virtual IEnumerator Run()
	{
		yield return null;
	}

	protected virtual IEnumerator AttackRun()
	{
		yield return null;
	}

	public void SetState(State newState)
	{
		isNewState = true;
		currentState = newState;
		animator.SetInteger("state", (int)newState);
	}
}
