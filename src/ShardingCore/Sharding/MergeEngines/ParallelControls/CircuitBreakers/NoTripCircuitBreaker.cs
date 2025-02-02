﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShardingCore.Sharding.Abstractions;

namespace ShardingCore.Sharding.MergeEngines.ParallelControls.CircuitBreakers
{
    internal class NoTripCircuitBreaker:AbstractCircuitBreaker
    {
        public NoTripCircuitBreaker(ISeqQueryProvider seqQueryProvider) : base(seqQueryProvider)
        {
        }

        protected override bool SeqConditionalTrip<TResult>(IEnumerable<TResult> results)
        {
            return false;
        }

        protected override bool RandomConditionalTrip<TResult>(IEnumerable<TResult> results)
        {
            return false;
        }
    }
}
