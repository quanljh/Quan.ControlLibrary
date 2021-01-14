using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using Control = System.Windows.Controls.Control;

namespace Quan.ControlLibrary
{
    /// <summary>
    /// The IsBusy attached property for a anything that wants to flag if the control is busy
    /// </summary>
    public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Control control)
            {
                //Focus this control once loaded
                control.Loaded += (ss, ee) => control.Focus();
            }
        }
    }

    /// <summary>
    /// Focuses (keyboard focus) this element if true
    /// </summary>
    public class FocusProperty : BaseAttachedProperty<FocusProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have a control, return
            if (!(sender is Control control))
                return;

            if ((bool)e.NewValue)
                // Focus this control
                control.Focus();
        }
    }

    /// <summary>
    /// Focuses (keyboard focus) and selects all text in this element if true
    /// </summary>
    public class FocusAndSelectProperty : BaseAttachedProperty<FocusAndSelectProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have a control, return
            if (sender is TextBoxBase control)
            {
                if ((bool)e.NewValue)
                {
                    // Focus this control
                    control.Focus();

                    // Select all text
                    control.SelectAll();
                }
            }

            if (sender is PasswordBox password)
            {
                if ((bool)e.NewValue)
                {
                    // Focus this control
                    password.Focus();

                    // Select all text
                    password.SelectAll();
                }
            }
        }
    }

    /// <summary>
    /// Search and select text in the Textbox with highlight
    /// </summary>
    public class FindAndSelectProperty : BaseAttachedProperty<FindAndSelectProperty, string>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have a textBox, return
            if (!(sender is RichTextBox richTextBox))
                return;

            // Get the search text
            var searchText = e.NewValue as string;

            // Get the textRange from richTextbox
            var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);

            // Clear seleted text in textbox
            textRange.ClearAllProperties();

            // If search text is empty...
            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            var foundRange = FindTextInRange(textRange, searchText);

            if (foundRange == null)
                return;

            foundRange.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Lato Bold"));

            foundRange.ApplyPropertyValue(TextElement.ForegroundProperty, (SolidColorBrush)Application.Current.FindResource("WordOrangeBrush") ?? new SolidColorBrush(Colors.Orange));
        }

        public TextRange FindTextInRange(TextRange searchRange, string searchText)
        {
            int offset = searchRange.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
            if (offset < 0)
                return null;  // Not found

            var start = GetTextPositionAtOffset(searchRange.Start, offset);
            TextRange result = new TextRange(start, GetTextPositionAtOffset(start, searchText.Length));

            return result;
        }

        public TextPointer GetTextPositionAtOffset(TextPointer position, int characterCount)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    int count = position.GetTextRunLength(LogicalDirection.Forward);
                    if (characterCount <= count)
                    {
                        return position.GetPositionAtOffset(characterCount);
                    }

                    characterCount -= count;
                }

                TextPointer nextContextPosition = position.GetNextContextPosition(LogicalDirection.Forward);
                if (nextContextPosition == null)
                    return position;

                position = nextContextPosition;
            }

            return position;
        }
    }


    public class AutoAdjustTextProperty : BaseAttachedProperty<AutoAdjustTextProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            base.OnValueChanged(sender, e);

            // If we don't have a textBox, return
            if (!(sender is RichTextBox richTextBox))
                return;

            richTextBox.Loaded -= RichTextBoxOnLoaded;
            richTextBox.Loaded += RichTextBoxOnLoaded;
        }

        private void RichTextBoxOnLoaded(object sender, RoutedEventArgs e)
        {
            // If we don't have a textBox, return
            if (!(sender is RichTextBox richTextBox))
                return;

            richTextBox.Width = richTextBox.Document.GetFormattedText(richTextBox).WidthIncludingTrailingWhitespace + 10;
        }
    }


    public static class FlowDocumentExtensions
    {
        private static IEnumerable<TextElement> GetRunsAndParagraphs(FlowDocument doc)
        {
            for (TextPointer position = doc.ContentStart;
              position != null && position.CompareTo(doc.ContentEnd) <= 0;
              position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
                {
                    if (position.Parent is Run run)
                    {
                        yield return run;
                    }
                    else
                    {
                        if (position.Parent is Paragraph para)
                        {
                            yield return para;
                        }
                    }
                }
            }
        }

        public static FormattedText GetFormattedText(this FlowDocument doc, RichTextBox richTextBox)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var output = new FormattedText(
              GetText(doc),
              CultureInfo.CurrentCulture,
              doc.FlowDirection,
              new Typeface(doc.FontFamily, doc.FontStyle, doc.FontWeight, doc.FontStretch),
              doc.FontSize,
              doc.Foreground, VisualTreeHelper.GetDpi(richTextBox).PixelsPerDip);

            int offset = 0;

            foreach (TextElement el in GetRunsAndParagraphs(doc))
            {
                Run run = el as Run;

                if (run != null)
                {
                    int count = run.Text.Length;

                    output.SetFontFamily(run.FontFamily, offset, count);
                    output.SetFontStyle(run.FontStyle, offset, count);
                    output.SetFontWeight(run.FontWeight, offset, count);
                    output.SetFontSize(run.FontSize, offset, count);
                    output.SetForegroundBrush(run.Foreground, offset, count);
                    output.SetFontStretch(run.FontStretch, offset, count);
                    output.SetTextDecorations(run.TextDecorations, offset, count);

                    offset += count;
                }
                else
                {
                    offset += Environment.NewLine.Length;
                }
            }

            return output;
        }

        private static string GetText(FlowDocument doc)
        {
            StringBuilder sb = new StringBuilder();

            foreach (TextElement el in GetRunsAndParagraphs(doc))
            {
                Run run = el as Run;
                sb.Append(run == null ? Environment.NewLine : run.Text);
            }
            return sb.ToString();
        }
    }
}
