//using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
//using System;
//using System.Collections.Generic;

//namespace StatCan.OrchardCore.AjaxForms
//{
//    public class FormClientModelValidationProvider : IClientModelValidatorProvider
//    {
//        public void CreateValidators(ClientValidatorProviderContext context)
//        {
//            if (context == null)
//            {
//                throw new ArgumentNullException(nameof(context));
//            }
//            var results = context.Results;
//            // Read interface .Count once rather than per iteration
//            var resultsCount = results.Count;
//            for (var i = 0; i < resultsCount; i++)
//            {
//                var validatorItem = results[i];
//            }
//        }
//    }

//    public class FormClientModelValidator : IClientModelValidator
//    {
//        public void AddValidation(ClientModelValidationContext context)
//        {
//            if (context == null)
//            {
//                throw new ArgumentNullException(nameof(context));
//            }
//        }

//        protected static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
//        {
//            if (attributes.ContainsKey(key))
//            {
//                return false;
//            }

//            attributes.Add(key, value);
//            return true;
//        }       
//    }
//}