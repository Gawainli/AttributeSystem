using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

namespace GAS.GameplayTag.Editor
{
    [Serializable]
    public struct GameplayTag
    {
        public string name;
        public int hashId;

        public static GameplayTag Create(string name)
        {
            var tag = new GameplayTag
            {
                name = name,
                hashId = name.GetHashCode()
            };
            return tag;
        }
        
        public bool IsParentOf(GameplayTag other)
        {
            var otherName = other.name;
            if (otherName.Length <= name.Length)
            {
                return false;
            }
            if (otherName.Substring(0, name.Length) == name)
            {
                return true;
            }
            return false;
        }
        
        public bool IsDescendantOf(GameplayTag other)
        {
            var otherName = other.name;
            if (name.Length <= otherName.Length)
            {
                return false;
            }
            if (name.Substring(0, otherName.Length) == otherName)
            {
                return true;
            }
            return false;
        }

        public static bool operator ==(GameplayTag a, GameplayTag b)
        {
            return a.hashId == b.hashId;
        }

        public static bool operator !=(GameplayTag a, GameplayTag b)
        {
            return !(a == b);
        }
        
    }

    [Serializable]
    public class GameplayTagNode
    {
        public string nodeName;
        public List<GameplayTagNode> children;
        public GameplayTagNode parent;
        public GameplayTag tag;

        public GameplayTagNode()
        {
            tag = new GameplayTag();
            children = new List<GameplayTagNode>();
            parent = null;
        }

        public GameplayTagNode(string name, GameplayTag tag, GameplayTagNode parent)
        {
            this.nodeName = name;
            this.tag = tag;
            children = new List<GameplayTagNode>();
            this.parent = parent;
        }
    }
}