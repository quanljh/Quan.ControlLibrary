using System;
using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    public static partial class FloatingProxyFabric
    {
        private sealed class TextBoxFloatingProxy : IFloatingProxy
        {
            private readonly TextBox _textBox;

            /// <inheritdoc />
            public bool IsEmpty() => string.IsNullOrEmpty(_textBox.Text);

            /// <inheritdoc />
            public bool IsFocused() => _textBox.IsKeyboardFocused;

            public bool IsLoaded => _textBox.IsLoaded;

            /// <inheritdoc />
            public bool IsVisible => _textBox.IsVisible;

            /// <inheritdoc />
            public event EventHandler? ContentChanged;

            /// <inheritdoc />
            public event EventHandler? IsVisibleChanged;

            /// <inheritdoc />
            public event EventHandler? Loaded;

            /// <inheritdoc />
            public event EventHandler? FocusedChanged;

            public TextBoxFloatingProxy(TextBox textBox)
            {
                _textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
                _textBox.TextChanged += TextBox_OnTextChanged;
                _textBox.Loaded += TextBox_OnLoaded;
                _textBox.IsVisibleChanged += TextBox_IsVisibleChanged;
                _textBox.IsKeyboardFocusedChanged += TextBox_IsKeyboardFocusedChanged;
            }

            private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
                => ContentChanged?.Invoke(sender, EventArgs.Empty);

            private void TextBox_OnLoaded(object sender, RoutedEventArgs e)
                => Loaded?.Invoke(sender, EventArgs.Empty);

            private void TextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
                => IsVisibleChanged?.Invoke(sender, EventArgs.Empty);

            private void TextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
                => FocusedChanged?.Invoke(sender, EventArgs.Empty);

            /// <inheritdoc />
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
