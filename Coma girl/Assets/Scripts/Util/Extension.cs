using UnityEngine;

namespace Util
{
    /// <summary>
    /// 定义一些扩展的工具方法
    /// </summary>
    public static class Extension
    {
        public static Transform FindAll(this Transform self, string name)
        {
            Transform result = self.Find(name);

            if (result != null) return result;

            for (int i = 0; i < self.childCount;
                i++)
            {
                Transform temp = self.GetChild(i).FindAll(name);
                if (temp != null) return temp;
            }
            return result;
        }




    }
}