using UnityEngine;

namespace Assets.Scripts.Common
{
    /// <summary>
    /// 脚本单例类
    /// 这个类为所有管理类的父类, 用于整合这些子管理类共同代码 下面的T表示该类的子类的类型
    /// 功能: 只要脚本类继承了这个类, 那么无论在任何地方, 都能直接 类名.Instance.Func()来调用函数 而不用FindComponent. (甚至脚本没挂在物体上也能点出来)
    /// 适用性: 场景中存在唯一的对象, 即可让该对象继承当前类
    /// 如何使用
    ///     继承时必须传递子类自己到中括号里  
    ///     任意脚本中, 都能通过子类类型访问Instance属性, 从而在任何地方调用其方法
    ///     child.Instance.父类方法() 即可通过儿子执行父类的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>  // 这里对T进行约束, 必须是自己或者自己的子类. 这样就能在编译上确定父子关系. (对泛型T的约束, 需要根据代码随时改变) 
    {

        private static T _instance;  // 这个值存着继承这个类的子管理类自身

        public static T Instance  // 由于子管理类自身经常被调用, 因此将其公开出来以避免过多的Find操作
        {
            get {
                if (_instance == null)
                    _instance = FindObjectOfType<T>(); // 将子类自己整个脚本类赋给_instance, 一会返回
                if (_instance == null)  // 如果没有, 说明脚本还没挂到物体上
                    // 那么就创建一个名为Singleton+类名空对象, 将脚本挂上去(立即执行下面的Awake)
                    _instance = new GameObject("Singleton of" + typeof(T)).AddComponent<T>();
                else  // 如果创建空物体挂上去, 那么那个瞬间就会执行Awake, 同时Awake进行了Init执行, 那么这里就不要重复执行
                    _instance.Init();  // 执行初始化

                return _instance;
            }
        }

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;   // 如果将脚本挂在游戏对象上并且触发了Awake事件, 那么此时脚本自身就是this了, 将其赋值给_instance
                Init();
            }
        }

        public virtual void Init() { }
    }
}