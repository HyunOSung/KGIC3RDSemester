using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUtil : MonoBehaviour
{
	public static bool Move(Vector3 target, CharacterController owner, float sightRange, float moveSpeed, float rotationSpeed)
	{
		Vector3 dir = target - owner.transform.position;
		Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);

		owner.Move(dirXZ.normalized * moveSpeed * Time.deltaTime
				+ Vector3.up * Physics.gravity.y * Time.deltaTime);

		owner.transform.rotation = Quaternion.RotateTowards(
			owner.transform.rotation,
			Quaternion.LookRotation(dirXZ.normalized),
			rotationSpeed * Time.deltaTime);

		if (Vector3.Distance(target, owner.transform.position) <= sightRange)
			return true;
		else
			return false;
	}

	public static bool DetectPlayer(Transform player, Transform owner, float sightRange)
	{
		if (player.CompareTag("Player") && Vector3.Distance(player.position, owner.position) <= sightRange)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
