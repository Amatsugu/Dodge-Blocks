using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public class Generator : MonoBehaviour
	{
		public enum GenerationPattern
		{
			Random,
			Spiral
		}

		public Transform parent;
		public Motor player;
		public List<ParticleSystem> particleSystems;
		public GenerationPattern pattern;
		public int width = 5;
		public int lenght = 20;
		public int phillarWidth;
		[Range(0,100)]
		public float phillarDensity = .5f;
		public int phillarHeight = 3;
		public float jitter = .5f;
		public int maxRows = 25;
		public bool generate = true;
		public bool generateCubes = true;
		public bool generatePhillars = true;
		public bool generateCeiling = false, generateWalls = false;
		public int loopDistance = 500;
		public int rearBuffer = 10;

		private ObjectPoolerWorld _cubePool;
		private ObjectPoolerWorld _phillarPool;
		private int curPos;
		private List<PooledGameObject> _cubes;
		private List<PooledGameObject> _phillars;
		private ParticleSystem.Particle[] _particles;
		private Vector3 _phillarPos;
		private Vector3 _cubePos;
		private int _wallCount = 1;

		void Start ()
		{
			_cubes = new List<PooledGameObject>();
			_phillars = new List<PooledGameObject>();
			_cubePool = ObjectPoolManager.GetPool("CubePool");
			_phillarPool = ObjectPoolManager.GetPool("PhillarPool");
			if (generateCeiling)
				_wallCount++;
			if (generateWalls)
				_wallCount += 2;
			GameMaster.loopOffset = loopDistance;
			Random.InitState(1);
		}
	
		void Update ()
		{
			if(generate)
			{

				//Cleanup distant cubes
				while(_cubes.Count > (maxRows + rearBuffer) * width * _wallCount)
				{
					for (int i = 0; i < width * _wallCount; i++)
					{
						if (_cubes[i] == null)
							continue;
						_cubes[i].Destroy();
					}
					_cubes.RemoveRange(0, width * _wallCount);
				}
				//Cleanup distant phillars
				while(_phillars.Count > (maxRows + rearBuffer) * phillarWidth * phillarHeight)
				{
					for(int i = 0; i < phillarWidth * phillarHeight; i++)
					{
						if (_phillars[i] == null)
							continue;
						_phillars[i].Destroy();
					}
					_phillars.RemoveRange(0, phillarWidth * phillarHeight);
				}

				//Generate new cubes ahead
				while (curPos < player.curPos.z + lenght)
				{
					//Generate
					if (generateCubes)
					{
						//Floor
						GenerateHorizontalPlane(Vector3.zero);
						//Ceiling
						if(generateCeiling)
							GenerateHorizontalPlane(new Vector3(0, phillarHeight));
						//Left Wall
						if(generateWalls)
							GenerateVerticalPlane(new Vector3(-(phillarWidth+1)/2, 1));
						//Right Wall
						if(generateWalls)
							GenerateVerticalPlane(new Vector3((phillarWidth+1)/2, 1));
					}
					if(generatePhillars)
					{
						for (int x = 0; x < phillarWidth; x++)
						{
							for(int y = 1; y <= phillarHeight; y++)
							{
								switch (pattern)
								{
									case GenerationPattern.Random:
										GenerateRandom(x, y);
										break;
									case GenerationPattern.Spiral:
										GenerateSpiral(x, y);
										break;
								}					
							}
						}
					}
					curPos++;
				}
			}
		}

		void GenerateSpiral(int x, int y)
		{
			
		}

		void GenerateRandom(int x, int y)
		{
			if (Random.value >= 1f - (phillarDensity / 100))
			{
				_phillarPos = new Vector3(x - (phillarWidth / 2), y, curPos);
				_phillars.Add(_phillarPool.Instantiate(_phillarPos, Quaternion.identity, parent));
			}
			else
				_phillars.Add(null);
		}

		void GenerateHorizontalPlane(Vector3 offset)
		{
			for (int x = 0; x < width; x++)
			{
				_cubePos = new Vector3(x - (width / 2), Random.Range(-jitter, jitter), curPos);
				_cubePos += offset;
				_cubes.Add(_cubePool.Instantiate(_cubePos, Quaternion.identity, parent));
			}
		}
		void GenerateVerticalPlane(Vector3 offset)
		{
			for (int x = 0; x < width; x++)
			{
				_cubePos = new Vector3(Random.Range(-jitter, jitter), x - (width / 2), curPos);
				_cubePos += offset;
				_cubes.Add(_cubePool.Instantiate(_cubePos, Quaternion.identity, parent));
			}
		}

		void LateUpdate()
		{
			//Shift everything back to allow for infinite generation
			if (player.curPos.z >= loopDistance)
			{
				player.Loop(loopDistance);
				EventManager.TriggerEvent(GameEvent.GENERATOR_LOOP); //Trigger Loop Event

				//Cubes
				foreach (PooledGameObject c in _cubes)
				{
					if (c == null)
						continue;
					c.transform.position = new Vector3
					{
						x = c.transform.position.x,
						y = c.transform.position.y,
						z = c.transform.position.z - loopDistance
					};
				}
				//Phillars
				foreach (PooledGameObject p in _phillars)
				{
					if (p == null)
						continue;
					p.transform.position = new Vector3
					{
						x = p.transform.position.x,
						y = p.transform.position.y,
						z = p.transform.position.z - loopDistance
					};
				}

				//Particles

				foreach (ParticleSystem p in particleSystems)
				{
					if (_particles == null || _particles.Length < p.maxParticles)
						_particles = new ParticleSystem.Particle[p.maxParticles];
					int pCount = p.GetParticles(_particles);
					for (int i = 0; i < pCount; i++)
					{
						_particles[i].position = new Vector3
						{
							x = _particles[i].position.x,
							y = _particles[i].position.y,
							z = _particles[i].position.z - loopDistance
						};
					}
					p.SetParticles(_particles, pCount);
				}

				//Change Reference point
				curPos -= loopDistance;

			}
		}


	}
}

