// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace TC.WinForms.Animation
{
	/// <summary>Static class that provides animation functionality.</summary>
	public static class Animator
	{
		#region initialization of the control for performing animation-steps

		private static Control _invokeControl;
		private static readonly AnimationThread _animationThread = new AnimationThread();

		/// <summary>Initializes the <see cref="T:Animator"/> class.</summary>
		/// <remarks>This method should be called on the UI-thread.</remarks>
		[SuppressMessage(
			"Microsoft.Performance",
			"CA1804:RemoveUnusedLocals",
			MessageId = "handle",
			Justification = "The handle of _invokeControl has to be created, so the getter of the Handle property has to be called.")]
		internal static void Initialize()
		{
			if (_invokeControl == null)
			{
				_invokeControl = new Control();
				IntPtr handle = _invokeControl.Handle;
			}
		}

		internal static void Perform(Action action)
		{
			_invokeControl.BeginInvoke(action, null);
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
			var propertySetter = PropertySetters<TTarget, TValue>.GetPropertySetter(propertyName);

			if (propertySetter == null)
				throw new ArgumentException(
					"{0} is not a settable property of the type {1}."
						.FormatInvariant(propertyName, typeof(TTarget).Name),
					"propertyName");

			return propertySetter;
		}

		private static Interpolator<TValue> GetInterpolator<TValue>()
		{
			var interpolator = Interpolators.GetInterpolator<TValue>();

			if (interpolator == null)
				throw new NotSupportedException(
					"Values of type {0} can not be interpolated."
						.FormatInvariant(typeof(TValue).Name));

			return interpolator;
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
				_propertySetter = propertySetter;
				_target = target;
				_interpolator = interpolator;
				_startValue = startValue;

				Update(endValue, totalTicks, endTimeTicks, pulsationCount, callback);
			}

			private readonly PropertySetter<TTarget, TValue> _propertySetter;
			private readonly TTarget _target;
			private readonly Interpolator<TValue> _interpolator;
			private TValue _startValue, _endValue;
			private long _totalTicks, _endTimeTicks;
			private int _pulsationCount;
			private Action _callback;

			internal void Update(
				TValue endValue,
				long totalTicks,
				long endTimeTicks,
				int pulsationCount,
				Action callback)
			{
				_endValue = endValue;
				_totalTicks += totalTicks;

				if (_endTimeTicks > 0)
					_endTimeTicks += totalTicks;
				else _endTimeTicks = endTimeTicks;

				_pulsationCount = pulsationCount;

				if (callback != null)
					_callback = Delegate.Combine(_callback, callback) as Action;
			}

			private void PerformStep()
			{
				long ticksLeft = _endTimeTicks - DateTime.UtcNow.Ticks;
				if (ticksLeft > 0)
				{
					TValue value = _interpolator(_startValue, _endValue, _totalTicks - ticksLeft, _totalTicks);
					_propertySetter(_target, value);
					_animationThread.Perform(PerformStep);
				}
				else
				{
					_propertySetter(_target, _endValue);

					if ((_pulsationCount -= 1) > 0)
					{
						TValue swapValue = _endValue;
						_endValue = _startValue;
						_startValue = swapValue;

						_endTimeTicks = DateTime.UtcNow.Ticks + _totalTicks;
						_animationThread.Perform(PerformStep);
					}
					else
					{
						_runningAnimations.Remove(new RunningAnimationKey(_target, _propertySetter));

						if (_callback != null)
							_callback();
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
				_runningAnimations = new Dictionary<RunningAnimationKey, Animation<TTarget, TValue>>();

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
				long totalTicks = duration.Ticks;
				long endTimeTicks = DateTime.UtcNow.Ticks + totalTicks;

				_animationThread.Perform(delegate
				{
					if (endTimeTicks <= DateTime.UtcNow.Ticks)
					{
						propertySetter(target, endValue);
						return;
					}

					RunningAnimationKey key = new RunningAnimationKey(target, propertySetter);
					Animation<TTarget, TValue> animation;
					if (_runningAnimations.TryGetValue(key, out animation))
						animation.Update(endValue, totalTicks, endTimeTicks, pulsationCount, callback);
					else
					{
						animation
							= new Animation<TTarget, TValue>(
								propertySetter,
								target,
								interpolator,
								startValue,
								endValue,
								totalTicks,
								endTimeTicks,
								pulsationCount,
								callback);
						_runningAnimations[key] = animation;
						animation.PerformStep();
					}
				});
			}

			#endregion
		}

		#endregion
	}
}
