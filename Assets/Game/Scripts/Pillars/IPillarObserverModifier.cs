namespace CosmosDefender
{
    public interface IPillarObserverModifier
    {
        void OnPillarActivate(PillarObserver observer);
        void SetPillarEmpowerState(PillarObserver observer, bool newState);
        void OnObserverInRange(PillarObserver observer);
        void OnObserverOutsideOfRange(PillarObserver observer);
    }
}