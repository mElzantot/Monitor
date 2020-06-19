using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class PredictedDuration
    {
        [ColumnName("Score")]
        public float Duration { get; set; }

        public double Rsquared { get; set; }

    }

    public class PredictedQuality
    {
        [ColumnName("Score")]
        public float Quality { get; set; }

        public double Rsquared { get; set; }

    }

}
