using System;
using System.Reflection;

namespace RPG.Tests
{
    internal static class EditModeTestLifecycle
    {
        public static void Invoke(object target, string methodName, params object[] arguments)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            MethodInfo method = target.GetType().GetMethod(
                methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (method == null)
            {
                throw new MissingMethodException(target.GetType().FullName, methodName);
            }

            method.Invoke(target, arguments);
        }
    }
}
