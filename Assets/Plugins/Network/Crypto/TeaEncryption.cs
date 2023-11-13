using System;
namespace NGE.Crypto
{
    /// <summary>
    /// Tea加密(6-32轮)
    /// </summary>
    public sealed class TeaEncryption
    {
		private const uint m_delta = 0x9E3779B9;
        private int m_loop = 16;//16轮加密,为了加快运算速度，没有采用32轮加密
        private uint m_sum,a,b,c,d;
		private byte[] m_key;
        public byte[] Key
        {
            get { return m_key; }
            set
            {
                if(value == null || value.Length != 16)
                    return;
                m_key = value;
                Genericabcd();
            }
        }
        public TeaEncryption(int loop)
        {
            m_loop = loop;
            m_sum = ((uint)m_loop) * m_delta;
        }
        public TeaEncryption():this(16)
        {
        }
        public void GenericKey()
        {
            m_key = new byte[16];//128bit
            for (int i = 0; i < 15; i++)
            {
                m_key[i] = (byte)Util.Utility.Randomizer.Next(255);
            }
            Genericabcd();
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">输入输出数据</param>
        /// <param name="length"></param>
        public void Encrypt(byte[] data,int offset,int length)
        {
            int step = length / 8;
            unsafe
            {
                fixed (byte* pbyte = data)
                {
                    byte* ppos = pbyte +offset;
                    for (int i = 0; i < length && step-- > 0; i += 8)
                    {
                        uint* v = (uint*)(ppos + i);
                        uint y = v[0], z = v[1], sum = 0;
                        int n = m_loop;
                        while (n-- > 0)
                        {
                            sum += m_delta;
                            y += (z << 4) + a ^ z + sum ^ (z >> 5) + b;
                            z += (y << 4) + c ^ y + sum ^ (y >> 5) + d;
                        }
                        v[0] = y;
                        v[1] = z;
                    }
                }
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">输入输出数据</param>
        /// <param name="length"></param>
        public void Decrypt(byte[] data, int offset, int length)
        {
            int step = length / 8;
            unsafe
            {
                fixed (byte* pbyte = data)
                {
                    byte* ppos = pbyte + offset;
                    for (int i = 0; i < length && step-- > 0; i += 8)
                    {
                        uint* v = (uint*)(ppos + i);
                        uint y = v[0], z = v[1], sum = m_sum;
                        int n = m_loop;
                        while (n-- > 0)
                        {
                            z -= (y << 4) + c ^ y + sum ^ (y >> 5) + d;
                            y -= (z << 4) + a ^ z + sum ^ (z >> 5) + b;
                            sum -= m_delta;
                        }
                        v[0] = y;
                        v[1] = z;
                    }
                }
            }
        }
        private unsafe void Genericabcd()
        {
            fixed(byte* pkey = m_key)
            {
                uint* pint = (uint*)pkey;
                a = pint[0];
                b = pint[1];
                c = pint[2];
                d = pint[3];
            }
        }
    }
}
