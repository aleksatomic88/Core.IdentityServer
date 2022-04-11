using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Model
{
    public class ValidationError : Exception
    {
        public List<ApiError> Errors = new List<ApiError>();

        public bool HasWarning =>
            Errors.Any(e => e.Severity == Severity.Warning);

        public ValidationError(IList<ValidationFailure> failures)
        {
            if (failures != null)
            {
                failures.ToList().ForEach(failure =>
                {
                    Errors.Add(new ApiError((int)System.Net.HttpStatusCode.BadRequest, failure.ErrorMessage, failure.Severity));
                });

                // If there is at least one error, then remove warning from list
                // Warnings should be shown only when errors dont exist
                if (Errors.Any(e => e.Severity == Severity.Error))
                {
                    Errors = Errors.Where(e => e.Severity == Severity.Error).ToList();
                }
            }
        }

        public ValidationError(List<ApiError> errors)
        {
            Errors = errors;
        }
    }
}
