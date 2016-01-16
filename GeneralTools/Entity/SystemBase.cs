using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

namespace Pseudo
{
	public abstract class SystemBase : ISystem
	{
		[Inject]
		public ISystemManager SystemManager { get; private set; }
		[Inject]
		public IEntityManager EntityManager { get; private set; }
		[Inject]
		public IEventManager EventManager { get; private set; }
		[InjectOptional]
		public IAudioManager AudioManager { get; private set; }
		[InjectOptional]
		public IParticleManager ParticleManager { get; private set; }
		[InjectOptional]
		public IInputManager InputManager { get; private set; }

		[PostInject]
		protected virtual void Initialize() { }
	}
}
