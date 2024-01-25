using System.Collections.Generic;
using System.Drawing;

namespace Models
{
    public class UiState
    {
        public double Scale  = 1;
        public double DotPaintRadius  = 2;

        // Readonly properties initialized in the constructor
        //public Offset ScaleCenter { get; } = new Offset(0, 0);
        //public Offset PanCenter { get; } = new Offset(0, 0);

        public int MaxTrailAge  = 200;
        public bool Running  = true;
        public bool ShowConnection  = true;
        public Size Size = new Size(500, 500);
        public bool ShowPositionHistory  = true;
        public int ParticleFindingInterval  = 10;

        // Constructors can be added if needed. This class uses the default constructor.
        public List<HistoryLine> HistoryLines { get; set; } = new List<HistoryLine>();
        public float TimeScale { get; set; } = 1;
        public double ScaledTime { get; set; } = 0;
        public bool RunningInBackground { get; set; }
    }
}
