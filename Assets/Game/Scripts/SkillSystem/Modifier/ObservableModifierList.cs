using System;
using System.Collections.Generic;

namespace CosmosDefender
{
    [Serializable]
    public class ObservableModifierList<T, T1> where T : IModifier<T1>
    {
        public List<T> attributeModifiers { get; private set; }
        private Action<IReadOnlyList<T>> onListUpdated;

        public ObservableModifierList(Action<IReadOnlyList<T>> onListUpdated) => this.onListUpdated = onListUpdated;

        public void AddModifier(T attribute)
        {
            attributeModifiers.Add(attribute);
            if (attribute is ITemporalModifier temporalModifier)
            {
                CronoScheduler.Instance.ScheduleForTime(temporalModifier.Time, () => RemoveModifier(attribute));
            }
            onListUpdated(attributeModifiers);
        }

        public void AddModifiers(IReadOnlyList<T> attributes)
        {
            attributeModifiers.AddRange(attributes);
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