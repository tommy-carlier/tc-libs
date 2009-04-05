// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Animation
{
	/// <summary>Static class that provides animation functionality.</summary>
	public static class Animator
	{
		#region initialization of the control for performing animation-steps

		private static Control fInvokeControl;
		private static readonly AnimationThread fAnimationThread = new AnimationThread();

		/// <summary>Initializes the <see cref="T:Animator"/> class.</summary>
		/// <remarks>This method should be called on the UI-thread.</remarks>
		internal static void Initialize()
		{
			if (fInvokeControl == null)
			{
				fInvokeControl = new Control();
				IntPtr lHandle = fInvokeControl.Handle;
			}
		}

		internal static void Perform(Action action)
		{
			fInvokeControl.BeginInvoke(action, null);
		}

		#endregion

		#region public Animate-methods

		/// <summary>Starts an animation.</summary>
		/// <typeparam name="TTarget">The type of object to animate a property of.</typeparam>
		/// <typeparam name="TValue">The type of value to animate.</typeparam>
		/// <param name="target">The object to animate a property of.</param>
		/// <param name="propertyName">The name of the property to animate.</param>
		/// <param name="startValue">The start-value of the animation.</param>
		/// <param name="endValue">The end-value of the animation.</param>
		/// <param name="duration">The duration of the animation.</param>
		public static void Animate<TTarget, TValue>(
			TTarget target,
			string propertyName,
			TValue startValue,
			TValue endValue,
			TimeSpan duration)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (propertyName == null) throw new ArgumentNullException("propertyName");
			if (propertyName.Length == 0)
				throw new ArgumentException("propertyName cannot be an empty string", "propertyName");

			AnimateCore(
				GetPropertySetter<TTarget, TValue>(propertyName),
				target,
				GetInterpolator<TValue>(),
				startValue,
				endValue,
				duration,
				0,
				null);
		}

		/// <summary>Starts an animation.</summary>
		/// <typeparam name="TTarget">The type of object to animate a property of.</typeparam>
		/// <typeparam name="TValue">The type of value to animate.</typeparam>
		/// <param name="target">The object to animate a property of.</param>
		/// <param name="propertyName">The name of the property to animate.</param>
		/// <param name="startValue">The start-value of the animation.</param>
		/// <param name="endValue">The end-value of the animation.</param>
		/// <param name="duration">The duration of the animation.</param>
		/// <param name="pulsationCount">The number of times to pulsate (= reverse the animation and start over).</param>
		public static void Animate<TTarget, TValue>(
			TTarget target,
			string propertyName,
			TValue startValue,
			TValue endValue,
			TimeSpan duration,
			int pulsationCount)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (propertyName == null) throw new ArgumentNullException("propertyName");
			if (propertyName.Length == 0)
				throw new ArgumentException("propertyName cannot be an empty string", "propertyName");
			if (pulsationCount < 0)
				throw new ArgumentOutOfRangeException("pulsationCount", "pulsationCount cannot be negative.");

			AnimateCore(
				GetPropertySetter<TTarget, TValue>(propertyName),
				target,
				GetInterpolator<TValue>(),
				startValue,
				endValue,
				duration,
				pulsationCount,
				null);
		}

		/// <summary>Starts an animation.</summary>
		/// <typeparam name="TTarget">The type of object to animate a property of.</typeparam>
		/// <typeparam name="TValue">The type of value to animate.</typeparam>
		/// <param name="target">The object to animate a property of.</param>
		/// <param name="propertyName">The name of the property to animate.</param>
		/// <param name="startValue">The start-value of the animation.</param>
		/// <param name="endValue">The end-value of the animation.</param>
		/// <param name="duration">The duration of the animation.</param>
		/// <param name="callback">The function that is called after the animation has finished (can be null).</param>
		public static void Animate<TTarget, TValue>(
			TTarget target,
			string propertyName,
			TValue startValue,
			TValue endValue,
			TimeSpan duration,
			Action callback)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (propertyName == null) throw new ArgumentNullException("propertyName");
			if (propertyName.Length == 0) throw new ArgumentException("propertyName cannot be an empty string", "propertyName");

			AnimateCore(
				GetPropertySetter<TTarget, TValue>(propertyName),
				target,
				GetInterpolator<TValue>(),
				startValue,
				endValue,
				duration,
				0,
				callback);
		}

		/// <summary>Starts an animation.</summary>
		/// <typeparam name="TTarget">The type of object to animate a property of.</typeparam>
		/// <typeparam name="TValue">The type of value to animate.</typeparam>
		/// <param name="target">The object to animate a property of.</param>
		/// <param name="propertyName">The name of the property to animate.</param>
		/// <param name="startValue">The start-value of the animation.</param>
		/// <param name="endValue">The end-value of the animation.</param>
		/// <param name="duration">The duration of the animation.</param>
		/// <param name="pulsationCount">The number of times to pulsate (= reverse the animation and start over).</param>
		/// <param name="callback">The function that is called after the animation has finished (can be null).</param>
		public static void Animate<TTarget, TValue>(
			TTarget target,
			string propertyName,
			TValue startValue,
			TValue endValue,
			TimeSpan duration,
			int pulsationCount,
			Action callback)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (propertyName == null) throw new ArgumentNullException("propertyName");
			if (propertyName.Length == 0) throw new ArgumentException("propertyName cannot be an empty string", "propertyName");
			if (pulsationCount < 0) throw new ArgumentOutOfRangeException("pulsationCount", "pulsationCount cannot be negative.");

			AnimateCore(
				GetPropertySetter<TTarget, TValue>(propertyName),
				target,
				GetInterpolator<TValue>(),
				startValue,
				endValue,
				duration,
				pulsationCount,
				callback);
		}

		/// <summary>Starts an animation.</summary>
		/// <typeparam name="TTarget">The type of object to animate a property of.</typeparam>
		/// <typeparam name="TValue">The type of value to animate.</typeparam>
		/// <param name="target">The object to animate a property of.</param>
		/// <param name="propertyName">The name of the property to animate.</param>
		/// <param name="interpolator">A function that calculates the intermediate values between startValue and endValue.</param>
		/// <param name="startValue">The start-value of the animation.</param>
		/// <param name="endValue">The end-value of the animation.</param>
		/// <param name="duration">The duration of the animation.</param>
		public static void Animate<TTarget, TValue>(
			TTarget target,
			string propertyName,
			Interpolator<TValue> interpolator,
			TValue startValue,
			TValue endValue,
			TimeSpan duration)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (propertyName == null) throw new ArgumentNullException("propertyName");
			if (propertyName.Length == 0) throw new ArgumentException("propertyName cannot be an empty string", "propertyName");
			if (interpolator == null) throw new ArgumentNullException("interpolator");

			AnimateCore(
				GetPropertySetter<TTarget, TValue>(propertyName),
				target,
				interpolator,
				startValue,
				endValue,
				duration,
				0,
				null);
		}

		/// <summary>Starts an animation.</summary>
		/// <typeparam name="TTarget">The type of object to animate a property of.</typeparam>
		/// <typeparam name="TValue">The type of value to animate.</typeparam>
		/// <param name="target">The object to animate a property of.</param>
		/// <param name="propertyName">The name of the property to animate.</param>
		/// <param name="interpolator">A function that calculates the intermediate values between startValue and endValue.</param>
		/// <param name="startValue">The start-value of the animation.</param>
		/// <param name="endValue">The end-value of the animation.</param>
		/// <param name="duration">The duration of the animation.</param>
		/// <param name="pulsationCount">The number of times to pulsate (= reverse the animation and start over).</param>
		public static void Animate<TTarget, TValue>(
			TTarget target,
			string propertyName,
			Interpolator<TValue> interpolator,
			TValue startValue,
			TValue endValue,
			TimeSpan duration,
			int pulsationCount)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (propertyName == null) throw new ArgumentNullException("propertyName");
			if (propertyName.Length == 0) throw new ArgumentException("propertyName cannot be an empty string", "propertyName");
			if (interpolator == null) throw new ArgumentNullException("interpolator");
			if (pulsationCount < 0) throw new ArgumentOutOfRangeException("pulsationCount", "pulsationCount cannot be negative.");

			AnimateCore(
				GetPropertySetter<TTarget, TValue>(propertyName),
				target,
				interpolator,
				startValue,
				endValue,
				duration,
				pulsationCount,
				null);
		}

		/// <summary>Starts an animation.</summary>
		/// <typeparam name="TTarget">The type of object to animate a property of.</typeparam>
		/// <typeparam name="TValue">The type of value to animate.</typeparam>
		/// <param name="target">The object to animate a property of.</param>
		/// <param name="propertyName">The name of the property to animate.</param>
		/// <param name="interpolator">A function that calculates the intermediate values between startValue and endValue.</param>
		/// <param name="startValue">The start-value of the animation.</param>
		/// <param name="endValue">The end-value of the animation.</param>
		/// <param name="duration">The duration of the animation.</param>
		/// <param name="callback">The function that is called after the animation has finished (can be null).</param>
		public static void Animate<TTarget, TValue>(
			TTarget target,
			string propertyName,
			Interpolator<TValue> interpolator,
			TValue startValue,
			TValue endValue,
			TimeSpan duration,
			Action callback)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (propertyName == null) throw new ArgumentNullException("propertyName");
			if (propertyName.Length == 0) throw new ArgumentException("propertyName cannot be an empty string", "propertyName");
			if (interpolator == null) throw new ArgumentNullException("interpolator");

			AnimateCore(
				GetPropertySetter<TTarget, TValue>(propertyName),
				target,
				interpolator,
				startValue,
				endValue,
				duration,
				0,
				callback);
		}

		/// <summary>Starts an animation.</summary>
		/// <typeparam name="TTarget">The type of object to animate a property of.</typeparam>
		/// <typeparam name="TValue">The type of value to animate.</typeparam>
		/// <param name="target">The object to animate a property of.</param>
		/// <param name="propertyName">The name of the property to animate.</param>
		/// <param name="interpolator">A function that calculates the intermediate values between startValue and endValue.</param>
		/// <param name="startValue">The start-value of the animation.</param>
		/// <param name="endValue">The end-value of the animation.</param>
		/// <param name="duration">The duration of the animation.</param>
		/// <param name="pulsationCount">The number of times to pulsate (= reverse the animation and start over).</param>
		/// <param name="callback">The function that is called after the animation has finished (can be null).</param>
		public static void Animate<TTarget, TValue>(
			TTarget target,
			string propertyName,
			Interpolator<TValue> interpolator,
			TValue startValue,
			TValue endValue,
			TimeSpan duration,
			int pulsationCount,
			Action callback)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (propertyName == null) throw new ArgumentNullException("propertyName");
			if (propertyName.Length == 0) throw new ArgumentException("propertyName cannot be an empty string", "propertyName");
			if (interpolator == null) throw new ArgumentNullException("interpolator");
			if (pulsationCount < 0) throw new ArgumentOutOfRangeException("pulsationCount", "pulsationCount cannot be negative.");

			AnimateCore(
				GetPropertySetter<TTarget, TValue>(propertyName),
				target,
				interpolator,
				startValue,
				endValue,
				duration,
				pulsationCount,
				callback);
		}

		#endregion

		private static void AnimateCore<TTarget, TValue>(
			PropertySetter<TTarget, TValue> propertySetter,
			TTarget target,
			Interpolator<TValue> interpolator,
			TValue startValue,
			TValue endValue,
			TimeSpan duration,
			int pulsationCount,
			Action callback)
		{
			Animation<TTarget, TValue>.Run(
				propertySetter,
				target,
				interpolator,
				startValue,
				endValue,
				duration,
				pulsationCount,
				callback);
		}

		private static PropertySetter<TTarget, TValue> GetPropertySetter<TTarget, TValue>(string propertyName)
		{
			PropertySetter<TTarget, TValue> lPropertySetter =
				PropertySetters<TTarget, TValue>.GetPropertySetter(propertyName);

			if (lPropertySetter == null)
				throw new ArgumentException(
					"{0} is not a settable property of the type {1}."
						.FormatInvariant(propertyName, typeof(TTarget).Name),
					"propertyName");

			return lPropertySetter;
		}

		private static Interpolator<TValue> GetInterpolator<TValue>()
		{
			Interpolator<TValue> lInterpolator = Interpolators.GetInterpolator<TValue>();

			if (lInterpolator == null)
				throw new NotSupportedException(
					"Values of type {0} can not be interpolated."
						.FormatInvariant(typeof(TValue).Name));

			return lInterpolator;
		}

		#region inner class Animation

		private class Animation<TTarget, TValue>
		{
			private Animation(
				PropertySetter<TTarget, TValue> propertySetter,
				TTarget target,
				Interpolator<TValue> interpolator,
				TValue startValue,
				TValue endValue,
				long totalTicks,
				long endTimeTicks,
				int pulsationCount,
				Action callback)
			{
				fPropertySetter = propertySetter;
				fTarget = target;
				fInterpolator = interpolator;
				fStartValue = startValue;

				Update(endValue, totalTicks, endTimeTicks, pulsationCount, callback);
			}

			private PropertySetter<TTarget, TValue> fPropertySetter;
			private TTarget fTarget;
			private Interpolator<TValue> fInterpolator;
			private TValue fStartValue, fEndValue;
			private long fTotalTicks, fEndTimeTicks;
			private int fPulsationCount;
			private Action fCallback;

			internal void Update(
				TValue endValue,
				long totalTicks,
				long endTimeTicks,
				int pulsationCount,
				Action callback)
			{
				fEndValue = endValue;
				fTotalTicks += totalTicks;

				if (fEndTimeTicks > 0)
					fEndTimeTicks += totalTicks;
				else fEndTimeTicks = endTimeTicks;

				fPulsationCount = pulsationCount;

				if (callback != null)
					fCallback = Delegate.Combine(fCallback, callback) as Action;
			}

			private void PerformStep()
			{
				long lTicksLeft = fEndTimeTicks - DateTime.UtcNow.Ticks;
				if (lTicksLeft > 0)
				{
					TValue lValue = fInterpolator(fStartValue, fEndValue, fTotalTicks - lTicksLeft, fTotalTicks);
					fPropertySetter(fTarget, lValue);
					fAnimationThread.Perform(PerformStep);
				}
				else
				{
					fPropertySetter(fTarget, fEndValue);

					if ((fPulsationCount -= 1) > 0)
					{
						TValue lSwapValue = fEndValue;
						fEndValue = fStartValue;
						fStartValue = lSwapValue;

						fEndTimeTicks = DateTime.UtcNow.Ticks + fTotalTicks;
						fAnimationThread.Perform(PerformStep);
					}
					else
					{
						fRunningAnimations.Remove(new RunningAnimationKey(fTarget, fPropertySetter));

						if (fCallback != null)
							fCallback();
					}
				}
			}

			#region running animations

			#region inner class RunningAnimationKey

			private struct RunningAnimationKey : IEquatable<RunningAnimationKey>
			{
				public TTarget Target;
				public PropertySetter<TTarget, TValue> PropertySetter;

				public RunningAnimationKey(TTarget target, PropertySetter<TTarget, TValue> propertySetter)
				{
					Target = target;
					PropertySetter = propertySetter;
				}

				public override int GetHashCode()
				{
					return Target.GetHashCode() ^ PropertySetter.GetHashCode();
				}

				public override bool Equals(object obj)
				{
					if (!(obj is RunningAnimationKey)) return false;
					return Equals((RunningAnimationKey)obj);
				}

				public static bool operator ==(RunningAnimationKey key1, RunningAnimationKey key2)
				{
					return key1.Equals(key2);
				}

				public static bool operator !=(RunningAnimationKey key1, RunningAnimationKey key2)
				{
					return !key1.Equals(key2);
				}

				#region IEquatable<RunningAnimationKey> Members

				public bool Equals(RunningAnimationKey other)
				{
					return object.Equals(Target, other.Target) && PropertySetter == other.PropertySetter;
				}

				#endregion
			}

			#endregion

			private static readonly Dictionary<RunningAnimationKey, Animation<TTarget, TValue>>
				fRunningAnimations = new Dictionary<RunningAnimationKey, Animation<TTarget, TValue>>();

			internal static void Run(
				PropertySetter<TTarget, TValue> propertySetter,
				TTarget target,
				Interpolator<TValue> interpolator,
				TValue startValue,
				TValue endValue,
				TimeSpan duration,
				int pulsationCount,
				Action callback)
			{
				long lTotalTicks = duration.Ticks;
				long lEndTimeTicks = DateTime.UtcNow.Ticks + lTotalTicks;

				fAnimationThread.Perform(delegate
				{
					if (lEndTimeTicks <= DateTime.UtcNow.Ticks)
					{
						propertySetter(target, endValue);
						return;
					}

					RunningAnimationKey lKey = new RunningAnimationKey(target, propertySetter);
					Animation<TTarget, TValue> lAnimation;
					if (fRunningAnimations.TryGetValue(lKey, out lAnimation))
						lAnimation.Update(endValue, lTotalTicks, lEndTimeTicks, pulsationCount, callback);
					else
					{
						lAnimation
							= new Animation<TTarget, TValue>(
								propertySetter,
								target,
								interpolator,
								startValue,
								endValue,
								lTotalTicks,
								lEndTimeTicks,
								pulsationCount,
								callback);
						fRunningAnimations[lKey] = lAnimation;
						lAnimation.PerformStep();
					}
				});
			}

			#endregion
		}

		#endregion
	}
}
