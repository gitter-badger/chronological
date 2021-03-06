﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chronological.QueryResults.Aggregates
{

    public class AggregateQueryResult : QueryResult
    {
        public Headers Headers { get; set; }
        [JsonProperty("content")]
        [JsonConverter(typeof(AggregateQueryResultAggregateJsonConverter))]
        public List<AggregateQueryResultAggregate> Content { get; set; }
        public List<object> Warnings { get; set; }
        
    }
}
