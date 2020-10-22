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

        public void OpenTagSpace(TagSpace newSpace)
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
            TagSpaces.Push(newSpace);
        }

        public TagSpaceManager(RuntimeData runtimeData)
        {
            TagSpaces = runtimeData.TagSpaces;
        }
    }
}
