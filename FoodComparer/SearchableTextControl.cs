using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FoodComparer
{
    /// <summary>
    /// Interaction logic for SearchableTextControl.xaml
    /// </summary>
    public partial class SearchableTextControl : Control
    {
        static SearchableTextControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchableTextControl),
                new FrameworkPropertyMetadata(typeof(SearchableTextControl)));
        }

        #region DependencyProperties

        /// <summary>
        /// Text sandbox which is used to get or set the value from a dependency property.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        // Real implementation about TextProperty which  registers a dependency property with 
        // the specified property name, property type, owner type, and property metadata. 
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SearchableTextControl),
            new UIPropertyMetadata(string.Empty,
              UpdateControlCallBack));

        /// <summary>
        /// HighlightBackground sandbox which is used to get or set the value from a dependency property,
        /// if it gets a value,it should be forced to bind to a Brushes type.
        /// </summary>
        public Brush HighlightBackground
        {
            get { return (Brush)GetValue(HighlightBackgroundProperty); }
            set { SetValue(HighlightBackgroundProperty, value); }
        }


        // Real implementation about HighlightBackgroundProperty which registers a dependency property 
        // with the specified property name, property type, owner type, and property metadata. 
        public static readonly DependencyProperty HighlightBackgroundProperty =
            DependencyProperty.Register("HighlightBackground", typeof(Brush), typeof(SearchableTextControl),
            new UIPropertyMetadata(Brushes.Yellow, UpdateControlCallBack));

        /// <summary>
        /// HighlightForeground sandbox which is used to get or set the value from a dependency property,
        /// if it gets a value,it should be forced to bind to a Brushes type.
        /// </summary>
        public Brush HighlightForeground
        {
            get { return (Brush)GetValue(HighlightForegroundProperty); }
            set { SetValue(HighlightForegroundProperty, value); }
        }


        // Real implementation about HighlightForegroundProperty which registers a dependency property with
        // the specified property name, property type, owner type, and property metadata. 
        public static readonly DependencyProperty HighlightForegroundProperty =
            DependencyProperty.Register("HighlightForeground", typeof(Brush), typeof(SearchableTextControl),
            new UIPropertyMetadata(Brushes.Black, UpdateControlCallBack));

        /// <summary>
        /// IsMatchCase sandbox which is used to get or set the value from a dependency property,
        /// if it gets a value,it should be forced to bind to a bool type.
        /// </summary>
        public bool IsMatchCase
        {
            get { return (bool)GetValue(IsMatchCaseProperty); }
            set { SetValue(IsMatchCaseProperty, value); }
        }

        // Real implementation about IsMatchCaseProperty which  registers a dependency property with
        // the specified property name, property type, owner type, and property metadata. 
        public static readonly DependencyProperty IsMatchCaseProperty =
            DependencyProperty.Register("IsMatchCase", typeof(bool), typeof(SearchableTextControl),
            new UIPropertyMetadata(true, UpdateControlCallBack));

        /// <summary>
        /// IsHighlight sandbox which is used to get or set the value from a dependency property,
        /// if it gets a value,it should be forced to bind to a bool type.
        /// </summary>
        public bool IsHighlight
        {
            get { return (bool)GetValue(IsHighlightProperty); }
            set { SetValue(IsHighlightProperty, value); }
        }

        // Real implementation about IsHighlightProperty which  registers a dependency property with
        // the specified property name, property type, owner type, and property metadata. 
        public static readonly DependencyProperty IsHighlightProperty =
            DependencyProperty.Register("IsHighlight", typeof(bool), typeof(SearchableTextControl),
            new UIPropertyMetadata(false, UpdateControlCallBack));

        /// <summary>
        /// SearchTextList sandbox which is used to get or set the value from a dependency property,
        /// if it gets a value,it should be forced to bind to a string type.
        /// </summary>
        public ObservableCollection<string> SearchTextList
        {
            get { return (ObservableCollection<string>)GetValue(SearchTextListProperty); }
            set { SetValue(SearchTextListProperty, value); }
        }

        /// <summary>
        /// Real implementation about SearchTextListProperty which registers a dependency property with
        /// the specified property name, property type, owner type, and property metadata. 
        /// </summary>
        public static readonly DependencyProperty SearchTextListProperty =
            DependencyProperty.Register("SearchTextList", typeof(ObservableCollection<string>), typeof(SearchableTextControl),
            new UIPropertyMetadata(UpdateControlCallBack));

        /// <summary>
        /// Create a call back function which is used to invalidate the rendering of the element, 
        /// and force a complete new layout pass.
        /// One such advanced scenario is if you are creating a PropertyChangedCallback for a 
        /// dependency property that is not  on a Freezable or FrameworkElement derived class that 
        /// still influences the layout when it changes.
        /// </summary>
        private static void UpdateControlCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchableTextControl obj = d as SearchableTextControl;
            obj.InvalidateVisual();
        }
        #endregion

        /// <summary>
        /// override the OnRender method which is used to search for the keyword and highlight
        /// it when the operation gets the result.
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            // Define a TextBlock to hold the search result.
            TextBlock displayTextBlock = this.Template.FindName("PART_TEXT", this) as TextBlock;

            if (string.IsNullOrEmpty(this.Text))
            {
                base.OnRender(drawingContext);

                return;
            }
            if (!this.IsHighlight)
            {
                displayTextBlock.Text = this.Text;
                base.OnRender(drawingContext);

                return;
            }

            displayTextBlock.Inlines.Clear();

            string compareText = this.IsMatchCase ? this.Text : this.Text.ToUpper();
            string displayText = this.Text;

            Run run = null;
            if (SearchTextList != null)
            {
                foreach (var text in SearchTextList)
                {
                    if (compareText.IndexOf(text) >= 0)
                    {
                        int position = compareText.IndexOf(text);
                        run = GenerateRun(displayText.Substring(0, position), false);

                        if (run != null)
                        {
                            displayTextBlock.Inlines.Add(run);
                        }

                        run = GenerateRun(displayText.Substring(position, text.Length), true);

                        if (run != null)
                        {
                            displayTextBlock.Inlines.Add(run);
                        }

                        compareText = compareText.Substring(position + text.Length);
                        displayText = displayText.Substring(position + text.Length);
                    }
                }
            }

            run = GenerateRun(displayText, false);

            if (run != null)
            {
                displayTextBlock.Inlines.Add(run);
            }

            base.OnRender(drawingContext);
        }

        /// <summary>
        /// Set inline-level flow content element intended to contain a run of formatted or unformatted 
        /// text into your background and foreground setting.
        /// </summary>
        private Run GenerateRun(string searchedString, bool isHighlight)
        {
            if (!string.IsNullOrEmpty(searchedString))
            {
                Run run = new Run(searchedString)
                {
                    Background = isHighlight ? this.HighlightBackground : this.Background,
                    Foreground = isHighlight ? this.HighlightForeground : this.Foreground,

                    // Set the source text with the style which is Bold.
                    FontWeight = isHighlight ? FontWeights.SemiBold : FontWeights.Normal,
                };
                return run;
            }
            return null;
        }
    }
}
