using System;
using System.Collections.Generic;

namespace CosmosDefender
{
    [Serializable]
    public class ObservableModifierList<T, T1, T2> where T : BaseModifier<T1, T2> where T1 : IModifier<T2>
    {
        public List<T> attributeModifiers { get; private set; } = new List<T>();
        private Action<IReadOnlyList<T>> onListUpdated;

        public ObservableModifierList(Action<IReadOnlyList<T>> onListUpdated) => this.onListUpdated = onListUpdated;

        public void ForceUpdate() => onListUpdated(attributeModifiers);

        public void AddModifier(T attribute, bool updateList = true)
        {
            attributeModifiers.Add(attribute);
            if (attribute is ITemporalModifier temporalModifier)
            {
                UnityEngine.Debug.Log(attribute.name + " buff for " + temporalModifier.Time + " seconds");
                CronoScheduler.Instance.ScheduleForTime(temporalModifier.Time, () => { RemoveModifier(attribute); UnityEngine.Debug.Log(attribute.name + " buff expired"); });

            }

            if (updateList)
                onListUpdated(attributeModifiers);
        }

        public void AddModifiers(IReadOnlyList<T> attributes)
        {
            foreach (var attribute in attributes)
                AddModifier(attribute, false);

            onListUpdated(attributeModifiers);
        }

        public void RemoveModifier(T attribute)
        {
            attributeModifiers.Remove(attribute);
            onListUpdated(attributeModifiers);
        }

        public void RemoveAllModifiers()
        {
            attributeModifiers.Clear();
            onListUpdated(attributeModifiers);
        }
    }
}