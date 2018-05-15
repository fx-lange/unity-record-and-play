using UnityEngine;

namespace RecordForTimeline
{
    public abstract class DataListener : MonoBehaviour
    {
        public abstract void ProcessData(DataFrame data);
    }
}
