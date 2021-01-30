namespace WaifuTaxi
{
    public class IndicationEvent
    {
        public Indication indication;

        public Indication prevIndication;

        public bool pathWasRestarted;

        public IndicationEvent(Indication indication, Indication prevIndication, bool pathWasRestarted)
        {
            this.indication = indication;
            this.prevIndication = prevIndication;
            this.pathWasRestarted = pathWasRestarted;
        }
    }
}