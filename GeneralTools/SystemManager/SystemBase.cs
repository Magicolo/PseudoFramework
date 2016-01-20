using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

namespace Pseudo
{
	public abstract class SystemBase : ISystem
	{
		public bool Active
		{
			get { return active; }
			set
			{
				if (active != value)
				{
					active = value;

					if (active)
						OnActivate();
					else
						OnDeactivate();
				}
			}
		}
		public virtual float UpdateDelay
		{
			get { return 0f; }
		}
		public virtual float LateUpdateDelay
		{
			get { return 0f; }
		}

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

		bool active;

		public virtual void OnInitialize() { }

		public virtual void OnDestroy() { }

		public virtual void OnActivate() { }

		public virtual void OnDeactivate() { }
	}
}
