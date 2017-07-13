using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace LocalApi.Routing
{
    public class HttpRoute
    {
        public HttpRoute(string controllerName, string actionName, HttpMethod methodConstraint) :
            this(controllerName, actionName, methodConstraint, null)
        {
        }

        #region Please modifies the following code to pass the test

        /*
         * You can add non-public helper method for help, but you cannot change public
         * interfaces.
         */

        public HttpRoute(string controllerName, string actionName, HttpMethod methodConstraint, string uriTemplate)
        {
            VerifyIdentifier(controllerName, nameof(controllerName));
            VerifyIdentifier(actionName, nameof(actionName));

            ControllerName = controllerName;
            ActionName = actionName;
            MethodConstraint = methodConstraint ?? throw new ArgumentNullException(nameof(methodConstraint));
            UriTemplate = uriTemplate;
        }

        static void VerifyIdentifier(string identifier, string memberName)
        {
            if (identifier == null) throw new ArgumentNullException(memberName);

            var regex = new Regex("^[a-z][a-z0-9]*$", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(identifier)) throw new ArgumentException();
        }

        #endregion

        public string ControllerName { get; }
        public string ActionName { get; }
        public HttpMethod MethodConstraint { get; }
        public string UriTemplate { get; }

        public bool IsMatch(Uri uri, HttpMethod method)
        {
            if (uri == null) { throw new ArgumentNullException(nameof(uri)); }
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            string path = uri.AbsolutePath.TrimStart('/');
            return path.Equals(UriTemplate, StringComparison.OrdinalIgnoreCase) &&
                   method == MethodConstraint;
        }
    }
}