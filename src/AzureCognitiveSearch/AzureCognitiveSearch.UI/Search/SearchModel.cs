// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace CognitiveSearch.UI
{
    public class SearchModel
    {
        private string[] facets = new string[]
        {
            // Add UI facets here in order
            //"people", 
            //"locations",
            //"organizations",
            //"keyphrases"
            "organizations",
            "persons",
            "locations"
        };

        private string[] tags = new string[]
        {
            // Add tags fields here in order
            //"people", 
            //"locations",
            //"organizations",
            //"keyphrases"
            "keyPhrases",
            "organizations",
            "persons",
            "locations"
        };

        private readonly string[] resultFields = new string[]
        {
            "id",
            "metadata_storage_name",
            "persons",
            "locations",
            "organizations",
            "keyPhrases",
            "name",
            "myOcrText"
            // Add fields needed to display results cards

            // NOTE: if you customize the resultFields, be sure to include metadata_storage_name,
            // id as those fields are needed for the UI to work properly
            //"people",
            //"locations",
            //"organizations",
            //"keyphrases"
        };

        public List<SearchField> Facets { get; set; }
        public List<SearchField> Tags { get; set; }

        public string[] SelectFilter { get; set; }

        public Dictionary<string, string[]> SearchFacets { get; set; }

        public SearchModel(SearchSchema schema)
        {
            Facets = new List<SearchField>();
            Tags = new List<SearchField>();
            SelectFilter = resultFields;

            if (facets?.Any() == true)
            {
                // add field to facets if in facets arr
                foreach (var field in facets)
                {
                    if (schema.Fields[field] != null && schema.Fields[field].IsFacetable)
                    {
                        Facets.Add(schema.Fields[field]);
                    }
                }
            }
            else
            {
                foreach (var field in schema.Fields.Where(f => f.Value.IsFacetable))
                {
                    Facets.Add(field.Value);
                }
            }

            if (tags?.Any() == true)
            {
                foreach (var field in tags)
                {
                    if (schema.Fields[field] != null && schema.Fields[field].IsFacetable)
                    {
                        Tags.Add(schema.Fields[field]);
                    }
                }
            }
            else
            {
                foreach (var field in schema.Fields.Where(f => f.Value.IsFacetable))
                {
                    Tags.Add(field.Value);
                }
            }
        }
    }
}
