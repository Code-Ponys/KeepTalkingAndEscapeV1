using UnityEngine;

public class GhostParticleHandler : MonoBehaviour {
	
	//Allows change in the Editor for Technical Artist/Level designer
	//MeshGameObject not nessesary 
	[SerializeField] private GameObject _meshGameObject;
	[SerializeField] private Material _material;
	
	[SerializeField] private float _particleStartSpeed = 0.01f;
	[SerializeField] private float _particleStartLifeTime = 1;
	[SerializeField] private float _particleStartSize = 100f;
	[SerializeField] private float _particleRadius = 100f;
	
	[SerializeField] private float _particlePositionX = 0;
	[SerializeField] private float _particlePositionY = 0;
	[SerializeField] private float _particlePositionZ = 0;
	[SerializeField] private float _particleRotationX = 90;
	[SerializeField] private float _particleRotationY = 0;
	[SerializeField] private float _particleRotationZ = 0;
	
	[SerializeField] private int _particleMaxParticles = 100;
	[SerializeField] private float _particleNormalOffset = 0.01f;
	[SerializeField] private float _particleSizeMin = 100;
	[SerializeField] private float _particleSizeMax = 150;
	[SerializeField] private Color _particleColor;
	

	private ParticleSystem _particle;


	private void Start () {
		ParticleHandler();
	}
	//Changes size over time to give dynamic 
	private void Update() {
		UpdateParticleHandler();
	}
	
	/// <summary>
	/// Controlls the particle system from this script
	/// </summary>
	private void ParticleHandler() {
		if(_meshGameObject == null) {
			_meshGameObject = gameObject;
		}
		//Creates new game object with the particle
		var particleObject = new GameObject("Particle");
		particleObject.transform.parent = _meshGameObject.transform;
		particleObject.transform.position = new Vector3(_meshGameObject.transform.position.x + _particlePositionX, _meshGameObject.transform.position.y + _particlePositionY, _meshGameObject.transform.position.z + _particlePositionZ);
		particleObject.transform.rotation = Quaternion.Euler(_meshGameObject.transform.rotation.x + _particleRotationX,_meshGameObject.transform.rotation.y + _particleRotationY,_meshGameObject.transform.rotation.z + _particleRotationZ);
		particleObject.transform.localScale = _meshGameObject.transform.localScale;
		//Sets layer, so only ghost can see it
		particleObject.layer = 9;
		
		//Assigns the particle system on the game object.
		_particle = particleObject.AddComponent<ParticleSystem>();
		_particle.GetComponent<Renderer>().material = _material;
		var particleMaster = _particle;
		var particleShape = _particle.shape;
		var particleCollision = _particle.collision;

		particleMaster.startSpeed = _particleStartSpeed;
		particleMaster.startSize = _particleStartSize;
		particleMaster.maxParticles = _particleMaxParticles;
		particleMaster.startLifetime = _particleStartLifeTime;
		particleMaster.startColor = _particleColor;
		
		particleShape.radius = _particleRadius;
		particleShape.shapeType = ParticleSystemShapeType.Circle;
		particleShape.normalOffset = _particleNormalOffset;

		particleCollision.type = ParticleSystemCollisionType.World;
	}
	
	private void UpdateParticleHandler() {
		_particle.startSize = Random.Range(_particleSizeMin, _particleSizeMax);
	}
}