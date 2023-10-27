using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GAS.GameplayTag
{
    [Serializable]
    public class TagNameLookup
    {
        public List<string> tagNames = new List<string>(){"root"};

        public byte AddTagName(string tagName)
        {
            var idx = tagNames.IndexOf(tagName);
            if (idx == -1)
            {
                tagNames.Add(tagName);
                return (byte)(tagNames.Count - 1);
            }

            return (byte)idx;
        }

        public string GetTagName(byte idx)
        {
            return tagNames[idx];
        }

        public byte GetTagIndex(string tagName)
        {
            var idx = tagNames.IndexOf(tagName);
            if (idx == -1)
            {
                return 0;
            }

            return (byte)idx;
        }
    }

    [CreateAssetMenu(fileName = "IndexTagAsset", menuName = "GAS/GameplayTag/IndexTagAsset")]
    public class IndexTagAsset : ScriptableObject
    {
        public List<IndexTag> indexTags = new List<IndexTag>();
        public List<TagNameLookup> tagNameLookup = new List<TagNameLookup>();

        public IndexTagAsset()
        {
        }

        public void AddIndexTag(string tagName)
        {
            if (GetIndexTag(tagName) == null)
            {
                AddTagNameToLookup(tagName);
                if (TagNameToLookupId(tagName, out var lookupId))
                {
                    var indexTag = new IndexTag()
                    {
                        id = lookupId
                    };
                    indexTags.Add(indexTag);
                }
            }
        }
        
        private void AddTagNameToLookup(string tagName)
        {
            var idxNames = tagName.Split(".");
            for (int i = 0; i < idxNames.Length; i++)
            {
                var idxName = idxNames[i];
                var lookup = tagNameLookup[i];
                lookup.AddTagName(idxName);
            }
        }

        public IndexTag GetIndexTag(string tagName)
        {
            if (TagNameToLookupId(tagName, out var lookupId))
            {
                return indexTags.Find(x => x.id == lookupId);
            }
            return null;
        }

        public bool TagNameToLookupId(string tagName, out int lookupId)
        {
            var idxNames = tagName.Split(".");
            lookupId = -1;
            var missLookup = false;
            var bytes = new byte[IndexTag.MaxTagLength];
            for (int i = idxNames.Length - 1; i >= 0; i--)
            {
                var idxName = idxNames[i];
                var lookup = tagNameLookup[i];
                var idx = lookup.tagNames.IndexOf(idxName);
                if (idx == -1)
                {
                    missLookup = true;
                    break;
                }
                else
                {
                    bytes[i] = (byte)idx;
                }
            }
            
            if (missLookup)
            {
                return false;
            }
            
            lookupId = BitConverter.ToInt32(bytes);
            return true;
        }
    }
}