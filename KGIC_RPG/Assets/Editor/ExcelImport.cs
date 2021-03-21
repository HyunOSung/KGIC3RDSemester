using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ExcelImport : AssetPostprocessor {

    static readonly string filePath = "Assets/Editor/Data/RPGData.xlsx";
    static readonly string playerExportPath = "Assets/Resources/Data/PlayerLevelData.asset";
    static readonly string enemyExportPath = "Assets/Resources/Data/EnemyData.asset";


    [MenuItem("Excel/Update RPGData")]
    static public void MakeData()
    {
        Debug.Log("Excel data covert start.");

        MakePlayerData();

        Debug.Log("Excel data covert complete.");
    }

    static void MakePlayerData()
    {
        Data.PlayerLevelData data = ScriptableObject.CreateInstance<Data.PlayerLevelData>();
        AssetDatabase.CreateAsset((ScriptableObject)data, playerExportPath);

        data.hideFlags = HideFlags.NotEditable;

        data.list.Clear();

        using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            IWorkbook book = new XSSFWorkbook(stream);
            ISheet sheet = book.GetSheetAt(0);

            for(int i=2; i<= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                    Data.PlayerLevelData.Attribute a = new Data.PlayerLevelData.Attribute()
                    {
                        level = (int)row.GetCell(0).NumericCellValue,
                        maxHP = (int)row.GetCell(1).NumericCellValue,
                        baseAttack = (int)row.GetCell(2).NumericCellValue,
                        baseDef = (int)row.GetCell(3).NumericCellValue,
                        requireNextLevelExp = (int)row.GetCell(4).NumericCellValue,
                        moveSpeed = (int)row.GetCell(5).NumericCellValue,
                        turnSpeed = (int)row.GetCell(6).NumericCellValue,
                        attackRange = (float)row.GetCell(7).NumericCellValue
                    };

                data.list.Add(a);
            }

            stream.Close();
        }

        ScriptableObject obj = AssetDatabase.LoadAssetAtPath(playerExportPath, typeof(ScriptableObject)) as ScriptableObject;
        EditorUtility.SetDirty(obj);
    }

    static void MakeEnemyData()
    {
        Data.EnemyData data = ScriptableObject.CreateInstance<Data.EnemyData>();
        AssetDatabase.CreateAsset((ScriptableObject)data, enemyExportPath);

        data.hideFlags = HideFlags.NotEditable;

        data.list.Clear();

        using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            IWorkbook book = new XSSFWorkbook(stream);
            ISheet sheet = book.GetSheetAt(0);

            for (int i = 2; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                Data.EnemyData.Attribute a = new Data.EnemyData.Attribute()
                {
                    level = (int)row.GetCell(0).NumericCellValue,
                    maxHP = (int)row.GetCell(1).NumericCellValue,
                    baseAttack = (int)row.GetCell(2).NumericCellValue,
                    baseDef = (int)row.GetCell(3).NumericCellValue,
                    moveSpeed = (int)row.GetCell(4).NumericCellValue,
                    turnSpeed = (int)row.GetCell(5).NumericCellValue,
                    attackRange = (float)row.GetCell(6).NumericCellValue
                };

                data.list.Add(a);
            }

            stream.Close();
        }

        ScriptableObject obj = AssetDatabase.LoadAssetAtPath(enemyExportPath, typeof(ScriptableObject)) as ScriptableObject;
        EditorUtility.SetDirty(obj);
    }
}


