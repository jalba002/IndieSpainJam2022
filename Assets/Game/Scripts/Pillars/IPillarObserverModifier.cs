namespace CosmosDefender
{
    public interface IPillarObserverModifier
    {
        void OnPillarActivate(PillarObserver observer);
        void OnObserverInRange(PillarObserver observer);
        void OnObserverOutsideOfRange(PillarObserver observer);
    }
}