using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser
{
    class TagSpaceManager
    {
        public Stack<TagSpace> TagSpaces { get; set; }

        public void CloseTagSpace()
        {
            TagSpaces.Pop();
        }

        #region OpenTagSpace
        public void OpenTagSpace(TagSpace newSpace)
        {
            if (TagSpaces.Count > 0)
            {
                var prevTags = TagSpaces.Peek().Tags;
                var newTags = newSpace.Tags;

                foreach (var tag in prevTags)
                {
                    if (!newTags.ContainsKey(tag.Key))
                    {
                        newTags.Add(tag.Key, tag.Value);
                    }
                }
            }
            TagSpaces.Push(newSpace);
        }

        public void OpenTagSpace(Dictionary<string, string> tags) =>
            OpenTagSpace(new TagSpace(tags));

        public void OpenTagSpace() =>
            OpenTagSpace(new Dictionary<string, string>());
        #endregion

        #region SetTag
        public void SetTag(string tagName, string tagValue)
        {
            var tags = TagSpaces.Peek().Tags;
            if (tags.ContainsKey(tagName))
                tags[tagName] = tagValue;
            else
                tags.Add(tagName, tagValue);
        }
        #endregion

        #region Constructor
        public TagSpaceManager(RuntimeData runtimeData)
        {
            TagSpaces = runtimeData.TagSpaces;
        }
        #endregion
    }
}
