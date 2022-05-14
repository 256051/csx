using Framework.Core;
using Framework.Core.Dependency;
using Framework.ExceptionHandling.Http;
using Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.ExceptionHandling
{
    public class DefaultExceptionToErrorInfoConverter : IExceptionToErrorInfoConverter, ITransient
    {
        public RemoteServiceErrorInfo Convert(Exception exception, bool includeSensitiveDetails)
        {
            var errorInfo = CreateErrorInfoWithoutCode(exception, includeSensitiveDetails);

            if (exception is IHasErrorCode hasErrorCodeException)
            {
                errorInfo.Code = hasErrorCodeException.Code;
            }

            return errorInfo;
        }

        /// <summary>
        /// 尝试捕获实际抛出的异常,因为有一部分是线程异常,必须定位到他的真实异常
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected virtual Exception TryToGetActualException(Exception exception)
        {
            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;

                if (aggException.InnerException is FrameworkValidationException ||
                    aggException.InnerException is IBusinessException)
                {
                    return aggException.InnerException;
                }
            }

            return exception;
        }

        protected virtual RemoteServiceErrorInfo CreateErrorInfoWithoutCode(Exception exception, bool includeSensitiveDetails)
        {
            //直接输出异常详情
            if (includeSensitiveDetails)
            {
                return CreateDetailedErrorInfoFromException(exception);
            }

            exception = TryToGetActualException(exception);

            var errorInfo = new RemoteServiceErrorInfo();

            if (exception is IUserFriendlyException)
            {
                errorInfo.Message = exception.Message;
                errorInfo.Details = (exception as IHasErrorDetails)?.Details;
            }

            if (exception is IHasValidationErrors)
            {
                if (errorInfo.Message.IsNullOrEmpty())
                {
                    errorInfo.Message = "ValidationErrorMessage";
                }

                if (errorInfo.Details.IsNullOrEmpty())
                {
                    errorInfo.Details = GetValidationErrorNarrative(exception as IHasValidationErrors);
                }

                errorInfo.ValidationErrors = GetValidationErrorInfos(exception as IHasValidationErrors);
            }

            if (errorInfo.Message.IsNullOrEmpty())
            {
                errorInfo.Message = "InternalServerErrorMessage";
            }

            errorInfo.Data = exception.Data;

            return errorInfo;
        }

        protected virtual RemoteServiceErrorInfo CreateDetailedErrorInfoFromException(Exception exception)
        {
            var detailBuilder = new StringBuilder();

            AddExceptionToDetails(exception, detailBuilder);

            var errorInfo = new RemoteServiceErrorInfo(exception.Message, detailBuilder.ToString());

            if (exception is FrameworkValidationException)
            {
                errorInfo.ValidationErrors = GetValidationErrorInfos(exception as FrameworkValidationException);
            }

            return errorInfo;
        }

        /// <summary>
        /// 将异常详情写入到detail中
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="detailBuilder"></param>
        protected virtual void AddExceptionToDetails(Exception exception, StringBuilder detailBuilder)
        {
            detailBuilder.AppendLine(exception.GetType().Name + ": " + exception.Message);

            if (exception is IUserFriendlyException &&
                exception is IHasErrorDetails)
            {
                var details = ((IHasErrorDetails)exception).Details;
                if (!details.IsNullOrEmpty())
                {
                    detailBuilder.AppendLine(details);
                }
            }

            if (exception is FrameworkValidationException)
            {
                var validationException = exception as FrameworkValidationException;
                if (validationException.ValidationErrors.Count > 0)
                {
                    detailBuilder.AppendLine(GetValidationErrorNarrative(validationException));
                }
            }

            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                detailBuilder.AppendLine("STACK TRACE: " + exception.StackTrace);
            }

            if (exception.InnerException != null)
            {
                AddExceptionToDetails(exception.InnerException, detailBuilder);
            }

            if (exception is AggregateException)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerExceptions!=null && aggException.InnerExceptions.Count>0)
                {
                    return;
                }

                foreach (var innerException in aggException.InnerExceptions)
                {
                    AddExceptionToDetails(innerException, detailBuilder);
                }
            }
        }

        protected virtual RemoteServiceValidationErrorInfo[] GetValidationErrorInfos(IHasValidationErrors validationException)
        {
            var validationErrorInfos = new List<RemoteServiceValidationErrorInfo>();

            foreach (var validationResult in validationException.ValidationErrors)
            {
                var validationError = new RemoteServiceValidationErrorInfo(validationResult.ErrorMessage);

                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    validationError.Members = validationResult.MemberNames.ToArray();
                }

                validationErrorInfos.Add(validationError);
            }

            return validationErrorInfos.ToArray();
        }

        protected virtual string GetValidationErrorNarrative(IHasValidationErrors validationException)
        {
            var detailBuilder = new StringBuilder();
            detailBuilder.AppendLine("ValidationNarrativeErrorMessageTitle");

            foreach (var validationResult in validationException.ValidationErrors)
            {
                detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
                detailBuilder.AppendLine();
            }

            return detailBuilder.ToString();
        }
    }
}
