using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimationParser {
    public enum AnimationCommandType {
        Trigger,    // One-time animation (nod, shake, etc.)
        Preset      // Change to emotion preset (excited, calm, etc.)
    }

    public class AnimationTag {
        public string _AnimationType;
        public int _StartIndex;
        public int _EndIndex;
        public AnimationCommandType _CommandType;

        public AnimationTag(string i_type, int i_start, int i_end, AnimationCommandType i_commandType = AnimationCommandType.Trigger) {
            _AnimationType = i_type;
            _StartIndex = i_start;
            _EndIndex = i_end;
            _CommandType = i_commandType;
        }
    }

    // Define which animation types are presets vs triggers
    private static readonly HashSet<string> PresetAnimations = new HashSet<string>
    {
        // "excited", "calm", "nervous", "serious", "sad", "angry", "happy", "scared", "confident"
    };

    /// <summary>
    /// Parse animation tags from text and return cleaned text + animation data
    /// </summary>
    public static (string cleanText, List<AnimationTag> animations) Parse(string i_text) {
        List<AnimationTag> animations = new List<AnimationTag>();
        string cleanText = i_text;

        // Find all opening tags
        int searchIndex = 0;
        while (searchIndex < cleanText.Length) {
            int openingTagStart = cleanText.IndexOf('[', searchIndex);
            if (openingTagStart == -1) break;

            int openingTagEnd = cleanText.IndexOf(']', openingTagStart);
            if (openingTagEnd == -1) break;

            // Extract animation type
            string openingTag = cleanText.Substring(openingTagStart + 1, openingTagEnd - openingTagStart - 1);

            // Skip if it's a closing tag
            if (openingTag.StartsWith("/")) {
                searchIndex = openingTagEnd + 1;
                continue;
            }

            // Find matching closing tag
            string closingTag = $"[/{openingTag}]";
            int closingTagStart = cleanText.IndexOf(closingTag, openingTagEnd);

            if (closingTagStart == -1) {
                Debug.LogWarning($"No closing tag found for [{openingTag}]");
                searchIndex = openingTagEnd + 1;
                continue;
            }

            // Calculate indices for the actual text (without tags)
            int contentStart = openingTagEnd + 1;
            int contentEnd = closingTagStart;

            // Create animation tag with adjusted indices (accounting for removed tags)
            int adjustedStart = contentStart - (openingTagEnd - openingTagStart + 1);
            int adjustedEnd = adjustedStart + (contentEnd - contentStart);

            // Determine if this is a preset or trigger
            AnimationCommandType commandType = IsPresetAnimation(openingTag)
                ? AnimationCommandType.Preset
                : AnimationCommandType.Trigger;

            animations.Add(new AnimationTag(openingTag, adjustedStart, adjustedEnd, commandType));

            // Remove the tags from the text
            cleanText = cleanText.Remove(closingTagStart, closingTag.Length);
            cleanText = cleanText.Remove(openingTagStart, openingTagEnd - openingTagStart + 1);

            // Update search position
            searchIndex = openingTagStart;
        }

        return (cleanText, animations);
    }

    /// <summary>
    /// Get the animation tag for the current character index
    /// </summary>
    public static AnimationTag GetAnimationAtIndex(int charIndex, List<AnimationTag> animations) {
        foreach (var anim in animations) {
            if (charIndex >= anim._StartIndex && charIndex < anim._EndIndex) {
                return anim;
            }
        }
        return null;
    }

    /// <summary>
    /// Check if entering a new animation range
    /// </summary>
    public static bool IsEnteringAnimationRange(int charIndex, List<AnimationTag> animations, out AnimationTag animationTag) {
        animationTag = null;

        foreach (var anim in animations) {
            if (charIndex == anim._StartIndex) {
                animationTag = anim;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check if exiting an animation range
    /// </summary>
    public static bool IsExitingAnimationRange(int charIndex, List<AnimationTag> animations, out AnimationTag animationTag) {
        animationTag = null;

        foreach (var anim in animations) {
            if (charIndex == anim._EndIndex) {
                animationTag = anim;
                return true;
            }
        }

        return false;
    }

    private static bool IsPresetAnimation(string animationType) {
        return PresetAnimations.Contains(animationType.ToLower());
    }

    /// <summary>
    /// Add a custom preset animation type
    /// </summary>
    public static void RegisterPresetAnimation(string presetName) {
        PresetAnimations.Add(presetName.ToLower());
    }
}