namespace WpfApp1
{
    using Csv;
    using OxyPlot;
    using OxyPlot.Series;
    using RDotNet;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// https://webcache.googleusercontent.com/search?q=cache:9FdGvh1f58wJ:https://davetang.org/muse/2013/05/09/on-curve-fitting/+&amp;cd=1&amp;hl=en&amp;ct=clnk&amp;gl=uk
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<int, OxyColor> dictColors = new Dictionary<int, OxyColor>
                                                           {
                                                               { 1, OxyColors.Blue },
                                                               { 2, OxyColors.Violet },
                                                               { 3, OxyColors.Black },
                                                               { 4, OxyColors.RosyBrown }
                                                           };

        private REngine engine;

        //private int number;

        public event Action<int> numberEvent;

        public MainWindow()
        {
            this.InitializeComponent();
            int number = 2;

            foreach (var (x_, y_) in this.SelectData())
            {
                this.Values.Add(x_, y_);
            }

            this.Values.KeyValueChanged += (a, b) => this.Act(number);
            this.DataGrid.ItemsSource = this.Values;
            this.PlotView.Model = new PlotModel();
            this.engine = REngine.GetInstance();

            Act(number);

            this.numberEvent += num =>
                {
                    number = num;
                    var (x, y) = GetNumericVectors();
                    this.xBox.Text = string.Join(Environment.NewLine, SelectResult(x));
                    this.yBox.Text = string.Join(Environment.NewLine, SelectResult(y));
                    this.pBox.Text = string.Join(" ", engine.Evaluate("t.test(x,y)").AsList());
                    this.PlotPredictionAndData(number, x, y);
                };
        }

        public ObservablePairCollection<double, double> Values { get; } = new ObservablePairCollection<double, double>();

        private void Act(int number)
        {
            var (x, y) = GetNumericVectors();
            this.PlotPoints(this.engine, x, y);
            this.PlotPredictionAndData(number, x, y);
        }

        private void PlotPoints(REngine engine, NumericVector vectorx, NumericVector vectory)
        =>
            this.Dispatcher?.InvokeAsync(
                () =>
                    {
                        foreach (var se in this.PlotView.Model.Series.OfType<ScatterSeries>().ToArray())
                        {
                            this.PlotView.Model.Series.Remove(se);
                        }

                        var sSeries = new ScatterSeries
                        {
                            MarkerType = MarkerType.Diamond,
                            MarkerSize = 5,
                            MarkerStrokeThickness = 1,
                            MarkerFill = OxyColors.Blue
                        };

                        sSeries.Points.AddRange(
                            vectorx.Zip(vectory, (a, b) => (a, b))
                            .Select(ab =>
                                new ScatterPoint(ab.Item1, ab.Item2, 10, 10)));

                        this.PlotView.Model.Series.Add(sSeries);
                        this.PlotView.Model.InvalidatePlot(true);
                    });
        

        private void MyUpDownControl_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int number = (int)e.NewValue;

            if (number > 0 && number < 5)
            {
                this.numberEvent?.Invoke(number);
            }
            else
            {
                MessageBox.Show($"Number should be between {0} & {5}");
            }
        }

        private (NumericVector x, NumericVector y) GetNumericVectors()
        {
            var x = new NumericVector(this.engine, this.Values.Select(a => (double)a.Key));
            var y = new NumericVector(this.engine, this.Values.Select(a => (double)a.Value));
            this.engine.SetSymbol("x", x);
            this.engine.SetSymbol("y", y);
            return (x, y);
        }

        private void PlotPredictionAndData(int number, NumericVector x, NumericVector y)
        {
            var p = new RPredict.Model(this.engine).Predict(number, x, y);

            var series = new LineSeries();

            series.Color = this.dictColors[number];

            series.Points.AddRange(p.Select(ab => new DataPoint(ab.Item1, ab.Item2)));
            this.Dispatcher?.Invoke(
                () =>
                    {
                        this.PlotView.Model.Series.Add(series);
                        this.PlotView.InvalidatePlot();
                    });
        }

        private IEnumerable<(int, int)> SelectData()
        {
            var csv = File.ReadAllText("../../Data/Data.csv");
            foreach (var line in CsvReader.ReadFromText(csv))
            {
                yield return (int.Parse(line["x"]), int.Parse(line["y"]));
            }
        }

        private static IEnumerable<string> SelectResult(SymbolicExpression result)
        {
            var numericResult = result.AsNumeric();
            yield return $"Size of numeric vector: {numericResult.Length}";
            yield return $"First vector result as numeric: {numericResult.FirstOrDefault()}";
            var characterResult = result.AsCharacter();
            yield return $"Size of result as character vector: {characterResult.Length}";
            yield return $"First vector result as character: {characterResult.FirstOrDefault()}";
        }
    }
}

// static void a()
// {
// REngine.SetEnvironmentVariables();
// REngine engine = REngine.GetInstance();
// // REngine requires explicit initialization.
// // You can set some parameters.
// engine.Initialize();

// // .NET Framework array to R vector.
// NumericVector group1 = engine.CreateNumericVector(new double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 });
// engine.SetSymbol("group1", group1);
// // Direct parsing from R script.
// NumericVector group2 = engine.Evaluate("group2 <- c(29.89, 29.93, 29.72, 29.98, 30.02, 29.98)").AsNumeric();

// // Test difference of mean and get the P-value.
// GenericVector testResult = engine.Evaluate("t.test(group1, group2)").AsList();
// double p = testResult["p.value"].AsNumeric().First();

// Console.WriteLine("Group1: [{0}]", string.Join(", ", group1));
// Console.WriteLine("Group2: [{0}]", string.Join(", ", group2));
// Console.WriteLine("P-value = {0:0.000}", p);

// // you should always dispose of the REngine properly.
// // After disposing of the engine, you cannot reinitialize nor reuse it
// engine.Dispose();

// }