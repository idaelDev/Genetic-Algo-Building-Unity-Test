using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Architect : MonoBehaviour {

	public Rules[] rules;
	public float speed;
	public float rotateSpeed;
	public float distanceDelta = 0.1f;
	public float rotationDelta = 0.02f;
	public GameObject objectToDrop;

	public float time;


	public Vector3 positionToReach;
	private Vector3 movement;
	
	private Quaternion rotationToReach;
	private bool drop;
	private int ruleCount = 0;
	private bool actionDone = true;
	private bool moving = false;
	// Use this for initialization
	void Awake () {
		positionToReach = transform.position;
		rotationToReach = transform.rotation;
		drop = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!actionDone)
		{

//			StartCoroutine(TranslateOvertime(transform.position, positionToReach, 0.5f));
//			moving = true;
			transform.position = Vector3.Lerp(transform.position, positionToReach, speed*Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotationToReach, rotateSpeed * Time.deltaTime);

			if(drop)
			{
				Instantiate(objectToDrop, transform.position + transform.forward, Quaternion.identity);
				drop = false;
			}
			else if(Vector3.Distance(transform.position, positionToReach) <= distanceDelta && Quaternion.Angle(transform.rotation, rotationToReach) <= rotationDelta)
			{
				transform.position = positionToReach;
				transform.rotation = rotationToReach;
				IncrementRules();
				actionDone = true;
			}
		}
		else
		{
			Interpretor(rules[ruleCount]);
			actionDone = false;
		}
	}

	void Interpretor(Rules rule)
	{
		switch(rule)
		{
		case Rules.FORWARD :
			if(!Collide(transform.forward))
				positionToReach = transform.position + transform.forward;
			break;
		case Rules.BACKWARD :
			if(!Collide(-transform.forward))
				positionToReach = transform.position - transform.forward;
			break;
		case Rules.LEFT :
			if(!Collide(-transform.right))
			positionToReach = transform.position - transform.right;
			break;
		case Rules.RIGHT :
			if(!Collide(transform.right))
				positionToReach = transform.position + transform.right;
			break;
		case Rules.UP :
			if(!Collide(transform.up))
				positionToReach = transform.position + transform.up;
			break;
		case Rules.DOWN :
			if(!Collide(-transform.up))
				positionToReach = transform.position - transform.up;
			break;
		case Rules.TURNLEFT :
			rotationToReach = Quaternion.Euler(transform.rotation.eulerAngles +  transform.up*-90);
			break;
		case Rules.TURNRIGHT :
			rotationToReach = Quaternion.Euler(transform.rotation.eulerAngles + transform.up*90);
			break;
		case Rules.DROP :
			if(!Collide(transform.forward))
				drop = true;
			break;
		}
		
	}

	bool Collide(Vector3 direction)
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position, direction,out hit, 1))
		{
			return true;
		}
		return false;
	}

	void IncrementRules()
	{
		ruleCount++;
		if(ruleCount == rules.Length)
			ruleCount = 0;
	}

//	public IEnumerator TranslateOvertime(Vector3 startPoint,Vector3 endPoint,float time)
//	{
//		speed = 1f/time;
//		for(float t=0; t<1.0; t += Time.deltaTime*speed)
//		{
//			transform.position = Vector3.Lerp(startPoint , positionToReach ,  t);
//			yield return 0;
//		}
//		actionDone = true;
//		moving = false;
//	}
}
