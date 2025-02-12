using Microsoft.Extensions.Caching.Memory;
using PrimitiveTextGame.Telegram.Modules.Games.Constants;
using System.Reflection;

namespace PrimitiveTextGame.Telegram.Modules.Games
{
    public class ParametersTemplateService : IParametersTemplateService
    {
        private readonly IMemoryCache _memoryCache;

        public ParametersTemplateService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<Dictionary<string, object>> GetParametersAsync(string templateName, params object[] userDataObjects)
        {
            if (!_memoryCache.TryGetValue(TemplateConstants.ParametersTemplate, out Dictionary<string, List<string>> parametersTemplateDictionary))
            {
                throw new InvalidOperationException("Parameters template not found in cache.");
            }

            if (!parametersTemplateDictionary.ContainsKey(templateName))
            {
                throw new InvalidOperationException($"Template '{templateName}' not found.");
            }

            var parameterKeys = parametersTemplateDictionary[templateName];
            var parameters = new Dictionary<string, object>();
            if (userDataObjects.Length == 1)
            {
                var userData = userDataObjects[0];
                foreach (var key in parameterKeys)
                {
                    var value = GetPropertyValue(userData, key);
                    var simplifiedKey = key.Replace(".", "");  // Убираем точки в ключах
                    parameters[simplifiedKey] = value;
                }
            }
            else
            {
                foreach (var key in parameterKeys)
                {
                    var objectKey = key.Split('.')[0];
                    var propertyKey = key.Substring(objectKey.Length + 1);
                    object targetObject = null;
                    if (objectKey == "User")
                    {
                        targetObject = userDataObjects.FirstOrDefault();
                    }
                    else if (objectKey == "Opponent")
                    {
                        targetObject = userDataObjects.Skip(1).FirstOrDefault();  
                    }
                    if (targetObject == null)
                    {
                        throw new Exception($"Object with name '{objectKey}' not found among passed objects.");
                    }

                    var value = GetPropertyValue(targetObject, propertyKey);
                    var simplifiedKey = key.Replace(".", "");
                    parameters[simplifiedKey] = value;
                }
            }
            return parameters;
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            var propertyNames = propertyName.Split('.');
            var currentObject = obj;
            foreach (var property in propertyNames)
            {
                var propertyInfo = currentObject.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    throw new Exception($"Property '{property}' not found on type {currentObject.GetType()}");
                }

                currentObject = propertyInfo.GetValue(currentObject);
                if (currentObject == null)
                {
                    throw new Exception($"Property '{property}' is null on type {currentObject.GetType()}");
                }
            }
            return currentObject;
        }
    }
}
