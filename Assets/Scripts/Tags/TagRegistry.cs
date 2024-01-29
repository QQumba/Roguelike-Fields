using System.Collections.Generic;
using UnityEngine;

namespace Tags
{
    /// <summary>
    /// Does not have a purpose yet, but the idea behind this thing is to check if the tag exists in a
    /// registry to catch missing tag errors.
    /// </summary>
    public class TagRegistry : MonoBehaviour
    {
        private List<string> _tags;

        private void Awake()
        {
            _tags = CellTags.GetTags();
        }
    }
}