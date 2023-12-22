using Grpc.Core.Interceptors;
using Grpc.Core;

namespace API.Classes;

public class ServerTestInterceptor : Interceptor
{
   
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
       
        Console.WriteLine($"MethodName= {context.Method} , Date={DateTime.Now.ToString()}");
        return await continuation(request, context);
    }
}
