using System;
using System.Collections.Generic;
using System.Text;
namespace NGE.Algorithm
{
    /// <summary>
    /// ѭ��id�б�,id��1��ʼ��maxid,��һ��idΪmaxid++
    /// 0Ϊ��Ч
    /// </summary>
    public class CycIDList
    {
        Queue<int> m_freeIDqueue = new Queue<int>();
        int[] m_array;
        public CycIDList(int maxid)
		{
            m_array = new int[maxid + 1];
            for (int i = 1; i <= maxid; i++)
                m_freeIDqueue.Enqueue(i);
		}
		public int GetNext()
		{
            if (m_freeIDqueue.Count == 0)
                return 0;
            int id = m_freeIDqueue.Dequeue();
            m_array[id] = 1;//1��ʾʹ����
            return id;
		}
		public void Remove(int id)
		{
            if (m_array[id] == 0)
                return;
            m_array[id] = 0;
            m_freeIDqueue.Enqueue(id);
		}
    }
}
