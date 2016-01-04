<<<<<<< HEAD:GeneralTools/Entity/EntityJanitor.cs
﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public class EntityJanitor : Singleton<EntityJanitor>
	{
		void OnDestroy()
		{
			EntityUtility.ClearAllEntityGroups();
		}
	}
=======
﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public interface IEntityUpdateable
	{
		bool Active { get; }

		void ComponentUpdate();
		void ComponentLateUpdate();
		void ComponentFixedUpdate();
	}
>>>>>>> Entity2:GeneralTools/Entity2/IEntityUpdateable.cs
}