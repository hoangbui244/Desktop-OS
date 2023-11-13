using System;
namespace NGE
{
	/// <summary>
	/// ����ϵͳ������,�ṩ��ʱǿ���������ջ���(0~2�����ж���)
	/// </summary>
	public sealed class CoreSystem
	{
		private static System.Threading.Thread m_GCThread;
		public static void StartUp()
		{
			if(m_GCThread == null)
			{
				m_GCThread = new System.Threading.Thread(new System.Threading.ThreadStart(GCThreadFun));
				m_GCThread.Start();
			}
		}
		public static void ClearUp()
		{
			if(m_GCThread != null)
			{
				m_GCThread.Abort();
				m_GCThread = null;
			}
		}
		private static void GCThreadFun()
		{
			while(true)
			{
				System.Threading.Thread.Sleep(5000);
				//GC.Collect();
				//GC.WaitForPendingFinalizers();
			}
		}
	}
}
