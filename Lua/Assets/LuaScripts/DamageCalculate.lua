--
-- Created by IntelliJ IDEA.
-- User: ASUS
-- Date: 2020/6/9
-- Time: 2:06
-- To change this template use File | Settings | File Templates.
--

local DamageCalculate = {}

function DamageCalculate:re()
    print(" in DamageCalculater")
end

function DamageCalculate:prtry(thisComp, value)
    thisComp.transform.position = thisComp.transform.position +
            thisComp.transform.forward * -0.5;
    local rigi =  thisComp.gameObject:GetComponent("Rigidbody");
    rigi:AddForce(thisComp.transform.forward * -50);

end

function DamageCalculate:Damaged(thisComp, value)
    thisComp.transform.position = thisComp.transform.position +
            thisComp.transform.forward * -0.5;
    local rigi =  thisComp.gameObject:GetComponent("Rigidbody");
    rigi:AddForce(thisComp.transform.forward * -50);

end

function DamageCalculate:Dead(thisComp)

    thisComp.gameObject:SetActive(false)

end


return DamageCalculate

