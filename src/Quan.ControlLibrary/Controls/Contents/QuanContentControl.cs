using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Quan.ControlLibrary.Helpers.Boxes;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

/// <summary>
/// A ContentControl which use a transition to slide in the content.
/// </summary>
[TemplatePart(Name = "AfterLoadedStoryboard", Type = typeof(Storyboard))]
[TemplatePart(Name = "AfterLoadedReverseStoryboard", Type = typeof(Storyboard))]
public class QuanContentControl : ContentControl
{
    #region Private members

    private Storyboard _afterLoadedStoryboard;

    private Storyboard _afterLoadedReverseStoryboard;

    private bool _transitionLoaded;

    #endregion

    #region ReverseTransition

    public bool ReverseTransition
    {
        get => (bool)GetValue(ReverseTransitionProperty);
        set => SetValue(ReverseTransitionProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ReverseTransitionProperty
        = DependencyProperty.Register(
            nameof(ReverseTransition),
            typeof(bool),
            typeof(QuanContentControl),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

    #endregion

    #region TransitionsEnabled

    public bool TransitionsEnabled
    {
        get => (bool)GetValue(TransitionsEnabledProperty);
        set => SetValue(TransitionsEnabledProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty TransitionsEnabledProperty =
        DependencyProperty.Register(
            nameof(TransitionsEnabled),
            typeof(bool),
            typeof(QuanContentControl),
            new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #region OnlyLoadTransition

    public bool OnlyLoadTransition
    {
        get => (bool)GetValue(OnlyLoadTransitionProperty);
        set => SetValue(OnlyLoadTransitionProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty OnlyLoadTransitionProperty =
        DependencyProperty.Register(
            nameof(OnlyLoadTransition),
            typeof(bool),
            typeof(QuanContentControl),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

    #endregion

    #region TransitionStarted

    public event RoutedEventHandler TransitionStarted
    {
        add => AddHandler(TransitionStartedEvent, value);
        remove => RemoveHandler(TransitionStartedEvent, value);
    }

    public static readonly RoutedEvent TransitionStartedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(TransitionStarted),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(QuanContentControl));

    #endregion

    #region TransitionCompleted

    public event RoutedEventHandler TransitionCompleted
    {
        add => AddHandler(TransitionCompletedEvent, value);
        remove => RemoveHandler(TransitionCompletedEvent, value);
    }

    public static readonly RoutedEvent TransitionCompletedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(TransitionCompleted),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(QuanContentControl));

    #endregion

    #region IsTransitioning

    public bool IsTransitioning
    {
        get => (bool)GetValue(IsTransitioningProperty);
        protected set => SetValue(IsTransitioningPropertyKey, BooleanBoxes.Box(value));
    }

    private static readonly DependencyPropertyKey IsTransitioningPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(IsTransitioning),
            typeof(bool),
            typeof(QuanContentControl),
            new PropertyMetadata(BooleanBoxes.FalseBox));

    public static readonly DependencyProperty IsTransitioningProperty = IsTransitioningPropertyKey.DependencyProperty;

    #endregion

    #region Constructors

    public QuanContentControl()
    {
        DefaultStyleKey = typeof(QuanContentControl);

        Loaded += QuanContentControlLoaded;
        Unloaded += QuanContentControlUnloaded;
    }

    #endregion

    #region Overrides

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _afterLoadedStoryboard = GetTemplateChild("AfterLoadedStoryboard") as Storyboard;
        _afterLoadedReverseStoryboard = GetTemplateChild("AfterLoadedReverseStoryboard") as Storyboard;
    }

    #endregion

    #region Methods

    private void QuanContentControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (TransitionsEnabled && !_transitionLoaded)
        {
            if (!IsVisible)
            {
                VisualStateManager.GoToState(this, ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", false);
            }
            else
            {
                VisualStateManager.GoToState(this, ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
            }
        }
    }

    private void QuanContentControlUnloaded(object sender, RoutedEventArgs e)
    {
        if (TransitionsEnabled)
        {
            UnsetStoryboardEvents();
            if (_transitionLoaded)
            {
                VisualStateManager.GoToState(this, ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", false);
            }

            IsVisibleChanged -= QuanContentControlIsVisibleChanged;
        }
    }

    private void QuanContentControlLoaded(object sender, RoutedEventArgs e)
    {
        if (TransitionsEnabled)
        {
            if (!_transitionLoaded)
            {
                SetStoryboardEvents();
                _transitionLoaded = OnlyLoadTransition;
                VisualStateManager.GoToState(this, ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
            }

            IsVisibleChanged -= QuanContentControlIsVisibleChanged;
            IsVisibleChanged += QuanContentControlIsVisibleChanged;
        }
        else
        {
            if (GetTemplateChild("RootGrid") is Grid rootGrid)
            {
                rootGrid.Opacity = 1.0;
                var transform = ((System.Windows.Media.TranslateTransform)rootGrid.RenderTransform);
                if (transform.IsFrozen)
                {
                    var modifiedTransform = transform.Clone();
                    modifiedTransform.X = 0;
                    rootGrid.RenderTransform = modifiedTransform;
                }
                else
                {
                    transform.X = 0;
                }
            }
        }
    }

    public void Reload()
    {
        if (!TransitionsEnabled || _transitionLoaded)
        {
            return;
        }

        if (ReverseTransition)
        {
            VisualStateManager.GoToState(this, "BeforeLoaded", true);
            VisualStateManager.GoToState(this, "AfterUnLoadedReverse", true);
        }
        else
        {
            VisualStateManager.GoToState(this, "BeforeLoaded", true);
            VisualStateManager.GoToState(this, "AfterLoaded", true);
        }
    }

    private void AfterLoadedStoryboardCurrentTimeInvalidated(object sender, System.EventArgs e)
    {
        if (sender is Clock clock)
        {
            if (clock.CurrentState == ClockState.Active)
            {
                SetValue(IsTransitioningPropertyKey, BooleanBoxes.TrueBox);
                RaiseEvent(new RoutedEventArgs(TransitionStartedEvent));
            }
        }
    }

    private void AfterLoadedStoryboardCompleted(object sender, System.EventArgs e)
    {
        if (_transitionLoaded)
        {
            UnsetStoryboardEvents();
        }

        InvalidateVisual();
        SetValue(IsTransitioningPropertyKey, BooleanBoxes.FalseBox);
        RaiseEvent(new RoutedEventArgs(TransitionCompletedEvent));
    }

    private void SetStoryboardEvents()
    {
        if (_afterLoadedStoryboard != null)
        {
            _afterLoadedStoryboard.CurrentTimeInvalidated += AfterLoadedStoryboardCurrentTimeInvalidated;
            _afterLoadedStoryboard.Completed += AfterLoadedStoryboardCompleted;
        }

        if (_afterLoadedReverseStoryboard != null)
        {
            _afterLoadedReverseStoryboard.CurrentTimeInvalidated += AfterLoadedStoryboardCurrentTimeInvalidated;
            _afterLoadedReverseStoryboard.Completed += AfterLoadedStoryboardCompleted;
        }
    }

    private void UnsetStoryboardEvents()
    {
        if (_afterLoadedStoryboard != null)
        {
            _afterLoadedStoryboard.CurrentTimeInvalidated -= AfterLoadedStoryboardCurrentTimeInvalidated;
            _afterLoadedStoryboard.Completed -= AfterLoadedStoryboardCompleted;
        }

        if (_afterLoadedReverseStoryboard != null)
        {
            _afterLoadedReverseStoryboard.CurrentTimeInvalidated -= AfterLoadedStoryboardCurrentTimeInvalidated;
            _afterLoadedReverseStoryboard.Completed -= AfterLoadedStoryboardCompleted;
        }
    }

    #endregion
}