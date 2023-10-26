using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GAS.GameplayTag.Editor
{
    [CreateAssetMenu(fileName = "GameplayTagsAsset", menuName = "GAS/GameplayTagsAsset")]
    public class GameplayTagsAsset : ScriptableObject
    {
        public List<GameplayTag> tags = new List<GameplayTag>();
        
        [HideInInspector]
        public GameplayTagNode _rootNode;

        public Dictionary<int, GameplayTag> tagMap = new Dictionary<int, GameplayTag>();

        public GameplayTagsAsset()
        {
            var rootTag = GameplayTag.Create("Root");
            rootTag.hashId = 0;
            tagMap.Add(rootTag.hashId, rootTag);
            _rootNode = new GameplayTagNode("Root", rootTag, null);
        }

        private GameplayTagNode GetNode(string name, GameplayTagNode parent)
        {
            var node = parent.children.Find(x => x.nodeName == name);
            return node;
        }

        public void AddTag(string tagName)
        {
            var name = "";
            foreach (var str in tagName.Split("."))
            {
                if (name == "")
                {
                    name = str;
                }
                else
                {
                    name = name + "." + str;
                }
                var newTag = GameplayTag.Create(name);
                if (!tagMap.ContainsKey(newTag.hashId))
                {
                    tagMap.Add(newTag.hashId, newTag);
                    tags.Add(newTag);
                }
            }
        }

        public void RemoveTag(int tagId)
        {
            var tag = tagMap[tagId];
            RemoveTag(tag.name);
        }

        public void RemoveTag(string tagName)
        {
            var removeTagList = new List<GameplayTag>();
            foreach (var tag in tags)
            {
                if (tag.name.Contains(tagName))
                {
                    removeTagList.Add(tag);
                }
            }

            foreach (var tag in removeTagList)
            {
                tags.Remove(tag);
                tagMap.Remove(tag.hashId);
            }
        }

        public void ReBuildNodes()
        {
            _rootNode.children.Clear();
            foreach (var tag in tags)
            {
                BuildNodeByTag(tag);
            }
        }
        
        public void BuildNodeByTag(GameplayTag tag)
        {
            var strs = tag.name.Split('.');
            var parentNode = _rootNode;
            var name = "";
            for (int i = 0; i < strs.Length; i++)
            {
                if (i == 0)
                {
                    name = strs[i];
                }
                else
                {
                    name = name + "." + strs[i];
                }

                var childNode = GetNode(strs[i], parentNode);
                if (childNode == null)
                {
                    var newTag = tagMap[name.GetHashCode()];
                    childNode = new GameplayTagNode(strs[i], newTag, parentNode);
                    parentNode.children.Add(childNode);
                }
                parentNode = childNode;
            }
        }
    }
}