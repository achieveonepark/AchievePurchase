using System;
using System.Threading.Tasks;

namespace com.achieve.scripting.purchase
{
    public static class TaskExtension
    {
        public static async Task<T> Timeout<T>(this Task<T> task, TimeSpan time)
        {
            // Ÿ�Ӿƿ� �½�ũ ����
            Task delayTask = Task.Delay(time);
            Task firstToFinish = await Task.WhenAny(task, delayTask);

            if (firstToFinish == delayTask)
            {
                // Ÿ�Ӿƿ� �߻�
                throw new TimeoutException("The operation has timed out.");
            }

            // ���� �½�ũ�� ��� ��ȯ (�½�ũ�� ���ܸ� �߻����״ٸ� �� �� ���ܰ� �������ϴ�)
            return await task;
        }
    }
}