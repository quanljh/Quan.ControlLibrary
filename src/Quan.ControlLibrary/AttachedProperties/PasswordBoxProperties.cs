using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    /// <summary>
    /// The MonitorPassword attach property for a<see cref="PasswordBox"/> 
    /// </summary>
    public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                //Remove any previous evens
                passwordBox.PasswordChanged -= PasswordBoxOnPasswordChanged;

                //If the caller set MonitorPassword to true...
                if (e.NewValue is bool flag && flag)
                {
                    HasTextProperty.SetValue(passwordBox);
                    //start listening out for password changes
                    passwordBox.PasswordChanged += PasswordBoxOnPasswordChanged;
                }
            }
        }

        private void PasswordBoxOnPasswordChanged(object sender, RoutedEventArgs e)
        {
            HasTextProperty.SetValue((PasswordBox)sender);
        }
    }

    /// <summary>
    /// The HasText attach property for a<see cref="PasswordBox"/> 
    /// </summary>
    public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool>
    {
        public static void SetValue(DependencyObject sender)
        {
            SetValue(sender, ((PasswordBox)sender).SecurePassword.Length > 0);
        }
    }
}
