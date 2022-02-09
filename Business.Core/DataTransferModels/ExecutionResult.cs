using System.Collections.Generic;
using System.Net;

namespace Totem.Business.Core.DataTransferModels
{
    /// <summary>
    /// Represents result of an action.
    /// </summary>
    public class ExecutionResult
    {
        private bool? _success;

        #region Constructors

        public ExecutionResult() : this((ExecutionResult)null)
        { }

        /// <summary>
        /// Initialize execution result with one error
        /// </summary>
        public ExecutionResult(ErrorInfo error) : this(new[] { error })
        { }

        /// <summary>
        /// Initialize execution result with one information message
        /// </summary>
        public ExecutionResult(InfoMessage message) : this(new[] { message })
        { }

        /// <summary>
        /// Initialize execution result with error list
        /// </summary>
        public ExecutionResult(IEnumerable<ErrorInfo> errors) : this((ExecutionResult)null)
        {
            foreach (ErrorInfo errorInfo in errors)
            {
                Errors.Add(errorInfo);
            }
        }

        /// <summary>
        /// Initialize execution result with information message list
        /// </summary>
        public ExecutionResult(IEnumerable<InfoMessage> messages) : this((ExecutionResult)null)
        {
            foreach (InfoMessage message in messages)
            {
                Messages.Add(message);
            }
        }

        /// <summary>
        /// Main constructor
        /// </summary>
        public ExecutionResult(ExecutionResult result)
        {
            if (result != null)
            {
                Success = result.Success;
                Errors = new List<ErrorInfo>(result.Errors);
                Messages = new List<InfoMessage>(result.Messages);
            }
            else
            {
                Errors = new List<ErrorInfo>();
                Messages = new List<InfoMessage>();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if result is successful.
        /// </summary>
        public bool Success
        {
            get => _success ?? Errors.Count == 0;
            set => _success = value;
        }

        /// <summary>
        /// Errors collection
        /// </summary>
        public IList<ErrorInfo> Errors { get; }

        /// <summary>
        /// Info messages collection
        /// </summary>
        public IList<InfoMessage> Messages { get; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        #endregion
    }

    /// <summary>
    /// Represents result of an action that returns any value
    /// </summary>
    /// <typeparam name="T">Type of value to be returned with action</typeparam>
    public class ExecutionResult<T> : ExecutionResult
    {
        public ExecutionResult() : this((ExecutionResult)null)
        { }

        public ExecutionResult(T result) : this((ExecutionResult)null)
        {
            Value = result;
        }

        public ExecutionResult(T result, InfoMessage message) : this((ExecutionResult)null)
        {
            Value = result;
            Messages.Add(message);
        }

        public ExecutionResult(ExecutionResult result) : base(result)
        {
            if (result is ExecutionResult<T> r) // make sure result is not null and cast to ExecutionResult
            {
                Value = r.Value;
            }
        }

        public ExecutionResult(ErrorInfo error) : this(new[] { error })
        { }

        public ExecutionResult(InfoMessage message) : this(new[] { message })
        { }

        public ExecutionResult(IEnumerable<ErrorInfo> errors) : this((ExecutionResult)null)
        {
            foreach (ErrorInfo errorInfo in errors)
            {
                Errors.Add(errorInfo);
            }
        }

        public ExecutionResult(IEnumerable<InfoMessage> messages) : this((ExecutionResult)null)
        {
            foreach (InfoMessage message in messages)
            {
                Messages.Add(message);
            }
        }

        public T Value { get; set; }
    }
}
