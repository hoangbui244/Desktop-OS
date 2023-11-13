using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace NGE
{
    /// <summary>
    /// ���ԣ����棬��ϰ�����
    /// </summary>
    public static class Logger
    {
        [Conditional("DEBUG")]
        public static void TraceException(Exception e)
        {
            //Console.WriteLine(e.Message);
            //Console.WriteLine(e.StackTrace);
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }
        public static void TraceExceptionAlways(Exception e)
        {
            //Console.WriteLine(e.Message);
            //Console.WriteLine(e.StackTrace);
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }
        [Conditional("DEBUG")]
        public static void WriteLine(string str)
        {
            //Console.WriteLine(str);
        }
        [Conditional("DEBUG")]
        public static void WriteLine(string str, params object[] objs)
        {
            //Console.WriteLine(str, objs);
        }
    }
}
