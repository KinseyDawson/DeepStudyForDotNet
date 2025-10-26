//#define  decompile_version
#define  code_optimize
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace AsyncStateMachineStudy;
 
public class Program
{
    public static async Task Main(string[] args)
    {
        await MainAsync();
    }

    // public static void Main(string[] args)
    // {
    //     MainAsync().GetAwaiter().GetResult();
    // }

    private static Task MainAsync()
    {
        //构造状态机
        var stateMachine = new ProgramAsyncStateMachine
        {
            _asyncTaskMethodBuilder = AsyncTaskMethodBuilder.Create(),
            _state = -1
        };
        //启动状态机，执行MoveNext方法，builder的Task初始为null，在第一次MoveNext时，会对此task进行赋值操作
        stateMachine._asyncTaskMethodBuilder.Start(ref stateMachine);
        return stateMachine._asyncTaskMethodBuilder.Task;
    }
    private class ProgramAsyncStateMachine : IAsyncStateMachine
    {
        public AsyncTaskMethodBuilder _asyncTaskMethodBuilder;
        private TaskAwaiter _awaiter;
        public int _state = -1;
        Task? temp = null;

#if decompile_version
        public void MoveNext()
        {
            var num1 = this._state;
            try
            {
                TaskAwaiter awaiter1;
                TaskAwaiter awaiter2;
                if (num1 != 0)
                {
                    if (num1 != 1)
                    {
                        Console.WriteLine("1");
                        awaiter1 = Task.Delay(TimeSpan.FromSeconds(10.0)).GetAwaiter();
                        if (!awaiter1.IsCompleted)
                        {
                            this._state = 0;
                            this._awaiter = awaiter1;
                            ProgramAsyncStateMachine stateMachine = this;
                            //_asyncTaskMethodBuilder的内部task初始为null，在第一次AwaitUnsafeOnCompleted内部对其赋值
                            //该内部task成为异步操作的可等待对象,也就是该异步方法的调用者通过await来等待此内部task完成
                            this._asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter1, ref stateMachine);
                            temp = _asyncTaskMethodBuilder.Task;
                            return;
                        }
                    }
                    else
                    {
                        awaiter2 = this._awaiter;
                        this._awaiter = new TaskAwaiter();
                        this._state = -1;
                        goto label_9;
                    }
                }
                else
                {
                    awaiter1 = this._awaiter;
                    this._awaiter = new TaskAwaiter();
                    this._state = -1;
                }
                awaiter1.GetResult();
                Console.WriteLine("2");
                awaiter2 = Task.Delay(TimeSpan.FromSeconds(20.0)).GetAwaiter();
                if (!awaiter2.IsCompleted)
                {
                    this._state = 1;
                    this._awaiter = awaiter2;
                    ProgramAsyncStateMachine stateMachine = this;
                    this._asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref stateMachine);
                    var r = ReferenceEquals(temp, _asyncTaskMethodBuilder.Task);
                    return;
                }
                label_9:
                awaiter2.GetResult();
                Console.WriteLine("3");
            }
            catch (Exception ex)
            {
                this._state = -2;
                this._asyncTaskMethodBuilder.SetException(ex);
                return;
            }
            this._state = -2;
            //给内部task设置结果，标记该task完成，如果该task被await设置了回调操作，那么由task接管后续的操作执行
            this._asyncTaskMethodBuilder.SetResult();
        }
#endif

#if code_optimize
        public void MoveNext()
        {
            try
            {
                TaskAwaiter awaiter;
                if (_state == -1) //初始状态
                {
                    Console.WriteLine("1");
                    awaiter = Task.Delay(TimeSpan.FromSeconds(10.0)).GetAwaiter();
                    //awaiter = Task.CompletedTask.GetAwaiter(); 
                    _awaiter = awaiter;
                    _state = 0;
                    if (!awaiter.IsCompleted)
                    {
                        ProgramAsyncStateMachine stateMachine = this;
                        //_asyncTaskMethodBuilder的内部task初始为null，在第一次AwaitUnsafeOnCompleted内部对其赋值
                        //该内部task成为异步操作的可等待对象,也就是该异步方法的调用者通过await来等待此内部task完成
                        _asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
                        temp = _asyncTaskMethodBuilder.Task;
                        return;
                    }
                }
                if (_state == 0) //第0个任务完成
                {
                    _awaiter.GetResult();
                    Console.WriteLine("2");
                    awaiter = Task.Delay(TimeSpan.FromSeconds(20.0)).GetAwaiter();
                    //awaiter = Task.CompletedTask.GetAwaiter();
                    _awaiter = awaiter;
                    _state = 1;
                    if (!awaiter.IsCompleted)
                    {
                        ProgramAsyncStateMachine stateMachine = this;
                        _asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
                        Debug.WriteLine($"底层task是否发生变化?:{temp != _asyncTaskMethodBuilder.Task}");
                        return;
                    }
                }
                if (_state == 1) //第1个任务完成
                {
                    _awaiter.GetResult();
                    Console.WriteLine("3");
                }
                _state = -2;
                //给内部task设置结果，标记该task完成，如果该task被await设置了回调操作，那么由task接管后续的操作执行
                _asyncTaskMethodBuilder.SetResult();
            }
            catch (Exception ex)
            {
                _state = -2;
                _asyncTaskMethodBuilder.SetException(ex);
            }
        }
#endif
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
