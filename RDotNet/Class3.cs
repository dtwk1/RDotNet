using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDotNet
{
    class Class3
    {
        public static void Method()
        {
            var Engine = REngine.GetInstance();
            var result = Engine.Evaluate(
                @"setNames(data.frame(
                matrix(c(1,2,3,4,5,6),nrow=3,ncol=2)),
                c(""a"",""b""))");
            if (result.IsDataFrame()) {
                var data = result.AsDataFrame();
                int numCol = data.ColumnCount;
                for (int colIdx = 0; colIdx < numCol; colIdx++) {
                    var column = data[colIdx].AsCharacter();
                    int numRow = column.Length;
                    for (int rowIdx = 0; rowIdx < numRow; rowIdx++) {
                        Console.WriteLine(column[rowIdx]);
                    }
                }
            }

        }
    }
}
