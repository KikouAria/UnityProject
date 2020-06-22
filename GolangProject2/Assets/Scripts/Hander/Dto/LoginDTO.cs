using UnityEngine;
using System.Collections;

public class RegCReqDto
{
    public string username { get; set; }
    public string password { get; set; }
}

public class RegSResDto
{
    public string name;
    public bool result;
    public bool isMonster;

}

public class LoginCReqDto 
{
    public string username { get; set; }
    public string password { get; set; }
}

public class LoginSResDto
{
    public string name;
    public int userid;
    public int result;
    public bool isMonster;
}
