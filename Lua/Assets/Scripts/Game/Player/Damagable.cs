
using LuaInterface;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Damagable: MonoBehaviour
{
    public int hp = 10;
    private LuaTable damageCalculate;
    private LuaState state;
    private int score;
    
    void Start()
    {
        state = GameController.Inst.state;

    }

    public void OnDamaged(float value)  // 受到伤害时调用
    {
        print("in C# OnDamaged");
        

        // var nameTextObj = transform.Find("HudCanvas");
        // print(nameTextObj);
        // var nameText = nameTextObj.GetComponentInChildren<Text>();
        // print(nameText);
        // nameText.text = "hp: " + hp;

        hp--;


        // 在脚本中执行生命值加减, 死亡等 效果
        damageCalculate = state.DoFile<LuaTable>("DamageCalculate.lua");
        damageCalculate.Call("Damaged", damageCalculate, this);

        // 死了 就调用函数中的死亡方法
        if (hp <= 0) 
        {
            damageCalculate.Call("Dead", damageCalculate, this);
            var countText = GameObject.Find("Canvas").transform.Find("Count Text").GetComponent<Text>();
            int _a;
            score = int.TryParse(countText.text,out _a) ?  int.Parse(countText.text) : 0;
            score++;
            countText.text ="" +  score;
            
        }
        // Rigidbody rigi = (Rigidbody) gameObject.GetComponent("Rigidbody");
        // Rigidbody rigi = (Rigidbody) gameObject.GetComponent<Rigidbody>();
        // rigi.AddExplosionForce(1000, transform.position + transform.forward, 2);
        // rigi.AddForce(transform.forward * -50, ForceMode.Impulse);
        
    }
}
