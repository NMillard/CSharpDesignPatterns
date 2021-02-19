using System;
using System.Text.Json;

namespace GlobalMiddlewarePipeline {
    public abstract class CustomExceptionBase : Exception {
        protected CustomExceptionBase(string message, ErrorCode code) {
            Message = message;
            Code = code;
        }
        public string Message { get; }
        public ErrorCode Code { get; }
        
        public abstract string TransformToJson();
    }

    public class NotGoodException : CustomExceptionBase {
        public NotGoodException(string message, ErrorCode code) : base(message, code) {}

        public override string TransformToJson() => JsonSerializer.Serialize(new {
            base.Message,
            Code = base.Code.ToString()
        });
    }
    
    public class TooBusyException : CustomExceptionBase {
        private readonly DateTime tryAgainTime;
        public TooBusyException(string message, ErrorCode code, DateTime tryAgainTime) : base(message, code) {
            this.tryAgainTime = tryAgainTime;
        }

        public override string TransformToJson() => JsonSerializer.Serialize(new {
            base.Message,
            Code = base.Code.ToString(),
            WhenToTryAgain = tryAgainTime.ToString("g")
        });
    }

    public enum ErrorCode {
        Bad,
        VeryBad,
        Catastrophic,
    }
}