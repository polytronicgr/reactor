﻿// Author:
//       Gabriel Reiser <gabe@reisergames.com>
//
// Copyright (c) 2010-2016 Reiser Games, LLC.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;
using System.Threading.Tasks;
namespace Reactor
{
    public class RLog
    {
        internal static bool Enabled = false;
        internal static string LogPath = REngine.RootPath + "/debug.log";
        internal static object mutex = new object();

        static void WriteLine(string output)
        {
                if(Enabled)
                {
                    StreamWriter Writer = new StreamWriter(new FileStream(LogPath, FileMode.OpenOrCreate));
                    Writer.WriteLine(output);
                    Writer.Flush();
                    Writer.Close();
                    Console.WriteLine(output);
                }
        }
        
        public static void Info(string message)
        {
            string output = String.Format("{0} - {1} : {2}", "INFO", DateTime.Now.ToString(), message);
            WriteLine(output);
            System.Diagnostics.Debug.WriteLine(output);
        }

        public static void Warn(string message)
        {
            string output = String.Format("{0} - {1} : {2}", "WARN", DateTime.Now.ToString(), message);
            WriteLine(output);
            System.Diagnostics.Debug.WriteLine(output);

        }

        public static void Error(string message)
        {
            string output = String.Format("{0} - {1} : {2}", "ERROR", DateTime.Now.ToString(), message);
            WriteLine(output);
            System.Diagnostics.Debug.WriteLine(output);

        }

        public static void Error(Exception e)
        {
            Error(e.Message);
            Error(e.StackTrace);
            if(e.InnerException != null)
                Error(e.InnerException);
        }

        public static void Debug(string message)
        {
#if DEBUG
            string output = String.Format("{0} - {1} : {2}", "DEBUG", DateTime.Now.ToString(), message);
            WriteLine(output);
            System.Diagnostics.Debug.WriteLine(output);
#endif
        }
    }
}

