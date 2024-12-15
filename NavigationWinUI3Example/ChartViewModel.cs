using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationWinUI3Example
{
    internal static class ChartViewModel
    {
        public static PlotModel Model { get; private set; } = new PlotModel
        {
            Title = "Hello WinUI 3",
            PlotAreaBorderColor = OxyColors.Transparent,
            Axes =
        {
            new LinearAxis { Position = AxisPosition.Bottom },
            new LinearAxis { Position = AxisPosition.Left },
        },
                Series =
        {
            new LineSeries
            {
                Title = "LineSeries",
                MarkerType = MarkerType.Circle,
                Points =
                {
                    new DataPoint(0, 0),
                    new DataPoint(10, 18),
                    new DataPoint(20, 12),
                    new DataPoint(30, 8),
                    new DataPoint(40, 15),
                }
            }
        }
            };
    }
}
