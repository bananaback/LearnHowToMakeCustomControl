using LearnHowToMakeCustomControl.Converters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LearnHowToMakeCustomControl.Components
{
    public class BudgetCircleControl : Control
    {
        private Grid customControlGrid;
        private Path myArc;
        static BudgetCircleControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BudgetCircleControl), new FrameworkPropertyMetadata(typeof(BudgetCircleControl)));
        }

        public BudgetCircleControl()
        {
            InitializeBindings();
        }

        private void InitializeBindings()
        {


        }

        public static readonly DependencyProperty ArcThicknessProperty =
            DependencyProperty.Register("ArcThickness", typeof(double), typeof(BudgetCircleControl),
                new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double ArcThickness
        {
            get => (double)(GetValue(ArcThicknessProperty));
            set => SetValue(ArcThicknessProperty, value);
        }

        public static readonly DependencyProperty CenterXProperty =
        DependencyProperty.Register("CenterX", typeof(double), typeof(BudgetCircleControl),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double CenterX
        {
            get => (double)GetValue(CenterXProperty) + ArcThickness / 2;
            set => SetValue(CenterXProperty, value + ArcThickness / 2);
        }

        public static readonly DependencyProperty CenterYProperty =
        DependencyProperty.Register("CenterY", typeof(double), typeof(BudgetCircleControl),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double CenterY
        {
            get => (double)GetValue(CenterYProperty) + ArcThickness / 2;
            set => SetValue(CenterYProperty, value + ArcThickness / 2);
        }

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(BudgetCircleControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }


        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(BudgetCircleControl),
                new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Radius
        {
            get => (double)(GetValue(RadiusProperty));
            set => SetValue(RadiusProperty, value);
        }

        public static readonly DependencyProperty LargeAngleProperty =
            DependencyProperty.Register("LargeAngle", typeof(bool), typeof(BudgetCircleControl),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool LargeAngle
        {
            get => (bool)(GetValue(LargeAngleProperty));
            set => SetValue(LargeAngleProperty, value);
        }

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(double), typeof(BudgetCircleControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double CurrentValue
        {
            get => (double)(GetValue(CurrentValueProperty));
            set => SetValue(CurrentValueProperty, value);
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(BudgetCircleControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double MaxValue
        {
            get => (double)(GetValue(MaxValueProperty));
            set => SetValue(MaxValueProperty, value);
        }

        public static readonly DependencyProperty StartPointProperty =
        DependencyProperty.Register("StartPoint", typeof(Point), typeof(BudgetCircleControl),
            new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public Point StartPoint
        {
            get => (Point)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }

        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point), typeof(BudgetCircleControl),
                new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public Point EndPoint
        {
            get => (Point)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }

        public static readonly DependencyProperty RadiusSizeProperty =
            DependencyProperty.Register("RadiusSize", typeof(Size), typeof(BudgetCircleControl),
                new FrameworkPropertyMetadata(new Size(10, 10), FrameworkPropertyMetadataOptions.AffectsRender));

        public Size RadiusSize
        {
            get => (Size)GetValue(RadiusSizeProperty);
            set => SetValue(RadiusSizeProperty, value);
        }

        public static readonly DependencyProperty PercentageValueProperty =
        DependencyProperty.Register("PercentageValue", typeof(string), typeof(BudgetCircleControl),
            new FrameworkPropertyMetadata("0%"));

        public string PercentageValue
        {
            get => (string)GetValue(PercentageValueProperty);
            set => SetValue(PercentageValueProperty, value);
        }

        public bool IsMaxValueReached => CurrentValue >= MaxValue;


        private Point CalculatePointForValue(double value)
        {
            double percentage = (value / MaxValue) * 360;

            if (percentage == 360)
            {
                percentage = 359.99;
            }

            double angle = (percentage + 90) * (Math.PI / 180); // Convert to radians

            double x = CenterX + Radius * Math.Cos(-angle);
            double y = CenterY + Radius * Math.Sin(-angle);
            return new Point(x, y);
        }

        // Calculating start and end points based on provided properties
        private void CalculatePoints()
        {
            StartPoint = CalculatePointForValue(0);
            EndPoint = CalculatePointForValue(CurrentValue);
            RadiusSize = new Size(Radius, Radius);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == CurrentValueProperty || e.Property == MaxValueProperty ||
                e.Property == CenterXProperty || e.Property == CenterYProperty ||
                e.Property == RadiusProperty || e.Property == LargeAngleProperty)
            {
                CalculatePoints();
                LargeAngle = ((CurrentValue / MaxValue) * 100 > 50);
                PercentageValue = ((int)((CurrentValue / MaxValue) * 100)).ToString() + "%";
            }
            UpdateArcSegment();

            base.OnPropertyChanged(e);
        }

        public override void OnApplyTemplate()
        {
            customControlGrid = Template.FindName("PART_CustomControlGrid", this) as Grid;
            myArc = Template.FindName("MyArc", this) as Path;

            var radiusToSizeConverter = new RadiusToSizeConverter();

            var gridWidthBinding = new MultiBinding();
            gridWidthBinding.Converter = radiusToSizeConverter;
            gridWidthBinding.Bindings.Add(new Binding
            {
                Path = new PropertyPath(nameof(Radius)),
                Source = this,
            });
            gridWidthBinding.Bindings.Add(new Binding
            {
                Path = new PropertyPath(nameof(ArcThickness)),
                Source = this,
            });

            var gridHeightBinding = new MultiBinding();
            gridHeightBinding.Converter = radiusToSizeConverter;
            gridHeightBinding.Bindings.Add(new Binding
            {
                Path = new PropertyPath(nameof(Radius)),
                Source = this,
            });
            gridHeightBinding.Bindings.Add(new Binding
            {
                Path = new PropertyPath(nameof(ArcThickness)),
                Source = this,
            });

            customControlGrid.SetBinding(WidthProperty, gridWidthBinding);
            customControlGrid.SetBinding(HeightProperty, gridHeightBinding);

            var arcStrokeBinding = new Binding
            {
                Path = new PropertyPath(nameof(ArcThickness)),
                Source = this,
            };

            myArc.SetBinding(Path.StrokeThicknessProperty, arcStrokeBinding);

            UpdateArcSegment(); // Create or update the ArcSegment




            CalculatePoints();
            LargeAngle = ((CurrentValue / MaxValue) * 100 > 50);
            PercentageValue = ((int)((CurrentValue / MaxValue) * 100)).ToString() + "%";
            base.OnApplyTemplate();
        }

        private void UpdateArcSegment()
        {
            var myArcPath = GetTemplateChild("MyArc") as Path;

            if (myArcPath != null)
            {
                // Create the PathGeometry, PathFigure, and ArcSegment
                PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure();
                ArcSegment arcSegment = new ArcSegment();

                // Set the properties of the ArcSegment
                arcSegment.Point = EndPoint; // Use your EndPoint property here
                arcSegment.Size = RadiusSize; // Use your RadiusSize property here
                arcSegment.SweepDirection = SweepDirection.Counterclockwise; // Or your desired SweepDirection
                arcSegment.IsLargeArc = LargeAngle; // Use your LargeAngle property here

                // Set the StartPoint in PathFigure (should be set based on your logic)
                pathFigure.StartPoint = StartPoint; // Use your StartPoint property here

                // Add the ArcSegment to the PathFigure
                pathFigure.Segments.Add(arcSegment);

                // Add the PathFigure to the PathGeometry
                pathGeometry.Figures.Add(pathFigure);

                // Apply the created PathGeometry to the MyArc Path
                myArcPath.Data = pathGeometry;
            }
        }

    }
}
