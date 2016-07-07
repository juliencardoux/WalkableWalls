using UnityEngine;
using System.Collections;

public class PlayerWallClimber : MonoBehaviour {

	private Rigidbody _rb;
	[SerializeField]
	private float _speedDeplacement;
	[SerializeField]
	private float _jumpStrenght;
	[SerializeField]
	private float _gravityStrenght;

	[SerializeField]
	private float _waitDetection;
	private float _currentWaitDetection;

	private bool _jumped;

	private void Awake() {
		_rb = GetComponent<Rigidbody>();
	}

	private void Start() {
		StartCoroutine(WalkableDetection());
	}

	private void Update() {
		DrawPlayerBearings();
		PlayerMovements();

		Gravity();
	}

	private void PlayerMovements() {
		if(Input.GetKey(KeyCode.W)) _rb.AddForce(transform.forward * _speedDeplacement, ForceMode.Impulse);
		if(Input.GetKey(KeyCode.S)) _rb.AddForce(-transform.forward * _speedDeplacement, ForceMode.Impulse);
		if(Input.GetKey(KeyCode.A)) _rb.AddForce(-transform.right * _speedDeplacement, ForceMode.Impulse);
		if(Input.GetKey(KeyCode.D)) _rb.AddForce(transform.right * _speedDeplacement, ForceMode.Impulse);

		if(!_jumped) if(Input.GetKey(KeyCode.Space)) {
			_rb.AddForce(transform.up * _jumpStrenght, ForceMode.Impulse);
			_jumped = true;
		}
	}

	private IEnumerator WalkableDetection() {
		yield return new WaitForSeconds(_currentWaitDetection);
		_currentWaitDetection = 0;
		RaycastHit hit;
		if((Physics.Raycast(transform.position, transform.forward, out hit, 1f)) && (hit.collider.gameObject.layer == 8)) {
			RotatePlayer(transform.forward, transform.up);
			yield break;
		}
		if((Physics.Raycast(transform.position, transform.right, out hit, 1f)) && (hit.collider.gameObject.layer == 8)) {
			RotatePlayer(transform.right, transform.forward);
			yield break;
		}

		if((Physics.Raycast(transform.position, -transform.forward, out hit, 1f)) && (hit.collider.gameObject.layer == 8)) {
			RotatePlayer(-transform.forward, -transform.up);
			yield break;
		}
		if((Physics.Raycast(transform.position, -transform.right, out hit, 1f)) && (hit.collider.gameObject.layer == 8)) {
			RotatePlayer(-transform.right, transform.forward);
			yield break;
		}

		StartCoroutine(WalkableDetection());
	}

	private void RotatePlayer(Vector3 _from, Vector3 _to) {
		transform.rotation = Quaternion.LookRotation(_to , -_from);
		Debug.Log(Quaternion.LookRotation(_to , -_from).eulerAngles);
		_currentWaitDetection = _waitDetection;

		StartCoroutine(WalkableDetection());
	}

	private void DrawPlayerBearings() {
		Debug.DrawRay(transform.position, transform.forward, Color.blue);
		Debug.DrawRay(transform.position, transform.forward * -1, Color.cyan);
		Debug.DrawRay(transform.position, transform.right, Color.red);
		Debug.DrawRay(transform.position, transform.up, Color.green);
	}

	private void Gravity() {
		if(GroundCheck()) {
			_jumped = false;
			return;
		}
		else _rb.AddForce(-transform.up * _gravityStrenght, ForceMode.Impulse);
	}

	private bool GroundCheck() {
		Debug.DrawRay(transform.position, -transform.up * .6f, Color.red);
		if(Physics.Raycast(transform.position, -transform.up, .6f)) return true;
		else return false;
	}
}
