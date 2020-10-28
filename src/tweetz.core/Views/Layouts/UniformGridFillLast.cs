using System;
using System.Windows;
using System.Windows.Controls;

namespace tweetz.core.Views.Layouts
{
    public class UniformGridFillLast : Panel
    {
        public int FirstColumn
        {
            get { return (int)GetValue(FirstColumnProperty); }
            set { SetValue(FirstColumnProperty, value); }
        }

        public static readonly DependencyProperty FirstColumnProperty =
                DependencyProperty.Register(
                        "FirstColumn",
                        typeof(int),
                        typeof(UniformGridFillLast),
                        new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(ValidateFirstColumn));

        private static bool ValidateFirstColumn(object o)
        {
            return (int)o >= 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Critical Bug", "S4275:Getters and setters should access the expected fields",
            Justification = "https://community.sonarsource.com/t/c-dependencyproperty-flagged-by-s4275-getters-and-setters-should-access-the-expected-fields/26739")]
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public static readonly DependencyProperty ColumnsProperty =
                DependencyProperty.Register(
                        "Columns",
                        typeof(int),
                        typeof(UniformGridFillLast),
                        new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(ValidateColumns));

        private static bool ValidateColumns(object o)
        {
            return (int)o >= 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Critical Bug", "S4275:Getters and setters should access the expected fields",
            Justification = "https://community.sonarsource.com/t/c-dependencyproperty-flagged-by-s4275-getters-and-setters-should-access-the-expected-fields/26739")]
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public static readonly DependencyProperty RowsProperty =
                DependencyProperty.Register(
                        "Rows",
                        typeof(int),
                        typeof(UniformGridFillLast),
                        new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(ValidateRows));

        private static bool ValidateRows(object o)
        {
            return (int)o >= 0;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            UpdateComputedValues();

            Size childConstraint = new Size(availableSize.Width / _columns, availableSize.Height / _rows);
            double maxChildDesiredWidth = 0.0;
            double maxChildDesiredHeight = 0.0;

            //  Measure each child, keeping track of maximum desired width and height.
            for (int i = 0, count = InternalChildren.Count; i < count; ++i)
            {
                UIElement child = InternalChildren[i];

                // Measure the child.
                child.Measure(childConstraint);
                Size childDesiredSize = child.DesiredSize;

                if (maxChildDesiredWidth < childDesiredSize.Width)
                {
                    maxChildDesiredWidth = childDesiredSize.Width;
                }

                if (maxChildDesiredHeight < childDesiredSize.Height)
                {
                    maxChildDesiredHeight = childDesiredSize.Height;
                }
            }

            return new Size(maxChildDesiredWidth * _columns, maxChildDesiredHeight * _rows);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect childBounds = new Rect(0, 0, finalSize.Width / _columns, finalSize.Height / _rows);
            childBounds.X += childBounds.Width * FirstColumn;

            var count = InternalChildren.Count;
            var last = count - 1;

            double xStep = childBounds.Width;
            double xBound = finalSize.Width - 1.0;

            for (var i = 0; i < count; ++i)
            {
                if (i == last)
                {
                    childBounds.Width = finalSize.Width - childBounds.X;
                }

                var child = InternalChildren[i];
                child.Arrange(childBounds);

                if (child.Visibility == Visibility.Visible)
                {
                    childBounds.X += xStep;
                    if (childBounds.X >= xBound)
                    {
                        childBounds.Y += childBounds.Height;
                        childBounds.X = 0;
                    }
                }
            }

            return finalSize;
        }

        private void UpdateComputedValues()
        {
            _columns = Columns;
            _rows = Rows;

            if (FirstColumn >= _columns)
            {
                FirstColumn = 0;
            }

            if ((_rows == 0) || (_columns == 0))
            {
                int nonCollapsedCount = 0;

                for (int i = 0, count = InternalChildren.Count; i < count; ++i)
                {
                    UIElement child = InternalChildren[i];
                    if (child.Visibility != Visibility.Collapsed)
                    {
                        nonCollapsedCount++;
                    }
                }

                if (nonCollapsedCount == 0)
                {
                    nonCollapsedCount = 1;
                }

                if (_rows == 0)
                {
                    if (_columns > 0)
                    {
                        _rows = (nonCollapsedCount + FirstColumn + (_columns - 1)) / _columns;
                    }
                    else
                    {
                        _rows = (int)Math.Sqrt(nonCollapsedCount);
                        if ((_rows * _rows) < nonCollapsedCount)
                        {
                            _rows++;
                        }
                        _columns = _rows;
                    }
                }
                else if (_columns == 0)
                {
                    _columns = (nonCollapsedCount + (_rows - 1)) / _rows;
                }
            }
        }

        private int _rows;
        private int _columns;
    }
}