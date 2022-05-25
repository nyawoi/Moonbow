using HarmonyLib;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    public static class Extensions
    {
        public static T AccessField<T>(this object targetObject, string fieldName)
        {
            return (T) AccessTools.Field(targetObject.GetType(), fieldName)?.GetValue(targetObject);
        }

        public static void InvokeMethod(this object targetObject, string methodName, params object[] methodParameters)
        {
            AccessTools.Method(targetObject.GetType(), methodName)?.Invoke(targetObject, methodParameters);
        }
        
        public static T InvokeMethod<T>(this object targetObject, string methodName, params object[] methodParameters)
        {
            return (T) AccessTools.Method(targetObject.GetType(), methodName)?.Invoke(targetObject, methodParameters);
        }
    }
}