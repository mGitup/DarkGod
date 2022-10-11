
/****************************************************
	文件：PETools.cs
	作者：洛辰
	邮箱: 893271704@qq.com
	日期：2022/06/05 21:57   	
	功能：工具类
*****************************************************/
   public class PETools
    {
		public static int RDInt(int min,int max,System.Random rd = null)
        {
			if(rd == null)
            {
                rd = new System.Random();
            }
            
            //生成包含min和max的随机数
            return rd.Next(min, max + 1); 
        }
    }
