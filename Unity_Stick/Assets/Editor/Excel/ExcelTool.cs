using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;
using System.Data;
using Excel;
using System.Text;

public class ExcelTool
{
    /// <summary>
    /// Excel文件路径
    /// </summary>
    public static string EXCEL_PATH = Application.dataPath + "/ArtRes/Excel";

    /// <summary>
    /// 数据结构类文件路径
    /// </summary>
    public static string DATA_CLASS_PATH = Application.dataPath + "/Scripts/ExcelData/DataClass/";

    /// <summary>
    /// 数据容器类
    /// </summary>
    public static string DATA_CONTAINER_PATH = Application.dataPath + "/Scripts/ExcelData/Container/";


    

    private static int BEGIN_INDEX = 4;

    [MenuItem("/GameTool/GenerateExcel")]
    private static void GenerateExcelInfo()
    {
        //确保EXCEL_PATH指定的目录存在，若不存在则创建该目录，避免后续文件操作因目录不存在而报错。
        DirectoryInfo dInfo = Directory.CreateDirectory(EXCEL_PATH);
        //获取Excel目录下的所有文件信息，也就是得到其中所有的Excel表
        FileInfo[] files = dInfo.GetFiles();
        //预先声明一个数据表容器，用于暂存从 Excel 文件中读取的数据表集合。
        DataTableCollection tableCollection;

        for(int i = 0; i < files.Length; i++)
        {
            //判断文件扩展名，筛选出 Excel 文件，非 Excel 文件则跳过处理。
            if (files[i].Extension != ".xlsx" &&
                files[i].Extension != ".xls")
                continue;

            //使用文件流打开 Excel 文件
            using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read))
            {
                //通过ExcelReaderFactory创建读取器，将 Excel 文件内容转换为数据集
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                //并获取其中的所有数据表（DataTable）存入tableCollection
                tableCollection = excelReader.AsDataSet().Tables;
                //最后关闭文件流
                fs.Close();
            }

            foreach(DataTable table in tableCollection)
            {
                //生成数据结构类脚本
                GenerateExcelDataClass(table);
                //生成数据容器类脚本
                GenerateExcelContainer(table);
                //生成数据文件
                GenerateExcelBinary(table);
            }
        }
    }


    /// <summary>
    /// 生成Excel表对应的数据结构类
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelDataClass(DataTable table)
    {
        //字段名行
        DataRow rowName = GetVarialbleNameRow(table);
        //字段类型行
        DataRow rowType = GetVarialbleTypeRow(table);

        //判断路径是否存在 否则就创建对应文件夹
        if (!Directory.Exists(DATA_CLASS_PATH)) 
            Directory.CreateDirectory(DATA_CLASS_PATH);

        //创建对应的数据结构类脚本，其实就是字符串拼接，再存进文件，后缀改为.cs
        string str = "public class" + " " + table.TableName + "\n{\n";
        
        //变量进行字符串拼接
        for(int i = 0; i < table.Columns.Count; i++)
        {
            str += "    public " + rowType[i].ToString() + " " + rowName[i].ToString() + ";\n";
        }

        str += "}";

        //将拼接好的字符串传到新创建的.cs脚本中
        File.WriteAllText(DATA_CLASS_PATH + table.TableName + ".cs", str);

        //自动刷新
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成数据结构对应的数据容器类
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelContainer(DataTable table)
    {
        //获取键对应的索引(即哪一列)
        int keyIndex = GetKeyIndex(table);

        DataRow rowType = GetVarialbleTypeRow(table);

        if (!Directory.Exists(DATA_CONTAINER_PATH))
            Directory.CreateDirectory(DATA_CONTAINER_PATH);

        string str = "using System.Collections.Generic;\n";

        str += "\n";

        str += "public class " + table.TableName + "Container" + "\n{\n";

        str += "    " + "public Dictionary<" + rowType[keyIndex].ToString() + "," + table.TableName + ">";

        str += " dataDic = new Dictionary<" + rowType[keyIndex].ToString() + "," + table.TableName + ">();\n";

        str += "}";

        File.WriteAllText(DATA_CONTAINER_PATH + table.TableName + "Container.cs", str);

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成Excel对应的数据文件
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateExcelBinary(DataTable table)
    {
        //若没有对应路径，则创建
        if (!Directory.Exists(BinaryDataMgr.DATA_BINARY_PATH))
            Directory.CreateDirectory(BinaryDataMgr.DATA_BINARY_PATH);

        //声明一个对应数据结构类名的数据文件
        using (FileStream fs = new FileStream(BinaryDataMgr.DATA_BINARY_PATH + table.TableName + ".Dangyi", FileMode.OpenOrCreate, FileAccess.Write))
        {
            //先写入存入数据表的行数，方便读取
            fs.Write(BitConverter.GetBytes(table.Rows.Count - 4), 0, 4);

            //存入键值名的长度
            string keyName = GetVarialbleNameRow(table)[GetKeyIndex(table)].ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(keyName);
            fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            fs.Write(bytes, 0, bytes.Length);

            DataRow row;
            //先将对应的类型行取出，便于之后存每列的值时，判断其类型
            DataRow rowType = GetVarialbleTypeRow(table);

            //遍历BEGIN_INDEX之后的所有行(即开始写数据对象的有效行)
            for (int i = BEGIN_INDEX; i < table.Rows.Count; i++)
            {
                //取出当前行
                row = table.Rows[i];

                //遍历当前行的每一列
                for(int j = 0; j < table.Columns.Count; j++)
                {
                    //判断每列的类型
                    switch (rowType[j].ToString())
                    {
                        case "int":
                            fs.Write(BitConverter.GetBytes(int.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "float":
                            fs.Write(BitConverter.GetBytes(float.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "bool":
                            fs.Write(BitConverter.GetBytes(bool.Parse(row[j].ToString())), 0, 1);
                            break;
                        case "string":
                            bytes = Encoding.UTF8.GetBytes(row[j].ToString());

                            fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);

                            fs.Write(bytes, 0, bytes.Length);
                            break;
                    }
                }
                
            }
            //关闭流
            fs.Close();
        }
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// 获取变量名所在行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVarialbleNameRow(DataTable table)
    {
        return table.Rows[0];
    }

    /// <summary>
    /// 获取变量类型所在行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVarialbleTypeRow(DataTable table)
    {
        return table.Rows[1];
    }

    /// <summary>
    /// 获取主键索引
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static int GetKeyIndex(DataTable table)
    {
        DataRow row = table.Rows[2];

        for (int i = 0; i < table.Columns.Count; i++) 
        {
            if (row[i].ToString() == "key")
            {
                return i;
            }
        }
        return 0;
    }
}
