using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class BinaryDataMgr
{
    /// <summary>
    /// 2进制数据文件路径
    /// </summary>
    public static string DATA_BINARY_PATH = Application.streamingAssetsPath + "/Binary/";

    /// <summary>
    /// 用于存储所有Excel表数据的容器
    /// </summary>
    private Dictionary<string, object> allTableDic = new Dictionary<string, object>();

    /// <summary>
    /// 读取后的数据存储的位置
    /// </summary>
    public static string DATA_PATH = Application.persistentDataPath + "/Data/";


    private static BinaryDataMgr instance = new BinaryDataMgr();

    public static BinaryDataMgr Instance => instance;


    private BinaryDataMgr()
    {

    }

    public void Init()
    {
        //每新创建一张表格，就在这里面去Load
        //格式：LoadTable<数据容器类, 数据结构类>();
        //LoadTable<TowerInfoContainer, TowerInfo>();
    }

    /// <summary>
    /// 得到一张表的数据
    /// </summary>
    /// <typeparam name="T">需要调用数据的容器类名</typeparam>
    /// <returns></returns>
    public T GetTable<T>() where T : class
    {
        string tableName = typeof(T).Name;
        //如果存储的所有表格字典中，存在对应泛型T的Name的键(即数据容器类名）,则返回该键对应的值(值的类型也是字典,也就是表格字典中存储的某一张表)
        if (allTableDic.ContainsKey(tableName)) 
            return allTableDic[tableName] as T;

        return null;
    }


    /// <summary>
    /// 加载二进制数据文件到内存中
    /// </summary>
    /// <typeparam name="T">数据容器类</typeparam>
    /// <typeparam name="K">数据结构类</typeparam>
    public void LoadTable<T, K>()
    {
        using (FileStream fs = File.Open(DATA_BINARY_PATH + typeof(K).Name + ".Dangyi", FileMode.Open, FileAccess.Read))
        {
            //先把所有二进制数据读取到文件流中，以待使用
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();

            int index = 0;

            //读取表格行数（当时存储时声明的）
            int count = BitConverter.ToInt32(bytes, index);
            index += 4;

            //读取主键长度,再用主键长度读取主键名（当时存储时声明的）
            int keyNameLength = BitConverter.ToInt32(bytes, index);
            index += 4;
            string keyName = Encoding.UTF8.GetString(bytes, index, keyNameLength);
            index += keyNameLength;

            //声明容器类的反射信息类
            Type containerType = typeof(T);
            //用容器类的反射信息实例化一个容器类
            object containerObj = Activator.CreateInstance(containerType);
            //声明结构类的反射信息类
            Type classType = typeof(K);
            //用结构类的反射信息类获取其所有成员变量
            FieldInfo[] infos = classType.GetFields();

            //读取每一行的信息
            for(int i = 0; i < count; i++)
            {
                //用结构类的反射信息类实例化一个结构类
                object dataObj = Activator.CreateInstance(classType);
                
                //为所有成员变量轮流取出赋值
                foreach(FieldInfo info in infos)
                {
                    if(info.FieldType == typeof(int))
                    {
                        //
                        info.SetValue(dataObj, BitConverter.ToInt32(bytes, index));
                        index += 4;
                    }
                    else if(info.FieldType == typeof(float))
                    {
                        info.SetValue(dataObj, BitConverter.ToInt32(bytes, index));
                        index += 4;
                    }
                    else if (info.FieldType == typeof(bool))
                    {
                        info.SetValue(dataObj, BitConverter.ToInt32(bytes, index));
                        index += 1;
                    }
                    else if (info.FieldType == typeof(string))
                    {
                        int length = BitConverter.ToInt32(bytes, index);
                        index += 4;
                        info.SetValue(dataObj, Encoding.UTF8.GetString(bytes, index, length));
                        index += length;
                    }
                }

                //获取容器类的反射信息类的成员变量dataDic，将声明的临时object对象指定为dataDic类型
                //并将当前 声明好的containerObj中的dataDic的值 赋值给这个临时object对象
                object dicObject = containerType.GetField("dataDic").GetValue(containerObj);
                //获取dicObject的类型信息，再获取该类型的Add方法
                MethodInfo mInfo = dicObject.GetType().GetMethod("Add");
                //获取结构类的反射信息类的成员变量keyName（如int，float，string），将声明的临时object对象指定为keyName（如int，float，string）类型
                //并将当前 声明好的dataObj中的keyName的值 赋值给这个临时object对象
                object keyValue = classType.GetField(keyName).GetValue(dataObj);
                //调用dicObject中的存在的对应mInfo信息中的方法（即Add），按照键值对格式添加进字典
                mInfo.Invoke(dicObject, new object[] { keyValue, dataObj });
            }

            //把读取完的表记录下来
            allTableDic.Add(typeof(T).Name, containerObj);
        }
    }


    public void Save(object obj,string fileName)
    {
        if (!Directory.Exists(DATA_PATH)) 
            Directory.CreateDirectory(DATA_PATH);

        using (FileStream fs = new FileStream(DATA_PATH + fileName + ".Dangyi" , FileMode.OpenOrCreate , FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }
    }

    public T Load<T>(string fileName) where T : class
    {
        if (!File.Exists(DATA_PATH + fileName + ".Dangyi")) 
            return default(T);

        T obj;
        using (FileStream fs = File.Open(DATA_PATH + fileName + ".Dangyi", FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs) as T;
            fs.Close();
        }

        return obj;
    }
}
