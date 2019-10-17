using System;
using System.Collections.Generic;
using System.Linq;

namespace RPredict
{
    using RDotNet;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// https://webcache.googleusercontent.com/search?q=cache:9FdGvh1f58wJ:https://davetang.org/muse/2013/05/09/on-curve-fitting/+&amp;cd=1&amp;hl=en&amp;ct=clnk&amp;gl=uk
    /// </summary>
    public class Model
    {
        private readonly REngine engine;

        public Model(REngine engine)
        {
            this.engine = engine;
        }

        public IEnumerable<(double, double)> Predict(int degree, NumericVector x, NumericVector y)
        {
            //using (var engine = REngine.GetInstance())
            //{
            //engine.Evaluate(
            //    ".libPaths('C:/Users/declan.taylor/OneDrive - ChartCo Ltd/Documents/R/win-library/3.5')");
            //engine.Evaluate("library(glue)");

            var xx = engine.Evaluate("xx <- seq(30,160, length=50)").AsNumeric();

            engine.Evaluate("dtf <- data.frame(x=xx)");

            var dataFrame = engine.Evaluate($"data.frame(predict(lm(y~poly(x,{degree},raw=TRUE)), dtf))").AsDataFrame();

            return dataFrame.GetRows().Select(r => Convert.ToDouble(r[0])).Zip(xx.Select(a => a), (a, b) => (b, a));
            //}
        }
    }
}