using System;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace LocalApi
{
    static class ControllerActionInvoker
    {
        public static HttpResponseMessage InvokeAction(ActionDescriptor actionDescriptor)
        {
            object instance = Activator.CreateInstance(actionDescriptor.Controller.GetType());
            MethodInfo methodInfo = instance.GetType().GetMethod(actionDescriptor.ActionName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (methodInfo == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
            try
            {
                return (HttpResponseMessage)methodInfo.Invoke(instance, null);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
