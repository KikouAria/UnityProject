using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EnterCReqDto
{
    public int userid;
}

public class EnterSResDto
{
    public string name;
    public int userid;

    public double[] rotation;
    public double[] point;

    public EnterSResDto()
    {
        rotation = new double[3];
        point = new double[3];
    }
}

public class MoveCReqDto
{
    public double[] rotation;
    public double[] point;

    public MoveCReqDto()
    {
        rotation = new double[3];
        point = new double[3];
    }
}

public class MoveSResDto
{
    public int userid;
    public double[] rotation;
    public double[] point;

    public MoveSResDto()
    {
        rotation = new double[3];
        point = new double[3];
    }
}

public class ExitCReqDto
{
    public int userid;
}

public class ExitSResDto
{
    public int userid;
}

public class AttackCReqDto
{
    public int useridhit;  // 被攻击者
    public double[] pointSrc;
    public double[] pointDest;

    public AttackCReqDto()
    {
        pointSrc = new double[3];
        pointDest = new double[3];
    }
}

public class AttackSResDto
{
    public int useridatk;  // 攻击者
    public int useridhit;  // 被攻击者
    public double[] pointSrc;
    public double[] pointDest;

    public AttackSResDto()
    {
        pointSrc = new double[3];
        pointDest = new double[3];
    }
}