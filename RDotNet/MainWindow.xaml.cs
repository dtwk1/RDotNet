using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RDotNet
{
    using System.IO;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            a();
           
        }
        static void a()
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            // REngine requires explicit initialization.
            // You can set some parameters.
            engine.Initialize();

            // .NET Framework array to R vector.
            NumericVector group1 = engine.CreateNumericVector(new double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 });
            engine.SetSymbol("group1", group1);
            // Direct parsing from R script.
            NumericVector group2 = engine.Evaluate("group2 <- c(29.89, 29.93, 29.72, 29.98, 30.02, 29.98)").AsNumeric();

            // Test difference of mean and get the P-value.
            GenericVector testResult = engine.Evaluate("t.test(group1, group2)").AsList();
            double p = testResult["p.value"].AsNumeric().First();

            Console.WriteLine("Group1: [{0}]", string.Join(", ", group1));
            Console.WriteLine("Group2: [{0}]", string.Join(", ", group2));
            Console.WriteLine("P-value = {0:0.000}", p);

            // you should always dispose of the REngine properly.
            // After disposing of the engine, you cannot reinitialize nor reuse it
            engine.Dispose();

        }
        //public static void SetupPath(string Rversion = "R-3.0.0" )
        //{
        //    var oldPath = System.Environment.GetEnvironmentVariable("PATH");
        //    var rPath = System.Environment.Is64BitProcess ? 
        //                    string.Format(@"C:\Program Files\R\{0}\bin\x64", Rversion) :
        //                    string.Format(@"C:\Program Files\R\{0}\bin\i386",Rversion);

        //    if (!Directory.Exists(rPath))
        //        throw new DirectoryNotFoundException(
        //            string.Format(" R.dll not found in : {0}", rPath));
        //    var newPath = string.Format("{0}{1}{2}", rPath, System.IO.Path.PathSeparator, oldPath);
        //    System.Environment.SetEnvironmentVariable("PATH", newPath);
        //}

    }
}
