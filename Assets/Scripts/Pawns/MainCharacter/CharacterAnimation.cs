using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

	public float deadZone = 5f;
	
	private Animator anim;
	private HashIDs hash;
	private AnimatorSetup animSetup;
    private ControllerBase unitController;


    void Awake(){
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        unitController = transform.parent.gameObject.GetComponent<ControllerBase>();

        animSetup = new AnimatorSetup(anim, hash);
		anim.SetLayerWeight(1, 1f);
		anim.SetLayerWeight(2, 1f);
		deadZone *= Mathf.Deg2Rad;
	}

	void Update(){
		AnimSetup();
	}

	void OnAnimatorMove(){
		transform.rotation = anim.rootRotation;
	}

	void AnimSetup(){
        //float speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
        //float angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);

        float speed = Vector3.Project(unitController.moveDirection, transform.forward).magnitude;
        float angle = FindAngle(transform.forward, unitController.moveDirection, transform.up);

        if (Mathf.Abs(angle) < deadZone){
			transform.LookAt(transform.position + unitController.moveDirection);
			angle = 0f;
		}
		animSetup.Setup(5000, angle);
	}

	float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector){
		if(toVector == Vector3.zero)
			return 0f;

		float angle = Vector3.Angle(fromVector, toVector);
		Vector3 normal = Vector3.Cross(fromVector, toVector);
		angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
		angle *= Mathf.Deg2Rad;

		return angle;
	}

}
