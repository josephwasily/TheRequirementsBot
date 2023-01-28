// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples
{
    /// <summary>
    /// This is our application state. Just a regular serializable .NET class.
    /// </summary>
    public class UserProfile
    {
        public string AuthorName { get; set; }
        public string User { get; set; }
        public string NoOfUsers { get; set; }
        public string Feature { get; set; }
        public string Risk { get; set; }
        public int TechnicalComplexity { get; set; }
        public int BusinessImportance { get; set; }

    }
}
