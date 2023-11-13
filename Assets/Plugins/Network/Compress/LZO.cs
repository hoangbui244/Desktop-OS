using System;
namespace NGE.Compress
{
    /// <summary>
    /// lzo快速压缩算法
    /// </summary>
    public sealed class LZO
    {
        private static byte[] wrkmem = new byte[4 * 16384L];
        private LZO() { }
        
        /// <summary>
        /// 压缩数据，返回压缩后的大小，返回-1表示失败
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcoffset"></param>
        /// <param name="count"></param>
        /// <param name="dst"></param>
        /// <param name="dstoffset"></param>
        /// <returns></returns>
        public static unsafe int Compress(byte[] src, int srcoffset, int count, byte[] dst, int dstoffset)
        {
            fixed(byte* psrc = src, pdst = dst)
            {
                byte* pin = psrc + srcoffset;
                byte* pout = pdst + dstoffset;
                byte* op = pout;
                uint t, in_len, out_len;
                in_len = (uint)count;
                if (in_len <= 13)
                    t = in_len;
                else
                {
                    lock (wrkmem)
                        t = _do_compress(pin, in_len, op, out out_len);
                    op += out_len;
                }
                if (t > 0)
                {
                    byte* ii = pin + in_len - t;
                    if (op == pout && t <= 238)
                        *op++ = (byte)(17 + t);
                    else
                        if (t <= 3)
                            op[-2] |= (byte)t;
                        else
                            if (t <= 18)
                                *op++ = (byte)(t - 3);
                            else
                            {
                                uint tt = t - 18;
                                *op++ = 0;
                                while (tt > 255)
                                {
                                    tt -= 255;
                                    *op++ = 0;
                                }
                                *op++ = (byte)tt;
                            }
                    do *op++ = *ii++;
                    while (--t > 0);
                }
                *op++ = 17;
                *op++ = 0;
                *op++ = 0;
                return (int)(op - pout);
            }
        }
        public static int Compress(byte[] src, int count, byte[] dst )
        {
            return Compress(src, 0, count, dst, 0);
        }
        /// <summary>
        /// 解压数据，返回解压后数据大小，返回-1表示失败
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcoffset"></param>
        /// <param name="dst"></param>
        /// <param name="dstoffset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static unsafe int Decompress(byte[] src, int srcoffset, int count, byte[] dst, int dstoffset)
        {
            fixed (byte* psrc = src, pdst = dst)
            {
                byte* pin = psrc + srcoffset;
                byte* pout = pdst + dstoffset;
                byte* op;
                byte* ip;
                int t;
                uint in_len = (uint)count;
                byte* m_pos;
                byte* ip_end = pin + in_len;
                op = pout;
                ip = pin;
                if (*ip > 17)
                {
                    t = *ip++ - 17;
                    if (t < 4)
                        goto match_next;
                    do *op++ = *ip++; while (--t > 0);
                    goto first_literal_run;
                }
            start_loop:
                t = *ip++;
                if (t >= 16) goto match;
                if (t == 0)
                {
                    while (*ip == 0)
                    {
                        t += 255;
                        ip++;
                    }
                    t += 15 + *ip++;
                }
                *(uint*)op = *(uint*)ip;
                op += 4; ip += 4;
                if (--t > 0)
                {
                    if (t >= 4)
                    {
                        do
                        {
                            *(uint*)op = *(uint*)ip;
                            op += 4; ip += 4; t -= 4;
                        } while (t >= 4);
                        if (t > 0) do *op++ = *ip++; while (--t > 0);
                    }
                    else
                        do *op++ = *ip++; while (--t > 0);
                }
            first_literal_run:
                t = *ip++;
                if (t >= 16)
                    goto match;
                m_pos = op - 0x0801;
                m_pos -= t >> 2;
                m_pos -= *ip++ << 2;
                *op++ = *m_pos++; *op++ = *m_pos++; *op++ = *m_pos;
                goto match_done;
            match:
                if (t >= 64)
                {
                    m_pos = op - 1;
                    m_pos -= (t >> 2) & 7;
                    m_pos -= *ip++ << 3;
                    t = (t >> 5) - 1;
                    *op++ = *m_pos++; *op++ = *m_pos++;
                    do *op++ = *m_pos++; while (--t > 0);
                    goto match_done;
                }
                else
                {
                    if (t >= 32)
                    {
                        t &= 31;
                        if (t == 0)
                        {
                            while (*ip == 0)
                            {
                                t += 255;
                                ip++;
                            }
                            t += 31 + *ip++;
                        }
                        m_pos = op - 1;
                        m_pos -= (*(ushort*)ip) >> 2;
                        ip += 2;
                    }
                    else
                    {
                        if (t >= 16)
                        {
                            m_pos = op;
                            m_pos -= (t & 8) << 11;
                            t &= 7;
                            if (t == 0)
                            {
                                while (*ip == 0)
                                {
                                    t += 255;
                                    ip++;
                                }
                                t += 7 + *ip++;
                            }
                            m_pos -= (*(ushort*)ip) >> 2;
                            ip += 2;
                            if (m_pos == op)
                                goto eof_found;
                            m_pos -= 0x4000;
                        }
                        else
                        {
                            m_pos = op - 1;
                            m_pos -= t >> 2;
                            m_pos -= *ip++ << 2;
                            *op++ = *m_pos++; *op++ = *m_pos;
                            goto match_done;
                        }
                    }
                }
                if (t >= 6 && (op - m_pos) >= 4)
                {
                    *(uint*)op = *(uint*)m_pos;
                    op += 4; m_pos += 4; t -= 2;
                    do
                    {
                        *(uint*)op = *(uint*)m_pos;
                        op += 4; m_pos += 4; t -= 4;
                    } while (t >= 4);
                    if (t > 0)
                        do *op++ = *m_pos++; while (--t > 0);
                }
                else
                {
                //copy_match:
                    *op++ = *m_pos++; *op++ = *m_pos++;
                    do *op++ = *m_pos++; while (--t > 0);
                }
            match_done:
                t = ip[-2] & 3;
                if (t == 0) goto start_loop;
            match_next:
                do *op++ = *ip++; while (--t > 0);
                t = *ip++;
                goto match;
            eof_found:
                if (ip != ip_end) return -1;
                return (int)(op - pout);
            }
        }
        public static int Decompress(byte[] src, int count, byte[] dst)
        {
            return Decompress(src, 0, count,dst, 0);
        }
        private static unsafe uint _do_compress(byte* pin, uint in_len, byte* pout, out uint out_len)
        {
            byte* ip;
            byte* op;
            byte* in_end = pin + in_len;
            byte* ip_end = pin + in_len - 13;
            byte* ii;
            fixed (byte* pwrkmen = wrkmem)
            {
                byte** dict = (byte**)pwrkmen;
                op = pout;
                ip = pin;
                ii = ip;
                ip += 4;
                for (; ; )
                {
                    byte* m_pos;
                    uint m_off;
                    uint m_len;
                    uint dindex;
                    dindex = ((0x21 * (((((((uint)(ip[3]) << 6) ^ ip[2]) << 5) ^ ip[1]) << 5) ^ ip[0])) >> 5) & 0x3fff;
                    m_pos = dict[dindex];
                    if (((uint)m_pos < (uint)pin) ||
                        (m_off = (uint)((uint)ip - (uint)m_pos)) <= 0 ||
                        m_off > 0xbfff)
                        goto literal;
                    if (m_off <= 0x0800 || m_pos[3] == ip[3])
                        goto try_match;
                    dindex = (dindex & 0x7ff) ^ 0x201f;
                    m_pos = dict[dindex];
                    if ((uint)(m_pos) < (uint)(pin) ||
                        (m_off = (uint)((int)((uint)ip - (uint)m_pos))) <= 0 ||
                        m_off > 0xbfff)
                        goto literal;
                    if (m_off <= 0x0800 || m_pos[3] == ip[3])
                        goto try_match;
                    goto literal;
                try_match:
                    if (*(ushort*)m_pos == *(ushort*)ip && m_pos[2] == ip[2])
                        goto match;
                literal:
                    dict[dindex] = ip;
                    ++ip;
                    if (ip >= ip_end)
                        break;
                    continue;
                match:
                    dict[dindex] = ip;
                    if (ip - ii > 0)
                    {
                        uint t = (uint)(ip - ii);
                        if (t <= 3)
                            op[-2] |= (byte)t;
                        else if (t <= 18)
                            *op++ = (byte)(t - 3);
                        else
                        {
                            uint tt = t - 18;
                            *op++ = 0;
                            while (tt > 255)
                            {
                                tt -= 255;
                                *op++ = 0;
                            }
                            *op++ = (byte)tt;
                        }
                        do *op++ = *ii++; while (--t > 0);
                    }
                    ip += 3;
                    if (m_pos[3] != *ip++ || m_pos[4] != *ip++ || m_pos[5] != *ip++ ||
                        m_pos[6] != *ip++ || m_pos[7] != *ip++ || m_pos[8] != *ip++)
                    {
                        --ip;
                        m_len = (uint)(ip - ii);
                        if (m_off <= 0x0800)
                        {
                            --m_off;
                            *op++ = (byte)(((m_len - 1) << 5) | ((m_off & 7) << 2));
                            *op++ = (byte)(m_off >> 3);
                        }
                        else
                        {
                            if (m_off <= 0x4000)
                            {
                                --m_off;
                                *op++ = (byte)(32 | (m_len - 2));
                            }
                            else
                            {
                                m_off -= 0x4000;
                                *op++ = (byte)(16 | ((m_off & 0x4000) >> 11) | (m_len - 2));
                            }
                            *op++ = (byte)((m_off & 63) << 2);
                            *op++ = (byte)(m_off >> 6);
                        }
                    }
                    else
                    {
                        {
                            byte* end = in_end;
                            byte* m = m_pos + 9;
                            while (ip < end && *m == *ip)
                            {
                                m++;
                                ip++;
                            }
                            m_len = (uint)(ip - ii);
                        }
                        if (m_off <= 0x4000)
                        {
                            --m_off;
                            if (m_len <= 33)
                                *op++ = (byte)(32 | (m_len - 2));
                            else
                            {
                                m_len -= 33;
                                *op++ = 32;
                                while (m_len > 255)
                                {
                                    m_len -= 255;
                                    *op++ = 0;
                                }
                                *op++ = (byte)m_len;
                            }
                        }
                        else
                        {
                            m_off -= 0x4000;
                            if (m_len <= 9)
                                *op++ = (byte)(16 | ((m_off & 0x4000) >> 11) | (m_len - 2));
                            else
                            {
                                m_len -= 9;
                                *op++ = (byte)(16 | ((m_off & 0x4000) >> 11));
                                while (m_len > 255)
                                {
                                    m_len -= 255;
                                    *op++ = 0;
                                }
                                *op++ = (byte)m_len;
                            }
                        }
                        *op++ = (byte)((m_off & 63) << 2);
                        *op++ = (byte)(m_off >> 6);
                    }
                    ii = ip;
                    if (ip >= ip_end)
                        break;
                }
            }
            out_len = (uint)(op - pout);
            return (uint)(in_end - ii);
        }
    }
}
