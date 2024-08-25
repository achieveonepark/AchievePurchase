using System;
using System.Threading.Tasks;

namespace com.achieve.scripting.purchase
{
    public static class TaskExtension
    {
        public static async Task<T> Timeout<T>(this Task<T> task, TimeSpan time)
        {
            // 타임아웃 태스크 생성
            Task delayTask = Task.Delay(time);
            Task firstToFinish = await Task.WhenAny(task, delayTask);

            if (firstToFinish == delayTask)
            {
                // 타임아웃 발생
                throw new TimeoutException("The operation has timed out.");
            }

            // 원래 태스크의 결과 반환 (태스크가 예외를 발생시켰다면 이 때 예외가 던져집니다)
            return await task;
        }
    }
}