using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;

public static class HighlightProfiles
{
    public enum Profile
    {
        None,
        Highlighted,
        Preview
    }

    private static Dictionary<Profile, HighlightProfile> profiles = new();
    public static HighlightProfile GetHighlightProfile(Profile profile)
    {
        if (!profiles.ContainsKey(profile))
        {
            profiles.Add(profile, Resources.Load<HighlightProfile>($"HighlightProfiles/{profile}"));
        }

        return profiles[profile];
    }
}