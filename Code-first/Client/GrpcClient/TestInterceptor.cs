using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Interceptors.Interceptor;
using System.Reflection;

namespace GrpcClient;
//https://learn.microsoft.com/en-us/aspnet/core/grpc/interceptors?view=aspnetcore-7.0
public class TestInterceptor : Interceptor
{


    public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Console.WriteLine($"MethodName= {context.Method.Name} , Date={DateTime.Now.ToString()}");
        return base.BlockingUnaryCall(request, context, continuation);
    }

   
  
}
