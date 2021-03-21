using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other == null)
			return;

		if (other.CompareTag("Player"))
		{
			GetComponentInParent<Enemy>().target = other.gameObject;
			GetComponentInParent<Enemy>().SetState(FSM.State.AttackRun);
		}
	}
}
