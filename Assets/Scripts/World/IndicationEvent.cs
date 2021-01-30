namespace WaifuTaxi
{
    public class IndicationEvent
    {
        public Indication indication;

        public bool pathWasRestarted;

        public IndicationEvent(Indication indication, bool pathWasRestarted)
        {
            this.indication = indication;
            this.pathWasRestarted = pathWasRestarted;
        }
    }
}