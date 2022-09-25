namespace CosmosDefender
{
    public interface IPillarObserverModifier
    {
        void OnPillarActivate(PillarObserver observer);
        void SetPillarEmpowerState(PillarObserver observer, bool newState);
        void OnGoddessActive(PillarObserver observer);
        void OnGoddessUnactive(PillarObserver observer);
        void OnObserverInRange(PillarObserver observer);
        void OnObserverOutsideOfRange(PillarObserver observer);
    }
}