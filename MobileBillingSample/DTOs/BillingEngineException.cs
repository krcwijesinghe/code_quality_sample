using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBillingSample
{
    /// <summary>
    /// The generic business logic or conriguration related exception thrown by Billing Engine
    /// </summary>
    [Serializable]
    public class BillingEngineException : Exception
    {
        public BillingEngineException() { }
        public BillingEngineException(string message) : base(message) { }
        public BillingEngineException(string message, Exception inner) : base(message, inner) { }
        protected BillingEngineException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
