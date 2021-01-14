using System;
using System.Windows;

namespace Quan.ControlLibrary
{
    /// <summary>
    /// A base attached property to replace the vanilla WPF attached property
    /// </summary>
    /// <typeparam name="Parent">The parent class to be the attached property</typeparam>
    /// <typeparam name="Property">The type of this attached property</typeparam>
    /// where T : [baseTypeName] The type argument must be or derive from the specified base class
    public abstract class BaseAttachedProperty<Parent, Property>
        where Parent : new()
    {
        #region Public Events

        /// <summary>
        /// Fired when the value changes
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        /// <summary>
        /// Fired when the value changes,even when the value is same
        /// </summary>
        public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };

        #endregion

        #region Public Properties

        public static Parent Instansce { get; private set; } = new Parent();

        #endregion

        #region Attached Property Definitions

        /// <summary>
        /// The attach property for this class
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached(
                "Value",
                typeof(Property),
                typeof(BaseAttachedProperty<Parent, Property>),
                new UIPropertyMetadata(
                    default(Property),
                    new PropertyChangedCallback(OnValuePropertyChanged),
                    new CoerceValueCallback(OnValuePropertyUpdated)));



        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed
        /// </summary>
        /// <param name="d">The UI Element that had it's property changed</param>
        /// <param name="e">The arguments for the event</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Call the parent function
            (Instansce as BaseAttachedProperty<Parent, Property>)?.OnValueChanged(d, e);

            //Call event listners
            (Instansce as BaseAttachedProperty<Parent, Property>)?.ValueChanged(d, e);
        }

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed, even if it is the same value,or the value was set at first load
        /// </summary>
        /// <param name="d">The UI Element that had it's property changed</param>
        /// <param name="basevalue">The value</param>
        /// <returns></returns>
        private static object OnValuePropertyUpdated(DependencyObject d, object basevalue)
        {
            //Call the parent function
            (Instansce as BaseAttachedProperty<Parent, Property>)?.OnValueUpdated(d, basevalue);

            //Call event listners
            (Instansce as BaseAttachedProperty<Parent, Property>)?.ValueUpdated(d, basevalue);

            //Return the value
            return basevalue;
        }

        /// <summary>
        /// Gets the attached property
        /// </summary>
        /// <param name="ob">The element to get the property from</param>
        /// <returns></returns>
        public static Property GetValue(DependencyObject ob) => (Property)ob.GetValue(ValueProperty);

        /// <summary>
        /// Sets the attached property
        /// </summary>
        /// <param name="ob">The element to get the property from</param>
        /// <param name="value">The value to set the property</param>
        public static void SetValue(DependencyObject ob, Property value) => ob.SetValue(ValueProperty, value);

        #endregion

        #region Event Methods

        /// <summary>
        /// The method that is called when any attached property of this type is changed
        /// </summary>
        /// <param name="sender">The UI element that this property was changed for</param>
        /// <param name="e">The arguments for this event</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }


        /// <summary>
        /// The method that is called when any attached property of this type is changed,even if the value is same
        /// </summary>
        /// <param name="sender">The UI element that this property was changed for</param>
        /// <param name="value">The arguments for this event</param>
        public virtual void OnValueUpdated(DependencyObject sender, object value) { }

        #endregion
    }
}
