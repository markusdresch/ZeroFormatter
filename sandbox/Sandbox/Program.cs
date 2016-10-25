﻿using Sandbox;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using System.IO;

[ZeroFormattable]
public class MyClass
{
    [Index(0)]
    public virtual int Age { get; set; }

    [Index(1)]
    public virtual string FirstName { get; set; }

    [Index(2)]
    public virtual string LastName { get; set; }

    [IgnoreFormat]
    public string FullName { get { return FirstName + LastName; } }

    [Index(3)]
    public virtual IList<MogeMoge> List { get; set; }

    [Index(4)]
    public virtual MogeMoge Mone { get; set; }


}

namespace Sandbox
{
    public enum MogeMoge
    {
        Apple, Orange
    }



    public enum MyEnum
    {
        Furit, Alle
    }
    class Program
    {
        static int IntGetHashCode(int x) { return x; }
        static Int32 Identity(Int32 x) { return x; }

        static void Hoge<T>(T t)
        {

            Func<int, int> getHash = IntGetHashCode;

            //
            var aaa = getHash.GetMethodInfo().CreateDelegate(typeof(Func<T, int>));
            //var GetHashCodeDelegate = Expression.Lambda<Func<T, int>>(Expression.Call(getHash.GetMethodInfo())).Compile();



            Func<Int32, Int32> identity = Identity;
            var serializeCast = identity.GetMethodInfo().CreateDelegate(typeof(Func<T, Int32>)) as Func<T, Int32>;



            Console.WriteLine(serializeCast(t));

        }

        static byte[] Huga<T>(T obj)
        {
            return null;
        }

        static void HUga<T>()
        {
            //Func<T, byte[]> invoke = Huga<T>;
            //Func<object, byte[]> invoke2 = invoke;



        }

        static void Main(string[] args)
        {

            var mc = new MyClass
            {
                Age = 222,
                FirstName = "a",
                LastName = "bbb",
                List = new[] { MogeMoge.Orange, MogeMoge.Apple },
                Mone = MogeMoge.Orange
            };

            var bytes1 = ZeroFormatterSerializer.NonGeneric.Serialize(typeof(MyClass), mc);

            mc.Age = 999;
            mc.FirstName = "hugahuga";
            var bytes2 = ZeroFormatterSerializer.NonGeneric.Serialize(typeof(MyClass), mc);

            mc.Age = 3;
            mc.LastName = "tetete";
            mc.List[1] = MogeMoge.Orange;
            mc.Mone = MogeMoge.Apple;
            var bytes3 = ZeroFormatterSerializer.NonGeneric.Serialize(typeof(MyClass), mc);


            var block = bytes1.Concat(bytes2).Concat(bytes3).ToArray(); // slow:)

            var a = ZeroFormatterSerializer.Deserialize<MyClass>(block);
            var b = ZeroFormatterSerializer.Deserialize<MyClass>(block, bytes1.Length);
            var c = ZeroFormatterSerializer.Deserialize<MyClass>(block, bytes1.Length + bytes2.Length);


        }
    }
}

