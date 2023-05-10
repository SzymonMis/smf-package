using System.Collections.Generic;
using UnityEngine;

namespace SMF.Core
{
	public abstract class UITransition : MonoBehaviour
	{
		[SerializeField] protected AnimationCurve linearTransition = AnimationCurve.Linear(0, 0, 1, 1);
		[SerializeField] protected AnimationCurve easeInTransition = AnimationCurve.Linear(0, 0, 1, 1);
		[SerializeField] protected AnimationCurve easeOutTransition = AnimationCurve.Linear(0, 0, 1, 1);
		[SerializeField] protected AnimationCurve easeInOutTransition = AnimationCurve.Linear(0, 0, 1, 1);

		[SerializeField]
		protected AnimationCurve elasticTransition = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0.25f, 0.7f, 1, 0),
			new Keyframe(0.5f, 1, -1.5f, 0),
			new Keyframe(0.75f, 0.7f, 1, 0),
			new Keyframe(1, 1, 0, 0)
		});

		[SerializeField]
		protected AnimationCurve bounceTransition = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0.4f, 0.4f, 0, -1),
			new Keyframe(0.55f, 0.55f, 0, -2),
			new Keyframe(0.7f, 0.7f, 0, -1),
			new Keyframe(1, 1, 0, 0)
		});

		[SerializeField] protected AnimationCurve customTransition;

		[SerializeField] protected TransitionType transitionType;

		protected Dictionary<TransitionType, AnimationCurve> transitionFunctions;

		/// <summary>
		/// This function sets up the animation curves and dictionary of transition functions for use in the script.
		/// </summary>
		protected virtual void Awake()
		{
			easeInTransition.keys[1].inTangent = 1;
			easeOutTransition.keys[0].outTangent = 1;
			easeInOutTransition.keys[0].outTangent = 1;
			easeInOutTransition.keys[1].inTangent = 1;

			elasticTransition.preWrapMode = WrapMode.PingPong;
			elasticTransition.postWrapMode = WrapMode.PingPong;
			bounceTransition.preWrapMode = WrapMode.PingPong;
			bounceTransition.postWrapMode = WrapMode.PingPong;

			transitionFunctions = new Dictionary<TransitionType, AnimationCurve>()
			{
				{ TransitionType.Linear, linearTransition },
				{ TransitionType.EaseIn, easeInTransition },
				{ TransitionType.EaseOut, easeOutTransition },
				{ TransitionType.EaseInOut, easeInOutTransition },
				{ TransitionType.Elastic, elasticTransition },
				{ TransitionType.Bounce, bounceTransition },
				{ TransitionType.Custom, customTransition }
			};
		}

		/// <summary>
		/// Retrieves the animation curve associated with the given transition type from the transitionFunctions dictionary.
		/// </summary>
		/// <param name="transitionType">The type of transition whose animation curve to retrieve.</param>
		/// <returns>The animation curve associated with the given transition type.</returns>
		protected AnimationCurve GetTansition(TransitionType transitionType) => transitionFunctions[transitionType];

		public enum TransitionType
		{
			Linear,
			EaseIn,
			EaseOut,
			EaseInOut,
			Elastic,
			Bounce,
			Custom
		}
	}
}