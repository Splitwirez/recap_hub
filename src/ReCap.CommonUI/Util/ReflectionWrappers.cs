using System;
using System.Reflection;

namespace ReCap.CommonUI.Reflection
{
    public class MethodWrapper
    {
        protected readonly MethodInfo _methodInfo;
        internal MethodWrapper(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
        }
        protected MethodWrapper()
            : this(null)
        => throw new Exception();

        protected object InvokeInternal(object instance, params object[] parameters)
            => _methodInfo.Invoke(instance, parameters);
        
        public object StaticCall(params object[] parameters)
            => InvokeInternal(default, parameters);

        public object InstanceCall(object instance, params object[] parameters)
            => InvokeInternal(instance, parameters);
        
        protected static object[] ToParamArray(params object[] objects)
            => objects;
    }
    public class OMethodWrapper<TOwner>
        : MethodWrapper
    {
        internal OMethodWrapper(MethodInfo methodInfo)
            : base(methodInfo)
        {}
        public object InstanceCall(TOwner instance, params object[] parameters)
            => InvokeInternal(instance, parameters);
        

        protected static TRet ValidateReturn<TRet>(object value)
            => (value is TRet ret)
                ? ret
                : default
            ;
        
        public TRet StaticCall<TRet>(params object[] parameters)
            => ValidateReturn<TRet>(InvokeInternal(default, parameters));
        public TRet InstanceCall<TRet>(TOwner instance, params object[] parameters)
            => ValidateReturn<TRet>(_methodInfo.Invoke(instance, parameters));
    /*
        public TRet InvokeStatic(params object[] parameters)
            => InvokeInstance(default, parameters);
        
        public TRet InvokeInstance(TOwner instance, params object[] parameters)
            => (_methodInfo.Invoke(instance, parameters) is TRet ret)
                ? ret
                : default
            ;
    */
    }

    public class OPMethodWrapper<TOwner, TParam1>
        : OMethodWrapper<TOwner>
    {
        internal OPMethodWrapper(MethodInfo methodInfo)
            : base(methodInfo)
        {
            methodInfo.AreParametersValid(typeof(TParam1));
        }
        
        object CallInternal(TOwner instance, TParam1 p1)
            => InstanceCall(instance, p1);
        TRet CallInternalT<TRet>(TOwner instance, TParam1 p1)
            => ValidateReturn<TRet>(CallInternal(instance, p1));

        public object StaticCall(TParam1 p1)
            => CallInternal(default, p1);
        public object InstanceCall(TOwner instance, TParam1 p1)
            => CallInternal(instance, p1);

        public TRet StaticCall<TRet>(TParam1 p1)
            => CallInternalT<TRet>(default, p1);
        public TRet InstanceCall<TRet>(TOwner instance, TParam1 p1)
            => CallInternalT<TRet>(instance, p1);
    }

    public class OPMethodWrapper<TOwner, TParam1, TParam2>
        : OMethodWrapper<TOwner>
    {
        internal OPMethodWrapper(MethodInfo methodInfo)
            : base(methodInfo)
        {
            methodInfo.AreParametersValid(typeof(TParam1), typeof(TParam2));
        }
        
        object CallInternal(TOwner instance, TParam1 p1, TParam2 p2)
            => InstanceCall(instance, p1, p2);
        TRet CallInternalT<TRet>(TOwner instance, TParam1 p1, TParam2 p2)
            => ValidateReturn<TRet>(CallInternal(instance, p1, p2));

        public object StaticCall(TParam1 p1, TParam2 p2)
            => CallInternal(default, p1, p2);
        public object InstanceCall(TOwner instance, TParam1 p1, TParam2 p2)
            => CallInternal(instance, p1, p2);

        public TRet StaticCall<TRet>(TParam1 p1, TParam2 p2)
            => CallInternalT<TRet>(default, p1, p2);
        public TRet InstanceCall<TRet>(TOwner instance, TParam1 p1, TParam2 p2)
            => CallInternalT<TRet>(instance, p1, p2);
    }

    public class OPMethodWrapper<TOwner, TParam1, TParam2, TParam3>
        : OMethodWrapper<TOwner>
    {
        internal OPMethodWrapper(MethodInfo methodInfo)
            : base(methodInfo)
        {
            methodInfo.AreParametersValid(typeof(TParam1), typeof(TParam2), typeof(TParam3));
        }
        
        object CallInternal(TOwner instance, TParam1 p1, TParam2 p2, TParam3 p3)
            => InstanceCall(instance, p1, p2, p3);
        TRet CallInternalT<TRet>(TOwner instance, TParam1 p1, TParam2 p2, TParam3 p3)
            => ValidateReturn<TRet>(CallInternal(instance, p1, p2, p3));

        public object StaticCall(TParam1 p1, TParam2 p2, TParam3 p3)
            => CallInternal(default, p1, p2, p3);
        public object InstanceCall(TOwner instance, TParam1 p1, TParam2 p2, TParam3 p3)
            => CallInternal(instance, p1, p2, p3);

        public TRet StaticCall<TRet>(TParam1 p1, TParam2 p2, TParam3 p3)
            => CallInternalT<TRet>(default, p1, p2, p3);
        public TRet InstanceCall<TRet>(TOwner instance, TParam1 p1, TParam2 p2, TParam3 p3)
            => CallInternalT<TRet>(instance, p1, p2, p3);
    }

    public class OPMethodWrapper<TOwner, TParam1, TParam2, TParam3, TParam4>
        : OMethodWrapper<TOwner>
    {
        internal OPMethodWrapper(MethodInfo methodInfo)
            : base(methodInfo)
        {
            methodInfo.AreParametersValid(typeof(TParam1), typeof(TParam2), typeof(TParam3), typeof(TParam4));
        }
        
        object CallInternal(TOwner instance, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
            => InstanceCall(instance, p1, p2, p3, p4);
        TRet CallInternalT<TRet>(TOwner instance, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
            => ValidateReturn<TRet>(CallInternal(instance, p1, p2, p3, p4));

        public object StaticCall(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
            => CallInternal(default, p1, p2, p3, p4);
        public object InstanceCall(TOwner instance, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
            => CallInternal(instance, p1, p2, p3, p4);

        public TRet StaticCall<TRet>(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
            => CallInternalT<TRet>(default, p1, p2, p3, p4);
        public TRet InstanceCall<TRet>(TOwner instance, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
            => CallInternalT<TRet>(instance, p1, p2, p3, p4);
    }
}