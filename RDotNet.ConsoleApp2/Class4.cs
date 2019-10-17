using System;
using System.Linq;

namespace RDotNet
{
    /// <summary>
    /// https://github.com/lrasmus/RDotNetTestApp/blob/master/RDotNetTestApp/Program.cs
    /// </summary>
    internal class Class4
    {
        public static void Method()
        {
            //REngine.SetEnvironmentVariables();
            using (var engine = REngine.GetInstance())
            {
                engine.Evaluate(".libPaths('C:/Users/declan.taylor/OneDrive - ChartCo Ltd/Documents/R/win-library/3.5')");
                engine.Evaluate("library(glue)");
                var x = engine.Evaluate(" x <- c(32,64,96,118,126,144,152.5,158)");
                var y = engine.Evaluate(" y <- c(99.5,104.8,108.5,100,86,64,35.3,15)");
                engine.Evaluate("xx <- seq(30,160, length=50)");

                var fit = engine.Evaluate(" fit <-  lm(y~poly(x,2,raw=TRUE)");
                var lines = engine.Evaluate("");

                //var predict = engine.Evaluate("predict <- predict(lm(y~poly(x,2,raw=TRUE), data.frame(x=xx))");
                var dfp = engine.Evaluate("dtf <-  data.frame(x=xx)").AsDataFrame();

                var dfp9 = engine.Evaluate("dtf <-  data.frame(x=xx)").AsDataFrame();

                var dfpd9 = engine.Evaluate("data.frame(predict(lm(y~x), data.frame(x=xx)))").AsDataFrame();

                //                engine.Evaluate(" fit  <- lm(y~x)
                //#second degree
                //                fit2 <- lm(y~poly(x,2,raw=TRUE))
                //#third degree
                //                fit3 <- lm(y~poly(x,3,raw=TRUE))
                //#fourth degree
                //                fit4 <- lm(y~poly(x,4,raw=TRUE))");

                // #we will make y the response variable and x the predictor
                // #the response variable is usually on the y-axis

                var result = engine.Evaluate("a = 10");
                PrintResult(result);
                result = engine.Evaluate("a");
                PrintResult(result);
            }
        }

        //

        private static void PrintResult(SymbolicExpression result)
        {
            var numericResult = result.AsNumeric();
            Console.WriteLine("Size of numeric vector: {0}", numericResult.Length);
            Console.WriteLine("First vector result as numeric: {0}", numericResult.FirstOrDefault());
            var characterResult = result.AsCharacter();
            Console.WriteLine("Size of result as character vector: {0}", characterResult.Length);
            Console.WriteLine("First vector result as character: {0}", characterResult.FirstOrDefault());
            Console.WriteLine("");
        }
    }
}