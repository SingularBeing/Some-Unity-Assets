using UnityEngine;
using System.Collections;
using CYRO;

namespace CYRO
{

	[System.Serializable]
	public class STextureSettings
	{
		public class TSettings
		{
			public bool tileHorizontal;
			public bool tileVertical;
		}

		public TSettings settings = new TSettings ();

		public bool _tileHorizontal {
			get {
				return settings.tileHorizontal;
			}
			set {
				settings.tileHorizontal = value;
			}
		}

		public bool _tileVertical {
			get {
				return settings.tileVertical;
			}
			set {
				settings.tileVertical = value;
			}
		}

	}

}